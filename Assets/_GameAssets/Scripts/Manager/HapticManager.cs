using UnityEngine;
using System.Runtime.InteropServices;

public enum HapticType
{
    Light,
    Medium,
    Heavy,
    Success,
    Warning,
    Failure
}

public static class HapticManager
{
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void _PlayLightHaptic();
    [DllImport("__Internal")]
    private static extern void _PlayMediumHaptic();
    [DllImport("__Internal")]
    private static extern void _PlayHeavyHaptic();
    [DllImport("__Internal")]
    private static extern void _PlaySuccessHaptic();
    [DllImport("__Internal")]
    private static extern void _PlayWarningHaptic();
    [DllImport("__Internal")]
    private static extern void _PlayFailureHaptic();
#endif

    public static void PlayHaptic(HapticType type)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaObject vibrator = activity.Call<AndroidJavaObject>("getSystemService", "vibrator");

            long duration = GetVibrationDuration(type);
            if (vibrator != null)
            {
                vibrator.Call("vibrate", duration);
            }
        }
#elif UNITY_IOS && !UNITY_EDITOR
        switch (type)
        {
            case HapticType.Light: _PlayLightHaptic(); break;
            case HapticType.Medium: _PlayMediumHaptic(); break;
            case HapticType.Heavy: _PlayHeavyHaptic(); break;
            case HapticType.Success: _PlaySuccessHaptic(); break;
            case HapticType.Warning: _PlayWarningHaptic(); break;
            case HapticType.Failure: _PlayFailureHaptic(); break;
        }
#else
        // Dành cho Editor
        Debug.Log($"[HapticManager] PlayHaptic: {type}");
#endif
    }

    private static long GetVibrationDuration(HapticType type)
    {
        switch (type)
        {
            case HapticType.Light: return 20;
            case HapticType.Medium: return 40;
            case HapticType.Heavy: return 150;
            case HapticType.Success: return 120;
            case HapticType.Warning: return 100;
            case HapticType.Failure: return 150;
            default: return 50;
        }
    }
}
