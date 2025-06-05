using UnityEngine;
using System;
using System.Collections.Generic;
#if FIREBASE_ANALYTIC
using Hapiga.Tracking;
#endif

namespace Hapiga.Ads
{
    public class ApplovinModule : IAdsModule
    {
        private Action onRewardAdsClosed;
        private Action onInterAdsClosed;
        private Action onAppOpenAdsClosed;
        private bool IsRewarded;
        private AdManager adManger;
        private Dictionary<AdsType, string> adsUnitIds;

        private string place = "placement";

        public void Init(AdManager _adManger, Dictionary<AdsType, string> androidUnitIds,
            Dictionary<AdsType, string> iosUnitIds, params string[] others)
        {
            adManger = _adManger;
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                adsUnitIds = iosUnitIds;
            }
            else
            {
                adsUnitIds = androidUnitIds;
            }

            Debug.Log("Init Applovin");
#if APPLOVIN_MAX
            string sdkKey = others[0];
            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {
                if (adManger.adsConfig.isTestAds)
                {
                    MaxSdk.ShowMediationDebugger();
                }

                LoadAppOpenAd();
                InitializeBannerAds();
                LoadInterstitial();
                LoadRewardedAd();
                adManger.ManualInitMMP();
            };
            //MaxSdk.SetSdkKey(sdkKey);
            if (adManger.adsConfig.isTestAds)
            {
                MaxSdk.SetVerboseLogging(true);
            }

            MaxSdk.InitializeSdk();
#if UNITY_EDITOR
            MaxSdk.DisableStubAds();
#endif

            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
            MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnAppOpenLoadFailedEvent;
            MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnAppOpenLoadEvent;
            MaxSdkCallbacks.AppOpen.OnAdDisplayedEvent += OnAppOpenDisplayEvent;
            MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += OnAppOpenDisplayFailedEvent;


            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += (x, y) => OnAdRevenuePaidEvent(x, y, AdsType.Rewarded);
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent +=
                (x, y) => OnAdRevenuePaidEvent(x, y, AdsType.Interstitial);
            MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += (x, y) => OnAdRevenuePaidEvent(x, y, AdsType.AppOpen);
#endif
        }

#if APPLOVIN_MAX
        private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo impressionData, AdsType adsType)
        {
            double revenue = impressionData.Revenue;
            this.adManger.OnAdsRevPaid(impressionData, adUnitId, adsType);
#if FIREBASE_ANALYTIC
            var impressionParameters = new[]
            {
                new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
                new Firebase.Analytics.Parameter("ad_source", impressionData.NetworkName),
                new Firebase.Analytics.Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
                new Firebase.Analytics.Parameter("ad_format", impressionData.AdFormat),
                new Firebase.Analytics.Parameter("ad_creative", impressionData.CreativeIdentifier),
                new Firebase.Analytics.Parameter("country", MaxSdk.GetSdkConfiguration().CountryCode),
                new Firebase.Analytics.Parameter("value", revenue),
                new Firebase.Analytics.Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
            };
            // TrackAdRevenue(impressionData);
            TrackingManager.TrackEvent("custom_ad_impression", impressionParameters);
            TrackingManager.TrackEvent("ad_impression", impressionParameters);
#endif
        }
        //
        // private void TrackAdRevenue(MaxSdkBase.AdInfo adInfo)
        // {
        //     AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
        //
        //     adjustAdRevenue.setRevenue(adInfo.Revenue, "USD");
        //     adjustAdRevenue.setAdRevenueNetwork(adInfo.NetworkName);
        //     adjustAdRevenue.setAdRevenueUnit(adInfo.AdUnitIdentifier);
        //     adjustAdRevenue.setAdRevenuePlacement(adInfo.Placement);
        //
        //     Adjust.trackAdRevenue(adjustAdRevenue);
        // }
#endif

        public void CheckLoadAds()
        {
#if APPLOVIN_MAX
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                if (!MaxSdk.IsInterstitialReady(adsUnitIds[AdsType.Interstitial]))
                {
                    LoadInterstitial();
                }

                if (!MaxSdk.IsRewardedAdReady(adsUnitIds[AdsType.Rewarded]))
                {
                    LoadRewardedAd();
                }

                if (adManger.adsConfig.should_show_ads_open && !MaxSdk.IsAppOpenAdReady(adsUnitIds[AdsType.AppOpen]))
                {
                    LoadAppOpenAd();
                }
            }
#endif
        }

        #region Interstitial

        private void LoadInterstitial()
        {
#if APPLOVIN_MAX
            if (adManger.adsConfig.should_show_inter || adManger.adsConfig.is_show_inter_when_no_reward)
            {
                Debug.Log("Applovin Load Inter");
                MaxSdk.LoadInterstitial(adsUnitIds[AdsType.Interstitial]);
#if FIREBASE_ANALYTIC
                TrackingManager.TrackEvent("AdsInter_CallLoad");
#endif
            }
#endif
        }

        public bool IsInterstitalAdsLoaded()
        {
#if APPLOVIN_MAX
            return MaxSdk.IsInterstitialReady(adsUnitIds[AdsType.Interstitial]);
#endif
            return false;
        }

        public bool ShowInterAds(Action _onInterClosed, string _placement)
        {
#if APPLOVIN_MAX
            Debug.Log("show inter: " + _placement);
            bool isshow = false;
            if (MaxSdk.IsInterstitialReady(adsUnitIds[AdsType.Interstitial]))
            {
                MaxSdk.ShowInterstitial(adsUnitIds[AdsType.Interstitial], _placement);
                adManger.OnInterStarted();
                onInterAdsClosed = _onInterClosed;
                isshow = true;
#if FIREBASE_ANALYTIC
                TrackingManager.TrackEvent("AdsInter_CallShow", place, _placement);
#endif
            }
            else
            {
            }
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsInter_PassedLogic", place, _placement);
#endif
#endif
            return isshow;
        }

#if APPLOVIN_MAX
        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsInter_LoadDone");
#endif
        }

        private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            Debug.LogWarning($"OnInterstitial Failed {adUnitId} : {errorInfo.Message}");
            string networkName = errorInfo.MediatedNetworkErrorMessage.Replace(" ", "_");
        }

        private void InterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo arg3)
        {
            Debug.LogWarning("OnInterstitial Failed To Displayed");
            onInterAdsClosed?.Invoke();
            adManger.OnInterClosed();
            string networkName = errorInfo.MediatedNetworkErrorMessage.Replace(" ", "_");
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsInter_FailedToDisplay", place, "none");
#endif
        }

        private void OnInterstitialDisplayEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsInter_Displayed", place, "none");
#endif
        }

        private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is hidden. Pre-load the next ad
            Debug.Log("Applovin Inter Closed");
            onInterAdsClosed?.Invoke();
            adManger.OnInterClosed();

#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsInter_Closed", place, "none");
#endif
        }

        private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo arg2)
        {
        }
#endif

        #endregion Interstitial

        #region Banner

        public void InitializeBannerAds()
        {
            if (!adManger.adsConfig.should_show_banner) return;
#if APPLOVIN_MAX
            // Banners are automatically sized to 320x50 on phones and 728x90 on tablets
            // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments
            MaxSdk.CreateBanner(adsUnitIds[AdsType.Banner], GetBannerPos(adManger.adsConfig.pos_banner));

            string auto_size = adManger.adsConfig.auto_size_banner ? "true" : "false";
            MaxSdk.SetBannerExtraParameter(adsUnitIds[AdsType.Banner], "adaptive_banner", auto_size);
            // Set background or background color for banners to be fully functional
            MaxSdk.SetBannerBackgroundColor(adsUnitIds[AdsType.Banner], adManger.adsConfig.background_color_banner);

            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += (x, y) => OnAdRevenuePaidEvent(x, y, AdsType.Banner);
            //  TrackingManager.TrackEvent($"AdsBanner_CallLoad");
#endif
        }
#if APPLOVIN_MAX
        private MaxSdkBase.BannerPosition GetBannerPos(BannerPos pos)
        {
            MaxSdkBase.BannerPosition bannerPos = MaxSdkBase.BannerPosition.BottomCenter;
            switch (pos)
            {
                case BannerPos.TopCenter:
                    bannerPos = MaxSdkBase.BannerPosition.TopCenter;
                    break;
                case BannerPos.BottomCenter:
                    bannerPos = MaxSdkBase.BannerPosition.BottomCenter;
                    break;
                case BannerPos.TopLeft:
                    bannerPos = MaxSdkBase.BannerPosition.TopLeft;
                    break;
                case BannerPos.TopRight:
                    bannerPos = MaxSdkBase.BannerPosition.TopRight;
                    break;
                case BannerPos.Centered:
                    bannerPos = MaxSdkBase.BannerPosition.Centered;
                    break;
                case BannerPos.CenterLeft:
                    bannerPos = MaxSdkBase.BannerPosition.CenterLeft;
                    break;
                case BannerPos.CenterRight:
                    bannerPos = MaxSdkBase.BannerPosition.CenterRight;
                    break;
                case BannerPos.BottomLeft:
                    bannerPos = MaxSdkBase.BannerPosition.BottomLeft;
                    break;
                case BannerPos.BottomRight:
                    bannerPos = MaxSdkBase.BannerPosition.BottomRight;
                    break;
            }

            return bannerPos;
        }
#endif

        public void UpdatePositionBanner()
        {
            MaxSdk.UpdateBannerPosition(adsUnitIds[AdsType.Banner], GetBannerPos(adManger.adsConfig.pos_banner));
        }

        public void UpdateAdaptiveBanner()
        {
            string auto_size = adManger.adsConfig.auto_size_banner ? "true" : "false";
            MaxSdk.SetBannerExtraParameter(adsUnitIds[AdsType.Banner], "adaptive_banner", auto_size);
        }

        public void ShowBanner()
        {
#if APPLOVIN_MAX
            MaxSdk.ShowBanner(adsUnitIds[AdsType.Banner]);
#endif
        }

        public void HideBanner()
        {
#if APPLOVIN_MAX
            MaxSdk.HideBanner(adsUnitIds[AdsType.Banner]);
#endif
        }
#if APPLOVIN_MAX
        private void OnBannerLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
        {
#if FIREBASE_ANALYTIC
            Debug.Log("Load Banner Failed");
#endif
        }

        private void OnBannerLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            Debug.Log("Loaded Banner");
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent($"AdsBanner_LoadDone");
#endif
        }
#endif

        #endregion Banner

        #region Reward

        private void LoadRewardedAd()
        {
#if APPLOVIN_MAX
            MaxSdk.LoadRewardedAd(adsUnitIds[AdsType.Rewarded]);
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsReward_CallLoad");
#endif
#endif
        }

        public bool IsRewardAdsLoaded()
        {
#if APPLOVIN_MAX
            return MaxSdk.IsRewardedAdReady(adsUnitIds[AdsType.Rewarded]);
#endif
            return false;
        }

        public bool ShowRewardAds(Action _onRewardClose, string _placement)
        {
#if APPLOVIN_MAX
            Debug.Log("Show Reward");
            bool isshow = false;
            if (MaxSdk.IsRewardedAdReady(adsUnitIds[AdsType.Rewarded]))
            {
                IsRewarded = false;
                adManger.OnRewardedStarted();
                MaxSdk.ShowRewardedAd(adsUnitIds[AdsType.Rewarded], _placement);
                onRewardAdsClosed = _onRewardClose;
                isshow = true;
#if FIREBASE_ANALYTIC
                TrackingManager.TrackEvent("AdsReward_CallShow", place, _placement);
#endif
            }
            else
            {
            }
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsReward_PassedLogic", place, _placement);
#endif

#endif
            return isshow;
        }

#if APPLOVIN_MAX
        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Load done reward");
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsReward_LoadDone");
#endif
        }

        private void OnRewardedLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            Debug.LogWarning("OnRewardedAdFailedEvent");
            string networkName = errorInfo.MediatedNetworkErrorMessage.Replace(" ", "_");
        }

        private void OnRewardDisplayEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsReward_Displayed", place, "none");
#endif
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo arg3)
        {
            Debug.LogWarning("OnRewardedAd Failed To Display");

            // Rewarded ad failed to display. We recommend loading the next ad
            onRewardAdsClosed?.Invoke();
            adManger.OnRewardedClosed();
            string networkName = errorInfo.MediatedNetworkErrorMessage.Replace(" ", "_");
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsReward_FailedToDisplay", place, "none");
#endif
        }

        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo arg2)
        {
#if NB_APPFLYER
                        AppsFlyerSDK.AppsFlyer.sendEvent("event_video_reward_clicked", new Dictionary<string, string>() { { "clicked_video", "clicked_video" } });
#endif
        }

        private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is hidden. Pre-load the next ad
            Debug.Log("Applovin reward Closed");
            if (IsRewarded)
            {
                adManger.ResetAdsParameter();
                onRewardAdsClosed?.Invoke();
#if FIREBASE_ANALYTIC
                TrackingManager.TrackEvent("AdsReward_ReceivedReward", place, "none");
#endif
            }
            else
            {
#if FIREBASE_ANALYTIC
                TrackingManager.TrackEvent("AdsReward_Skip", place, "none");
#endif
            }

            adManger.OnRewardedClosed();
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsReward_Closed", place, "none");
#endif
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            IsRewarded = true;
        }
#endif

        #endregion

        #region App Open

        private void LoadAppOpenAd()
        {
#if APPLOVIN_MAX
            if (adManger.adsConfig.should_show_ads_open)
            {
                MaxSdk.LoadAppOpenAd(adsUnitIds[AdsType.AppOpen]);
#if FIREBASE_ANALYTIC
                TrackingManager.TrackEvent("AdsAOA_CallLoad");
#endif
            }
#endif
        }

        public bool IsAppOpenAdsLoaded()
        {
#if APPLOVIN_MAX
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsAOA_CallLoad");
#endif
            return MaxSdk.IsAppOpenAdReady(adsUnitIds[AdsType.AppOpen]);
#endif
        }
#if APPLOVIN_MAX
        public bool ShowAppOpenAds(Action _onAppOpenClose, string _placement = "none")
        {
            Debug.Log("Show App Open");
            if (!adManger.adsConfig.should_show_ads_open) return false;
            bool isshow = false;
            if (MaxSdk.IsAppOpenAdReady(adsUnitIds[AdsType.AppOpen]))
            {
                IsRewarded = false;
                MaxSdk.ShowAppOpenAd(adsUnitIds[AdsType.AppOpen], _placement);
                isshow = true;
#if FIREBASE_ANALYTIC
                TrackingManager.TrackEvent("AdsAOA_CallShow", place, "none");
#endif
            }
            else
            {
            }
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsAOA_PassedLogic", place, "none");
#endif
            return isshow;
        }

        private void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Applovin Open App Ads Closed");
            onAppOpenAdsClosed?.Invoke();
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsAOA_Closed", place, "none");
#endif
        }

        private void OnAppOpenLoadEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsAOA_LoadDone");
#endif
        }

        private void OnAppOpenLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
        {
        }

        private void OnAppOpenDisplayFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
        {
            string networkName = arg2.MediatedNetworkErrorMessage.Replace(" ", "_");
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsAOA_FailedToDisplay", place, "none");
#endif
        }

        private void OnAppOpenDisplayEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
#if FIREBASE_ANALYTIC
            TrackingManager.TrackEvent("AdsAOA_Displayed", place, "none");
#endif
        }
#endif

        #endregion
    }

    public enum BannerPos
    {
        TopCenter = 0,
        BottomCenter = 1,
        TopLeft = 2,
        TopRight = 3,
        Centered = 4,
        CenterLeft = 5,
        CenterRight = 6,
        BottomLeft = 7,
        BottomRight = 8
    }
}