using UnityEngine;
using System;
using System.Collections.Generic;
using Hapiga.Core.Runtime.Singleton;

namespace Hapiga.Ads
{
    [Singleton("AdManger", true)]
#if ODIN_INSPECTOR
    public class AdManager : SerializedSingleton<AdManager>
#else
    public class AdManager : Singleton<AdManager>
#endif
    {
        [SerializeField] private bool isStartInit = true;
        [SerializeField] private AdjustEvents adjustEvents;

        [SerializeField] AdsNetworkConfig adsNetworkConfig;
        public AdsConfig adsConfig;

        [SerializeField] private Dictionary<AdsType, string> AndroidUnitId;
        [SerializeField] private Dictionary<AdsType, string> IOSUnitId;

        [SerializeField] private bool isFakeBanner;
        [SerializeField] private bool isFakeInter;
        [SerializeField] private bool isFakeOpenApp;
        [SerializeField] private bool isFakeReward;

        private IAdsModule _adsModule;
        private DateTime lastTimeShowInter;
        private float loadAdsTimer = 0;
        private float nextInterTime = 0;
        private DateTime lastTimePause;
        private bool isInterOrRewardShowing = true;

        private AdsRevPaidEvent adsRevPaidEvent = new AdsRevPaidEvent();

        public Action<MaxSdkBase.AdInfo> PaidEvent;

        public override void Init()
        {
            if (Application.isEditor)
            {
                isFakeBanner = true;
                isFakeInter = true;
                isFakeReward = true;
                isFakeOpenApp = true;
            }

            CreateAdModule();
            loadAdsTimer = adsConfig.check_load_ads_interval;
            lastTimeShowInter = DateTime.Now;
            SetNextInterTime(false);
        }

        public void ManualInitMMP()
        {
            adjustEvents?.ManualInitAdjust();
        }
        private void CreateAdModule()
        {
            if (!isStartInit) return;

            _adsModule = AdModule(adsNetworkConfig.adsModule);
            _adsModule.Init(this, AndroidUnitId, IOSUnitId, adsNetworkConfig.extras);

            IAdsModule AdModule(AdsModuleType adsModuleType)
            {
                IAdsModule adsModule = null;
                switch (adsModuleType)
                {
                    case AdsModuleType.ApplovinMax:
                        adsModule = new ApplovinModule();
                        break;
                }

                return adsModule;
            }
        }

        public void Update()
        {
            loadAdsTimer -= Time.unscaledDeltaTime;
            if (loadAdsTimer <= 0)
            {
                loadAdsTimer = adsConfig.check_load_ads_interval;
                _adsModule.CheckLoadAds();
            }
        }

        public void OnFake(bool fakeBanner, bool fakeInter, bool fakeOpenApp, bool fakeReward)
        {
            isFakeBanner = fakeBanner;
            isFakeInter = fakeInter;
            isFakeOpenApp = fakeOpenApp;
            isFakeReward = fakeReward;
            if (isFakeBanner)
            {
                HideBanner();
            }
        }

        public void ResetAdsParameter()
        {
            lastTimeShowInter = DateTime.Now;
            SetNextInterTime(true);
        }

        public void OnAdsRevPaid(MaxSdkBase.AdInfo info, string _unitId, AdsType _adType)
        {
            PaidEvent?.Invoke(info);
        }

        #region Banner

        public void ShowBanner()
        {
            if (isFakeBanner) return;
            if (adsConfig.should_show_banner)
            {
                _adsModule.ShowBanner();
            }
        }

        public void HideBanner()
        {
            if (isFakeBanner) return;
            _adsModule.HideBanner();
        }

        #endregion

        #region Interstitial

        public void OnInterStarted()
        {
            isInterOrRewardShowing = true;
        }

        public void OnInterClosed()
        {
            isInterOrRewardShowing = false;
            lastTimeShowInter = DateTime.Now;
            SetNextInterTime(false);
        }

        public void ShowInterstitialAds(string placement, bool ignoreInterval = false, Action closeCallback = null)
        {
            if (!adsConfig.should_show_inter || isFakeInter)
            {
                closeCallback?.Invoke();
                return;
            }

            if (ignoreInterval)
            {
                if (_adsModule.ShowInterAds(closeCallback, placement))
                {
                    lastTimeShowInter = DateTime.Now;
                }
                else
                {
                    closeCallback?.Invoke();
                }

                return;
            }

            if (DateTime.Now - lastTimeShowInter > TimeSpan.FromSeconds(nextInterTime))
            {
                if (_adsModule.ShowInterAds(closeCallback, placement))
                {
                    lastTimeShowInter = DateTime.Now;
                }
                else
                {
                    closeCallback?.Invoke();
                }
            }
            else
            {
                closeCallback?.Invoke();
            }
        }

        public bool CanInterstitalAdsShow()
        {
            bool isShowTime = DateTime.Now - lastTimeShowInter > TimeSpan.FromSeconds(nextInterTime);
            return isShowTime && _adsModule.IsInterstitalAdsLoaded();
        }

        public bool IsInterstitalAdsLoaded()
        {
            return _adsModule.IsInterstitalAdsLoaded();
        }

        private void SetNextInterTime(bool isAfterReward)
        {
            if (!isAfterReward)
            {
                nextInterTime = adsConfig.inter_ads_interval_time;
            }
            else
            {
                nextInterTime = adsConfig.inter_after_reward_time;
            }
        }

        #endregion

        #region Rewarded

        public void OnRewardedStarted()
        {
            isInterOrRewardShowing = true;
        }

        public void OnRewardedClosed()
        {
            isInterOrRewardShowing = false;
        }

        public bool ShowRewardedVideo(Action closeRewardCallback, string placement = null)
        {
            if (Application.isEditor || isFakeReward)
            {
                closeRewardCallback?.Invoke();
                return true;
            }

            if (_adsModule == null)
            {
                Debug.Log("<color=cyan>=>" + "applovinModule is null" + "</color>");
                return false;
            }

            if (_adsModule.ShowRewardAds(closeRewardCallback, placement))
            {
                return true;
            }
            else
            {
                if (CheckShowInterWhenNoReward(closeRewardCallback, placement))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsRewardAdsLoaded()
        {
            return _adsModule.IsRewardAdsLoaded();
        }

        private bool CheckShowInterWhenNoReward(Action closeRewardCallback, string placement)
        {
            if (!adsConfig.is_show_inter_when_no_reward)
            {
                return false;
            }

            if (_adsModule.IsInterstitalAdsLoaded())
            {
                _adsModule.ShowInterAds(closeRewardCallback, placement);
                return true;
            }

            return false;
        }

        public bool IsRewardedVideoLoaded()
        {
#if UNITY_EDITOR
            return true;
#endif
            return _adsModule.IsRewardAdsLoaded();
        }

        #endregion

        #region AppOpen

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                if (!isInterOrRewardShowing)
                {
                    if (ShowAppOpenAds())
                    {
                        lastTimePause = DateTime.Now;
                    }
                }
                else
                {
                    isInterOrRewardShowing = false;
                }
            }
        }

        public bool ShowAppOpenAds(string placement = "none", Action closeCallback = null)
        {
            if (isFakeOpenApp)
            {
                closeCallback?.Invoke();
                return true;
            }

            if (!adsConfig.should_show_ads_open)
            {
                return false;
            }

            if (DateTime.Now - lastTimePause < TimeSpan.FromSeconds(adsConfig.ads_open_interval))
            {
                return false;
            }

            if (_adsModule.ShowAppOpenAds(closeCallback, placement))
            {
                return true;
            }

            closeCallback?.Invoke();
            return false;
        }

        public bool IsAppOpenAdsLoaded()
        {
            return _adsModule.IsAppOpenAdsLoaded();
        }

        #endregion
    }

    public enum AdsType : byte
    {
        Banner,
        Interstitial,
        Rewarded,
        AppOpen,
        MREC,
    }

    public enum AdsModuleType : byte
    {
        ApplovinMax,
        YodoAds
    }
}