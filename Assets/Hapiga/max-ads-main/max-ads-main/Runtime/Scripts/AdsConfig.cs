using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#endif

namespace Hapiga.Ads
{
    [Serializable]
    public class AdsNetworkConfig
    {
        public AdsModuleType adsModule;
        public string[] extras;
    }

    [Serializable]
    public class AdsConfig
    {
        public bool isTestAds; // có dùng test ads không

        public float check_load_ads_interval; // thời gian interval kiểm tra xem có ađs chưa, nếu chưa có thì sẽ load ads 
        public BannerPos pos_banner; // vị trí của banner
        public bool should_show_banner; // có show banner không
        public bool auto_size_banner;
        public Color background_color_banner;

        public bool should_show_ads_open; // có show App Open Ads không
        public float ads_open_interval;

        public bool should_show_inter; // có show interstital không
        public float inter_ads_interval_time; // thời gian interval hiển thị giữa 2 interstitial
        public float inter_after_reward_time; // thời gian hiện iterstitial sau khi hiện rewarded
        public bool is_show_inter_when_no_reward; // có dùng interstitial thay cho rewarded khi không load được reward hay không
    }
}