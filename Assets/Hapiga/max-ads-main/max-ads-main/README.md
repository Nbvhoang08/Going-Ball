# Hapiga Ads

Package để hiển thị ads <br>
Mediation hỗ trợ

- Applovin

Các loại Ads hỗ trợ

- Banner
- Interstitial
- Rewarded
- AppOpen

## Hướng dẫn sử dụng

### cài đặt package

1. Trong Unity mở <b>Package Manager</b>
2. Chọn <b>Add Package from git URL...</b>
3. Điền url = https://gitlab.com/hapiga-core/max-ads.git
### Setup

1. import các plugin Ads vào (Applovin Max).
2. Load các module ads bằng lệnh trên Menu : **Hapiga Package-> APPLOVIN_MAX**
3. Kiểm tra xem trong Scripting Define Symbols xem có các plugin Ads chưa, nếu có rồi thì là thành công
- Applovin : **APPLOVIN_MAX**

4. Thêm prefab **AdManager** vào trong scene, chuột phải vào Window **Hierarchy**, chọn option **Hapiga Package-> **AdManager** <br>

![adMangerr](Documentation~/images/add prefab.png)

### Điều kiện

#### Các package cần thiết

1. **Sirenix Odin**

### Các tính năng

#### I. Các thông số Ads Config

1. Chỉnh sửa Ads Config ngay trong **AdManager**

2. Set ads config lúc runtime, bằng lệnh
```csharp
    private bool isStartInit = true;// AdManager sẽ tự động được init ở start

    public bool isTestAds; // có dùng test ads không
    public float check_load_ads_interval; // thời gian interval kiểm tra xem có ađs chưa, nếu chưa có thì sẽ load ads 
```
3. Các Event của Ads

   **AdManager** sẽ bắn ra các Event: PaidEvent

#### II. Banner


```csharp
	Banner sẽ được tự động show khi gọi AdManager.Init.

   public BannerPos pos_banner; // vị trí của banner
   public bool should_show_banner; // có show banner không
   public bool auto_size_banner; // size banner tự động
   public Color background_color_banner;// màu background color


    AdManager.Instance.ShowBanner(BannerPos bannerPos;
    AdManager.Instance.HideBanner(BannerPos bannerPos;
```

#### III. Interstitial


 ```csharp
	     public bool should_show_inter; // có show interstital không
        public float inter_ads_interval_time; // thời gian interval hiển thị giữa 2 interstitial
        public float inter_after_reward_time; // thời gian hiện iterstitial sau khi hiện rewarded
        public bool is_show_inter_when_no_reward; // có dùng interstitial thay cho rewarded khi không load được reward hay không

    	AdManager.Instance.ShowInterstitialAds(Action closeCallback = null);
```

#### IV. Rewarded

 ```csharp
    AdManager.Instance.ShowRewardedVideo(Action closeRewardCallback, Action skipRewardCallback, string placement = null);
```

#### V. AppOpen

**AdManager** sẽ tự động check show AppOpen Ads trong sự kiện `void OnApplicationPause(bool pauseStatus)`<br>
Lúc đầu game vào nếu muốn hiện AppOpen thì phải tự gọi thông qua hàm bên dưới

1. Set có show AppOpen hay không

```csharp
    AdManager.Instance.SetShouldShowAppOpen(bool shouldShow);
```

2. Show AppOpen

 ```csharp
    AdManager.Instance.ShowAppOpenAds(Action closeCallback = null);
```
3. 