# Hướng dẫn cài được

1. import các plugin Firebase vào project
2. Load các module ads bằng lệnh trên Menu : **Hapiga Package-> FIREBASE_ANALYTIC**
3. Kiểm tra xem trong Scripting Define Symbols thành công chưa

4. Thêm prefab **AdManager** vào trong scene, chuột phải vào Window **Hierarchy**, chọn option **Hapiga Package-> **TrackingManager** <br>
```csharp
  	void TrackScreen(string screen);
        void TrackEvent(string _eventName);
        void TrackEvent(string _eventName, string _paramName, string _paramValue);
        void TrackEvent(string _eventName, string _paramName, int _paramValue);

        void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            string _paramValue2);

        void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            int _paramValue2);

        void TrackEvent(string _eventName, string _paramName1, int _paramValue1, string _paramName2,
            int _paramValue2);
        void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            float _paramValue2);
        void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            string _paramValue2, string _paramName3, string _paramValue3);

        void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            string _paramValue2, string _paramName3, int _paramValue3);
        void TrackEvent(string _eventName, params object[] parameterList);

        void TrackUserProperty(string _propertyName, string _propertyValue);// tracki user property
```