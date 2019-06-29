using GameFramework;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    public static class LocalizationExtension
    {
        public static void LoadDictionary(this LocalizationComponent localizationComponent, string dictionaryName)
        {
            if (string.IsNullOrEmpty(dictionaryName))
            {
                Log.Warning("Dictionary name is invalid.");
                return;
            }

            GameEntry.Localization.LoadDictionary(dictionaryName, AssetUtility.GetDictionaryAsset(dictionaryName), null);
        }
    }
}
