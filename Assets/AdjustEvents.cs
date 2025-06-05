using AdjustSdk;
using Hapiga.Ads;
using System;
using UnityEngine;

[Serializable]
public class IdAdjust
{
    public string idAndroid;
    public string idIOS;

    public string GetIdPlatform()
    {
#if UNITY_ANDROID
        return idAndroid;
#elif UNITY_IPHONE
            return idIOS;
#endif
        return "";
    }
}
public class AdjustEvents : MonoBehaviour
{
    [Space]
    [SerializeField]
    IdAdjust tokenId;
    [SerializeField]
    AdjustEnvironment adjustEnvironment;
    [SerializeField]
    IdAdjust iapEvent;

    static string iapEventName = "g2mbc8";

    public void ManualInitAdjust()
    {
        AdManager.Instance.PaidEvent += PaidEvent;
        iapEventName = iapEvent.GetIdPlatform();
        AdjustConfig adjustConfig = new AdjustConfig(tokenId.GetIdPlatform(), adjustEnvironment);
        Adjust.InitSdk(adjustConfig);
    }
    public static void TrackPurchase(double price, string priceName, string productId, string transactionId, string receipt)
    {
        AdjustEvent adjustEvent = new AdjustEvent(iapEventName);
        adjustEvent.SetRevenue(price * 0.8f, priceName);
        adjustEvent.ProductId = productId;
        adjustEvent.TransactionId = transactionId;
        adjustEvent.PurchaseToken = receipt;
        Adjust.TrackEvent(adjustEvent);
    }
    private void PaidEvent(MaxSdkBase.AdInfo adInfo)
    {
        var adRevenue = new AdjustAdRevenue("applovin_max_sdk");
        adRevenue.SetRevenue(adInfo.Revenue, "USD");
        adRevenue.AdRevenueNetwork = adInfo.NetworkName;
        adRevenue.AdRevenueUnit = adInfo.AdUnitIdentifier;
        adRevenue.AdRevenuePlacement = adInfo.Placement;

        Adjust.TrackAdRevenue(adRevenue);
    }
}