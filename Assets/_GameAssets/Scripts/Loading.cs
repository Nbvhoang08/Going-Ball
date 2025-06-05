using GameAnalyticsSDK;
using Hapiga.Ads;
using Hapiga.RemoteConfig;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public float timeCheck = 5;
    private float timer;

    private IEnumerator Start()
    {
        //AdjustQualityForDevice();
        GameAnalytics.Initialize();
        timer = 0;
        yield return new WaitUntil(() => RemoteConfigManager.Instance.isFetchSuccess || timer > timeCheck);
        timer = 0;

        yield return new WaitUntil(() => AdManager.Instance.IsAppOpenAdsLoaded() || timer > timeCheck);
        SceneManager.LoadScene("Main");
    }
    void Update()
    {
        timer += Time.deltaTime;
    }
    void AdjustQualityForDevice()
    {
        int ramSize = SystemInfo.systemMemorySize; // RAM in MB
        int maxTextureSize = SystemInfo.maxTextureSize;

        if (ramSize <= 2048 || maxTextureSize <= 2048)
        {
            QualitySettings.SetQualityLevel(0); // Low quality
            Debug.Log("Low-end device detected. Quality set to Low.");
        }
        else if (ramSize <= 3072 || maxTextureSize <= 4096)
        {
            QualitySettings.SetQualityLevel(1); // Medium quality
            Debug.Log("Mid-range device detected. Quality set to Medium.");
        }
        else
        {
            QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1); // High quality
            Debug.Log("High-end device detected. Quality set to High.");
        }
    }
}
