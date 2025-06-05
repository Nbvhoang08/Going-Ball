using System;

namespace Hapiga.RemoteConfig
{
    public interface IRemoteConfig
    {
        void Init();
        int GetIntValue(string key);
        string GetStringValue(string key);
        bool GetBoolValue(string key);
    }
}