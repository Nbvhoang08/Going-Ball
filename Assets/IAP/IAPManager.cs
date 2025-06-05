using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine.Purchasing.Extension;
using Hapiga.Ads;
using UnityEngine.Events;
namespace HapigaIAP
{
    public class IAPManager : MonoBehaviour, IDetailedStoreListener
    {
        public static IAPManager Instance { get; private set; }

        public static Action OnInitSuccess { get; set; }
        public static Action<string> OnPurchaseSuccess { get; set; }
        public static Action<SubscriptionInfo> CheckSubScriptionInfo { get; set; }

        public IAPConfigs iapConfigs;

        // Unity IAP objects
        private IStoreController m_Controller;

        private IAppleExtensions m_AppleExtensions;

        private ConfigurationBuilder builder;

        private bool isIniting = false;
        private bool isBuilderDone = false;
        private bool isPurchaseProcessing = false;
        UnityAction callBackReward;
        private void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        async void Start()
        {
            if (Instance != this)
                return;

            var module = StandardPurchasingModule.Instance();
            // The FakeStore supports: no-ui (always succeeding), basic ui (purchase pass/fail), and
            // developer ui (initialization, purchase, failure code setting). These correspond to
            // the FakeStoreUIMode Enum values passed into StandardPurchasingModule.useFakeStoreUIMode.
            module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

            builder = ConfigurationBuilder.Instance(module);

            var options = new InitializationOptions().SetEnvironmentName("production");
            await UnityServices.InitializeAsync(options);
            Debug.Log("init iap UnityServices InitializeAsync " + Time.time);

#if UNITY_ANDROID || UNITY_IOS
            StartCoroutine(CrInitializeIAP());
#endif
        }

        public string GetPrice(string productId)
        {
            //Debug.LogError("-----------" + id + m_Controller == null + " | " );
            if (IsInitialized() == false)
                return "?";

            //Debug.LogError("-----------" + id + m_Controller == null + " | " + m_Controller.products == null);
            if (m_Controller == null || m_Controller.products == null)
                return "?";

            var product = m_Controller.products.WithID(productId);
            if (product == null || (!product.availableToPurchase))
                return "?";

            return product.metadata.localizedPriceString;
        }

        public void BuyProduct(string productId,UnityAction callBackReward)
        {
            this.callBackReward = callBackReward;
            Debug.Log("buy " + productId);
            // If the stores throw an unexpected exception, use try..catch to protect my logic here.
            try
            {
                // If Purchasing has been initialized ...
                if (IsInitialized() == false)
                {
                    Debug.Log("IsInitialized FAIL");

                    ManualInitIAP();

                    //PopupConfirm.ShowPanel(new PopupConfirmData() { content = "NetworkError!\nPlease_try_again!" });

                    return;
                }

                // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
                var product = m_Controller.products.WithID(productId);

                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product == null || product.availableToPurchase == false)
                {
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                    //PopupConfirm.ShowPanel(new PopupConfirmData() { content = StringConstants.ProducNotAvailable });
                    return;
                }
                //ngan show open ads
                AdManager.Instance.OnInterStarted();
#if UNITY_EDITOR
                ProcessPurchaseSuccess(product);
                return;
#endif
                isPurchaseProcessing = true;
                m_Controller.InitiatePurchase(product);

            }
            // Complete the unexpected exception handling ...
            catch (Exception e)
            {
                isPurchaseProcessing = false;
                //PopupConfirm.ShowPanel(new PopupConfirmData() { content = "Error: " + e.Message });
                // ... by reporting any unexpected exception for later diagnosis.
                Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
                //GameAnalytics.LogInAppPurchaseException(productId, e.Message);
            }
        }

        private IEnumerator CrInitializeIAP()
        {
            if (IsInitialized()) yield break;

            Debug.Log("init iap");
            WaitForSeconds wait = new WaitForSeconds(5);

            GetBuilder();

            // Now we're ready to initialize Unity IAP.
            int tryCount = 10;

            while (tryCount > 0 && IsInitialized() == false)
            {
                tryCount--;

                Debug.Log("init iap tryCount " + tryCount);
                UnityPurchasing.Initialize(this, builder);

                yield return wait;
            }
        }

        private void GetBuilder()
        {
            if (isBuilderDone)
                return;

            isBuilderDone = true;

            foreach (var item in iapConfigs.ProductDetailses)
            {
                if (string.IsNullOrEmpty(item.Id) == false)
                {
                    Debug.Log("init IAP " + item.Id + "  isConsumable " + item.productType);
                    builder.AddProduct(item.Id.Trim(), item.productType);
                }
            }
        }

        void GetProductsInfor(IStoreController controller)
        {
            for (int i = 0; i < iapConfigs.ProductDetailses.Length; i++)
            {
                Product product = controller.products.WithID(iapConfigs.ProductDetailses[i].Id);
                if (product != null && product.availableToPurchase)
                {
                    ProductMetadata metadata = product.metadata;
                    iapConfigs.ProductDetailses[i].localizedPrice = metadata.localizedPriceString;
                    iapConfigs.ProductDetailses[i].currencyCode = metadata.isoCurrencyCode;
                }
            }
        }
        public ProductDetails GetProductDetailsById(string id)
        {
            foreach (ProductDetails pd in iapConfigs.ProductDetailses)
            {
                if (pd.Id == id)
                {
                    return pd;
                }
            }

            return null;
        }
        private void ManualInitIAP()
        {
            if (isIniting)
                return;

            if (IsInitialized())
                return;

            Debug.Log("manual init iap");

            GetBuilder();

            isIniting = true;
            UnityPurchasing.Initialize(this, builder);
        }

        public bool IsInitialized()
        {
            return m_Controller != null;
        }

        public void OnClickRestore()
        {
            // If Purchasing has not yet been set up ...
            if (IsInitialized() == false)
            {
                ManualInitIAP();
                // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
                Debug.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
            {
                m_AppleExtensions.RestoreTransactions(OnTransactionsRestored);
            }
            // Otherwise ...
            else
            {
                // We are not running on an Apple device. No work is necessary to restore purchases.
                Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }

        /// <summary>
        /// This will be called after a call to IAppleExtensions.RestoreTransactions().
        /// </summary>
        private void OnTransactionsRestored(bool success, string result)
        {
            Debug.Log("Transactions restored." + success + " " + result);

            if(!HasRestored())
            {
                iapConfigs.OnRestore();
                SaveRestoreStatus();
            }
        }

        /// <summary>
        /// iOS Specific.
        /// This is called as part of Apple's 'Ask to buy' functionality,
        /// when a purchase is requested by a minor and referred to a parent
        /// for approval.
        ///
        /// When the purchase is approved or rejected, the normal purchase events
        /// will fire.
        /// </summary>
        /// <param name="item">Item.</param>
        private void OnDeferred(Product item)
        {
            Debug.Log("Purchase deferred: " + item.definition.id);
        }

        private void OnCheckSubScription(Product item)
        {
            var introductory_info_dict = m_AppleExtensions.GetIntroductoryPriceDictionary();

            if (item == null) return;

            if (item.definition.type == ProductType.Subscription)
            {
                if (item.availableToPurchase)
                {
                    if (item.receipt != null)
                    {
                        if (CheckIfProductIsAvailableForSubscriptionManager(item.receipt))
                        {
                            string intro_json = (introductory_info_dict == null || !introductory_info_dict.ContainsKey(item.definition.storeSpecificId)) ? null : introductory_info_dict[item.definition.storeSpecificId];
                            SubscriptionManager p = new SubscriptionManager(item, intro_json);
                            SubscriptionInfo info = p.getSubscriptionInfo();

                            Debug.Log("product id is: " + info.getProductId());
                            Debug.Log("purchase date is: " + info.getPurchaseDate());
                            Debug.Log("subscription next billing date is: " + info.getExpireDate());
                            Debug.Log("is subscribed? " + info.isSubscribed().ToString());
                            Debug.Log("is expired? " + info.isExpired().ToString());
                            Debug.Log("is cancelled? " + info.isCancelled());
                            Debug.Log("product is in free trial peroid? " + info.isFreeTrial());
                            Debug.Log("product is auto renewing? " + info.isAutoRenewing());
                            Debug.Log("subscription remaining valid time until next billing date is: " + info.getRemainingTime());
                            Debug.Log("is this product in introductory price period? " + info.isIntroductoryPricePeriod());
                            Debug.Log("the product introductory localized price is: " + info.getIntroductoryPrice());
                            Debug.Log("the product introductory price period is: " + info.getIntroductoryPricePeriod());
                            Debug.Log("the number of product introductory price period cycles is: " + info.getIntroductoryPricePeriodCycles());

                            if (CheckSubScriptionInfo != null && info != null)
                                CheckSubScriptionInfo(info);
                        }
                    }
                }
            }

        }

        private bool CheckIfProductIsAvailableForSubscriptionManager(string receipt)
        {
            var receipt_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
            if (!receipt_wrapper.ContainsKey("Store") || !receipt_wrapper.ContainsKey("Payload"))
            {
                Debug.Log("The product receipt does not contain enough information");
                return false;
            }
            var store = (string)receipt_wrapper["Store"];
            var payload = (string)receipt_wrapper["Payload"];

            if (payload != null)
            {
                switch (store)
                {
                    case GooglePlay.Name:
                        {
                            var payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
                            if (!payload_wrapper.ContainsKey("json"))
                            {
                                Debug.Log("The product receipt does not contain enough information, the 'json' field is missing");
                                return false;
                            }
                            var original_json_payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode((string)payload_wrapper["json"]);
                            if (original_json_payload_wrapper == null || !original_json_payload_wrapper.ContainsKey("developerPayload"))
                            {
                                Debug.Log("The product receipt does not contain enough information, the 'developerPayload' field is missing");
                                return false;
                            }
                            var developerPayloadJSON = (string)original_json_payload_wrapper["developerPayload"];
                            var developerPayload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(developerPayloadJSON);
                            if (developerPayload_wrapper == null || !developerPayload_wrapper.ContainsKey("is_free_trial") || !developerPayload_wrapper.ContainsKey("has_introductory_price_trial"))
                            {
                                Debug.Log("The product receipt does not contain enough information, the product is not purchased using 1.19 or later");
                                return false;
                            }
                            return true;
                        }
                    case AppleAppStore.Name:
                    case AmazonApps.Name:
                    case MacAppStore.Name:
                        {
                            return true;
                        }
                    default:
                        {
                            return false;
                        }
                }
            }
            return false;
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("iap success to initialize!");
            m_Controller = controller;
            m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
            //m_GooglePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();

            // On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
            // On non-Apple platforms this will have no effect; OnDeferred will never be called.
            m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);
            GetProductsInfor(m_Controller);
            OnInitSuccess?.Invoke();

            foreach (var item in controller.products.all)
            {
                //Debug.Log("iap item " + item.definition.id);
                OnCheckSubScription(item);
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            isIniting = false;
            Debug.Log("iap failed to initialize!");
            switch (error)
            {
                case InitializationFailureReason.AppNotKnown:
                    Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
                    break;
                case InitializationFailureReason.PurchasingUnavailable:
                    // Ask the user if billing is disabled in device settings.
                    Debug.Log("Billing disabled!");
                    break;
                case InitializationFailureReason.NoProductsAvailable:
                    // Developer configuration error; check product metadata.
                    Debug.Log("No products available for purchase!");
                    break;
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            isIniting = false;
            Debug.Log("iap failed to initialize!" + message);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log("Purchase failed: " + product.definition.id + "  " + failureReason);
            isPurchaseProcessing = false;
            //PopupConfirm.ShowPanel(new PopupConfirmData() { title = "Error !", content = "Purchase failed: " + failureReason });
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.Log("Purchase failed: " + product.definition.id + "  " + failureDescription.reason);
            isPurchaseProcessing = false;
            //PopupConfirm.ShowPanel(new PopupConfirmData() { title = "Purchase Failed !", content = failureDescription.reason + " " + failureDescription.message });
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            float gPrice = (float)e.purchasedProduct.metadata.localizedPrice;
            string gCurrency = e.purchasedProduct.metadata.isoCurrencyCode;

            try
            {
                //Debug.Log("needValidate: " + needValidate);
                Debug.Log("Purchase OK: " + e.purchasedProduct.definition.id);
                Debug.Log("Receipt: " + e.purchasedProduct.receipt);

                bool needValidate = isPurchaseProcessing;
                isPurchaseProcessing = false;

#if UNITY_EDITOR
                needValidate = false;
#endif
                if (needValidate == false)
                {
                    // Truong hop restore purchase
                    ProcessPurchaseSuccess(e.purchasedProduct, false);

                    return PurchaseProcessingResult.Complete;
                }

                // Test edge case where product is unknown
                if (e.purchasedProduct == null)
                {
                    Debug.Log("Attempted to process purchase with unknown product. Ignoring");
                    OnPurchaseFailed(e.purchasedProduct, PurchaseFailureReason.ProductUnavailable);
                    return PurchaseProcessingResult.Complete;
                }

                // Test edge case where purchase has no receipt
                if (string.IsNullOrEmpty(e.purchasedProduct.receipt))
                {
                    Debug.Log("Attempted to process purchase with no receipt: ignoring");
                    OnPurchaseFailed(e.purchasedProduct, PurchaseFailureReason.SignatureInvalid);
                    return PurchaseProcessingResult.Complete;
                }

                bool validPurchase = true; // Presume valid for platforms with no R.V.

                // Unity IAP's validation logic is only included on these platforms.
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
                // Prepare the validator with the secrets we prepared in the Editor
                // obfuscation window.
                var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
                    AppleTangle.Data(), Application.identifier);

                try
                {
                    // On Google Play, result has a single product ID.
                    // On Apple stores, receipts contain multiple products.
                    var result = validator.Validate(e.purchasedProduct.receipt);
                    // For informational purposes, we list the receipt(s)
                    Debug.Log("Receipt is valid. Contents:");
                    foreach (IPurchaseReceipt productReceipt in result)
                    {
                        Debug.Log(productReceipt.productID);
                        Debug.Log(productReceipt.purchaseDate);
                        Debug.Log(productReceipt.transactionID);
                    }

                    double price = (double)e.purchasedProduct.metadata.localizedPrice;
                    string priceName = e.purchasedProduct.metadata.isoCurrencyCode;
                    string productId = e.purchasedProduct.definition.id;
                    string transactionID = e.purchasedProduct.transactionID;
                    string receipt = e.purchasedProduct.receipt;
                    AdjustEvents.TrackPurchase(price, priceName, productId, transactionID, receipt);
                }
                catch (IAPSecurityException)
                {
                    Debug.Log("Invalid receipt, not unlocking content");
                    validPurchase = false;
                }
#endif

                if (validPurchase)
                {
                    // Unlock the appropriate content here.
                    ProcessPurchaseSuccess(e.purchasedProduct);
                }
                else
                {
                    OnPurchaseFailed(e.purchasedProduct, PurchaseFailureReason.SignatureInvalid);
                }

                return PurchaseProcessingResult.Complete;
                //return PurchaseProcessingResult.Pending;

            }
            catch (Exception ex)
            {
                Debug.Log("IAP Exception " + ex.Message);
                //GameAnalytics.LogInAppPurchaseException(e.purchasedProduct.definition.id, ex.Message);
            }

            return PurchaseProcessingResult.Complete;
        }

        private void ProcessPurchaseSuccess(Product product, bool isValidate = true)
        {
            Debug.Log("=======> ProcessEventSuccess <====== " + product.definition.id);
#if !UNITY_EDITOR
            OnCheckSubScription(product);
#endif
            if (isValidate)
            {
                //LogIAPPurchase(product);
                if (callBackReward != null)
                {
                    callBackReward();
                    callBackReward = null;
                }
                //iapConfigs.RewardItem(product.definition.id);
                //PopupConfirm.ShowPanel(new PopupConfirmData() { content = StringConstants.PurchaseSuccess });
            }
            else
            {
                //int count = GetIAPCount(product.definition.id);
                //if (count == 0)
                //{
                //    LogIAPPurchase(product);
                //    //database.RewardItem(product.definition.id);
                //}

                //restore
            }

            OnPurchaseSuccess?.Invoke(product.definition.id);
        }

        const string keyRestored = "IsRestored";
        bool HasRestored()
        {
            int isRestored = PlayerPrefs.GetInt(keyRestored, 0);
            return isRestored == 1;
        }
        void SaveRestoreStatus()
        {
            PlayerPrefs.SetInt(keyRestored, 1);
            PlayerPrefs.Save();
        }
    }

}