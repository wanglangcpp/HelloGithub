namespace Genesis.GameClient
{
    public static class AssetUtility
    {
        public static string GetDataTableAsset(string assetName)
        {
            return string.Format("Assets/Main/DataTables/{0}.txt", assetName);
        }

        public static string GetDictionaryAsset(string assetName)
        {
            return string.Format("Assets/Main/Localization/{0}/Dictionaries/{1}.txt", GameEntry.Localization.Language.ToString(), assetName);
        }

        public static string GetSceneAsset(string assetName)
        {
            return string.Format("Assets/Main/Scenes/{0}.unity", assetName);
        }

        public static string GetMusicAsset(string assetName)
        {
            return string.Format("Assets/Main/Music/{0}.mp3", assetName);
        }

        public static string GetSoundAsset(string assetName)
        {
            return string.Format("Assets/Main/Sounds/{0}.wav", assetName);
        }

        public static string GetUIFormAsset(string assetName)
        {
            return string.Format("Assets/Main/Prefabs/UI/{0}.prefab", assetName);
        }

        public static string GetCharacterAsset(string assetName)
        {
            return string.Format("Assets/Main/Prefabs/Characters/{0}.prefab", assetName);
        }

        public static string GetWeaponAsset(string assetName)
        {
            return string.Format("Assets/Main/Prefabs/Characters/{0}.prefab", assetName);
        }

        public static string GetBuildingAsset(string assetName)
        {
            return string.Format("Assets/Main/Prefabs/Buildings/{0}.prefab", assetName);
        }

        public static string GetEffectAsset(string assetName)
        {
            return string.Format("Assets/Main/Prefabs/Effects/{0}.prefab", assetName);
        }

        public static string GetBulletAsset(string assetName)
        {
            return string.Format("Assets/Main/Prefabs/ShedObjects/Bullets/{0}.prefab", assetName);
        }

        public static string GetPortalAsset(string assetName)
        {
            return string.Format("Assets/Main/Prefabs/ShedObjects/Portals/{0}.prefab", assetName);
        }

        public static string GetAirWallAsset(string assetName)
        {
            return string.Format("Assets/Main/Prefabs/ShedObjects/AirWalls/{0}.prefab", assetName);
        }

        public static string GetChestAsset(string assetName = null)
        {
            return string.Format("Assets/Main/Prefabs/ShedObjects/Chests/{0}.prefab", assetName ?? "DefaultChest");
        }

        public static string GetBulletRebounderAsset(string assetName)
        {
            return string.Format("Assets/Main/Prefabs/ShedObjects/BulletRebounders/{0}.prefab", assetName);
        }

        public static string GetBehaviorAsset(string assetName)
        {
            return string.Format("Assets/Main/Behaviors/{0}.asset", assetName);
        }

        public static string GetTimeLineAsset(string assetName)
        {
            return string.Format("Assets/Main/TimeLines/{0}.txt", assetName);
        }

        public static string GetAtlasAsset(string assetName)
        {
            return string.Format("Assets/Main/UI/Atlases/{0}.prefab", assetName);
        }

        public static string GetTextureAsset(string assetName)
        {
            return string.Format("Assets/Main/UI/UITextures/{0}.png", assetName);
        }

        public static string GetLuaAsset(string scriptName, LuaScriptCategory category)
        {
            switch (category)
            {
                case LuaScriptCategory.Base:
                    return string.Format("Assets/ToLua/ToLua/Lua/{0}.lua", scriptName);
                case LuaScriptCategory.Custom:
                default:
                    return string.Format("Assets/Main/LuaScripts/{0}.lua", scriptName);
            }
        }

        public static string GetCameraAnimationAsset(string animName)
        {
            return string.Format("Assets/Main/CameraAnimations/{0}.anim", animName);
        }
    }
}
