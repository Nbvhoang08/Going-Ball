# Big Bear Remote Config

Package dùng để lấy remote config runtime</br>
Hiện đang hỗ trợ 
- Firebase Remote Config

## Hướng dẫn sử dụng

### cài đặt package

1. Trong Unity mở <b>Package Manager</b>
2. Chọn <b>Add Package from git URL...</b>
3. Điền url = https://gitlab.com/big-bear-team/packages/package-remote-config.git

### Setup

1. import các plugin Remote Config vào (Firebase Remote Config).
2. Load các module ads bằng lệnh trên Menu : **Big Bear -> Reload Remote Config Module**
3. Kiểm tra xem trong Scripting Define Symbols xem có các plugin Ads chưa, nếu có rồi thì là thành công

- Firebase Remote : **BB_FIREBASE_REMOTE**

4. Thêm prefab **RemoteConfigManager** vào trong scene đầu tiên, chuột phải vào Window **Hierarchy**, chọn option **Big Bear** -> **RemoteConfigManager** <br>

![RemoteConfigManager](Documentation~/images/add prefab.png)

5. Chỉnh sửa các thông số remote config qua cửa **Remote Config Window**, mở qua menu **Big Bear -> Remote Config**
   
![RemoteConfigManager](Documentation~/images/remoteconfig window.png)

### Các package cần thiết
Trong project cần có các package sau để có thể dùng đc package Remote Config:
1. <b>Sirenix Odin package</b>: sử dụng odin để dựng các window
2. <b>Big Bear Core</b> [(link)](https://gitlab.com/big-bear-team/packages/package-core.git) 

### Các tính năng

#### I. Lấy thông số remote config qua các hàm
   hiện tại hỗ trợ các loại dữ liệu `int`,`bool`,`string`
```csharp
    bool boolValue = RemoteConfigManager.Instance.GetBool(string remoteParam);
    int intValue = RemoteConfigManager.Instance.GetInt(string remoteParam);
    string stringValue = RemoteConfigManager.Instance.GetString(string remoteParam);
```

#### II. Best Practice
1. Nên để prefab **RemoteConfigManager** ở scene đầu tiên, đợi sau khi đã lấy xong remote config thì mới vào tiếp game
2. Code demo scene đầu tiên
```csharp
public class FirstScene : MonoBehaviour
{
    void Start()
    {
        // nếu đã fetch xong remote config thì tiếp tục luôn
        if (RemoteConfigManager.Instance.FetchStatus == RemoteFetchStatus.FetchFinish)
        {
            GetRemoteConfig();
        }
        else
        {
            // nếu chưa fetch xong thì sẽ lắng nghe sự kiện RemoteConfigFetched
            EventManager.Instance.AddListener<RemoteConfigFetched>(OnRemoteConfigFetched);                        
        }
    }

    private void OnRemoteConfigFetched(RemoteConfigFetched e)
    {
        EventManager.Instance.RemoveListener<RemoteConfigFetched>(OnRemoteConfigFetched);
        GetRemoteConfig();
    }

    void GetRemoteConfig()
    {
        bool boolValue = RemoteConfigManager.Instance.GetBool("bool_config_key");
        int intValue = RemoteConfigManager.Instance.GetInt("int_config_key");
        string stringValue = RemoteConfigManager.Instance.GetString("string_config_key");
        
        //
        SceneManager.LoadScene("Gameplay");
    }
}
```