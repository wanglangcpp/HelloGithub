using UnityEngine;

namespace Genesis.GameClient
{
    public static class PacketUtility
    {
        public static bool FillLogReport(CLLogReport packet)
        {
            if (!GameEntry.IsAvailable)
            {
                return false;
            }

            string publisher;
            if (GameEntry.Data.HasTempData(Constant.TempData.Publisher))
            {
                publisher = GameEntry.Data.GetTempData<string>(Constant.TempData.Publisher);
            }
            else
            {
                publisher = string.Empty;
            }

            packet.Params.Add(new PBKeyValuePair() { Key = "Publisher", Value = publisher });
            packet.Params.Add(new PBKeyValuePair() { Key = "DeviceId", Value = SystemInfo.deviceUniqueIdentifier });
            packet.Params.Add(new PBKeyValuePair() { Key = "DeviceName", Value = SystemInfo.deviceName });
            packet.Params.Add(new PBKeyValuePair() { Key = "DeviceModel", Value = SystemInfo.deviceModel });
            packet.Params.Add(new PBKeyValuePair() { Key = "ProcessorType", Value = SystemInfo.processorType });
            packet.Params.Add(new PBKeyValuePair() { Key = "ProcessorCount", Value = SystemInfo.processorCount.ToString() });
            packet.Params.Add(new PBKeyValuePair() { Key = "MemorySize", Value = SystemInfo.systemMemorySize.ToString() });
            packet.Params.Add(new PBKeyValuePair() { Key = "OperatingSystem", Value = SystemInfo.operatingSystem });
            packet.Params.Add(new PBKeyValuePair() { Key = "GameVersion", Value = GameEntry.Base.GameVersion });
            packet.Params.Add(new PBKeyValuePair() { Key = "Platform", Value = Application.platform.ToString() });
            packet.Params.Add(new PBKeyValuePair() { Key = "Language", Value = GameEntry.Localization.Language.ToString() });
            packet.Params.Add(new PBKeyValuePair() { Key = "UnityVersion", Value = Application.unityVersion });
            packet.Params.Add(new PBKeyValuePair() { Key = "InstallMode", Value = Application.installMode.ToString() });
            packet.Params.Add(new PBKeyValuePair() { Key = "SandboxType", Value = Application.sandboxType.ToString() });
            packet.Params.Add(new PBKeyValuePair() { Key = "ScreenWidth", Value = Screen.width.ToString() });
            packet.Params.Add(new PBKeyValuePair() { Key = "ScreenHeight", Value = Screen.height.ToString() });
            packet.Params.Add(new PBKeyValuePair() { Key = "ScreenDPI", Value = Screen.dpi.ToString() });
            packet.Params.Add(new PBKeyValuePair() { Key = "ScreenOrientation", Value = Screen.orientation.ToString() });
            packet.Params.Add(new PBKeyValuePair() { Key = "ScreenResolution", Value = string.Format("{0} x {1} @ {2}Hz", Screen.currentResolution.width.ToString(), Screen.currentResolution.height.ToString(), Screen.currentResolution.refreshRate.ToString()) });
            packet.Params.Add(new PBKeyValuePair() { Key = "UseWifi", Value = (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork).ToString() });

#if UNITY_IOS && !UNITY_EDITOR
            packet.Params.Add(new PBKeyValuePair() { Key = "IOSGeneration", Value = UnityEngine.iOS.Device.generation.ToString() });
            packet.Params.Add(new PBKeyValuePair() { Key = "IOSSystemVersion", Value = UnityEngine.iOS.Device.systemVersion });
            packet.Params.Add(new PBKeyValuePair() { Key = "IOSVendorIdentifier", Value = UnityEngine.iOS.Device.vendorIdentifier ?? string.Empty });
            packet.Params.Add(new PBKeyValuePair() { Key = "IOSAdvertisingIdentifier", Value = UnityEngine.iOS.Device.advertisingIdentifier ?? string.Empty });
            packet.Params.Add(new PBKeyValuePair() { Key = "IOSAdvertisingTracking", Value = UnityEngine.iOS.Device.advertisingTrackingEnabled.ToString() });
#endif

            return true;
        }
    }
}
