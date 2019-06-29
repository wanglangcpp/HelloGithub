using GameFramework;
using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    public static class EntityExtension
    {
        private static int s_SerialId = -1;
        private static IList<UnityGameFramework.Runtime.Entity> s_LockedEntities = new List<UnityGameFramework.Runtime.Entity>();

        public static void Reset()
        {
            s_SerialId = -1;
            s_LockedEntities.Clear();
        }

        public static void ShowLobbyMeHero(this EntityComponent entityComponent, HeroData data)
        {
            entityComponent.ShowCharacter(typeof(MeHeroCharacter), "Hero", true, data);
        }

        public static void ShowLobbyHero(this EntityComponent entityComponent, HeroData data)
        {
            entityComponent.ShowCharacter(typeof(HeroCharacter), "Hero", true, data);
        }

        public static void ShowMeHero(this EntityComponent entityComponent, HeroData data)
        {
            entityComponent.ShowCharacter(typeof(MeHeroCharacter), "Hero", false, data);
        }

        public static void ShowHero(this EntityComponent entityComponent, HeroData data)
        {
            entityComponent.ShowCharacter(typeof(HeroCharacter), "Hero", false, data);
        }

        public static void ShowPureAIHero(this EntityComponent entityComponent, HeroData data)
        {
            entityComponent.ShowCharacter(typeof(OppHeroCharacter), "Hero", false, data);
        }

        public static void ShowNpc(this EntityComponent entityComponent, NpcCharacterData data)
        {
            entityComponent.ShowCharacter(typeof(NpcCharacter), "Character", false, data);
        }

        public static void ShowLobbyNpc(this EntityComponent entityComponent, LobbyNpcData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DRLobbyNpc> dtLobbyNpc = GameEntry.DataTable.GetDataTable<DRLobbyNpc>();
            DRLobbyNpc dataRow = dtLobbyNpc.GetDataRow(data.LobbyNpcId);
            if (dataRow == null)
            {
                Log.Warning("Can not load DRLobbyNpc '{0}' from data table.", data.Id.ToString());
                return;
            }
            entityComponent.ShowEntity(data.Id, typeof(LobbyNpc), AssetUtility.GetCharacterAsset(dataRow.ResourceName), "LobbyNpc", data);
        }

        public static void ShowBullet(this EntityComponent entityComponent, BulletData data)
        {
            IDataTable<DRBullet> dtBullet = GameEntry.DataTable.GetDataTable<DRBullet>();
            DRBullet dataRow = dtBullet.GetDataRow(data.BulletId);
            if (dataRow == null)
            {
                Log.Warning("Can not load bullet '{0}' from data table.", data.BulletId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, typeof(Bullet), AssetUtility.GetBulletAsset(dataRow.ResourceName), "Bullet", data);
        }

        public static void ShowAirWall(this EntityComponent entityComponent, AirWallData data)
        {
            IDataTable<DRAirWall> dtAirWall = GameEntry.DataTable.GetDataTable<DRAirWall>();
            DRAirWall dataRow = dtAirWall.GetDataRow(data.AirWallId);
            if (dataRow == null)
            {
                Log.Warning("Can not load air wall '{0}' from data table.", data.AirWallId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, typeof(AirWall), AssetUtility.GetAirWallAsset(dataRow.ResourceName), "SceneShedObject", data);
        }

        public static void ShowEffect(this EntityComponent entityComponent, EffectData data)
        {
            entityComponent.ShowEntity(data.Id, typeof(Effect), AssetUtility.GetEffectAsset(data.ResourceName), "Effect", data);
        }

        public static void ShowBuilding(this EntityComponent entityComponent, BuildingData data)
        {
            var dataTable = GameEntry.DataTable.GetDataTable<DRBuildingModel>();


            DRBuildingModel dataRow = dataTable.GetDataRow(data.BuildingModelId);
            if (dataRow == null)
            {
                Log.Error("Building '{0}' not found.", data.BuildingModelId);
            }

            entityComponent.ShowEntity(data.Id, typeof(Building), AssetUtility.GetBuildingAsset(dataRow.ResourceName), "Building", data);
        }

        public static void ShowChest(this EntityComponent entityComponent, ChestData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            entityComponent.ShowEntity(data.Id, typeof(Chest), AssetUtility.GetChestAsset(), "SceneShedObject", data);
        }

        public static void ShowFakeCharacter(this EntityComponent entityComponent, FakeCharacterData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DRCharacter> dtCharacter = GameEntry.DataTable.GetDataTable<DRCharacter>();
            DRCharacter dataRow = dtCharacter.GetDataRow(data.CharacterId);
            if (dataRow == null)
            {
                Log.Warning("Can not load character '{0}' from data table.", data.CharacterId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, typeof(FakeCharacter), AssetUtility.GetCharacterAsset(dataRow.ResourceNameForShow), "Fake", data);
        }

        public static void ShowGuideIndicator(this EntityComponent entityComponent, int ownerId)
        {
            var data = new EffectData(GetSerialId(entityComponent), string.Empty, "Characters/Players/effects_hero_guide_indicator", ownerId);

            entityComponent.ShowEntity(data.Id, typeof(GuideIndicatorEffect), AssetUtility.GetEffectAsset(data.ResourceName), "Effect", data);
        }

        public static void ShowDebuggerCharacter(this EntityComponent entityComponent, DebuggerCharacterData data)
        {
            if (!GameEntry.Base.EditorResourceMode)
            {
                return;
            }

            entityComponent.ShowEntity(data.Id, typeof(DebuggerCharacter), AssetUtility.GetCharacterAsset(data.ResourceName), "Character", data);
        }

        public static void ShowBulletRebounder(this EntityComponent entityComponent, BulletRebounderData data)
        {
            IDataTable<DRBulletRebounder> dt = GameEntry.DataTable.GetDataTable<DRBulletRebounder>();
            DRBulletRebounder dataRow = dt.GetDataRow(data.BulletRebounderId);
            if (dataRow == null)
            {
                Log.Warning("Can not load bullet rebounder '{0}' from data table.", data.BulletRebounderId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, typeof(BulletRebounder), AssetUtility.GetBulletRebounderAsset(dataRow.ResourceName), "SceneShedObject", data);
        }

        public static void ShowPortal(this EntityComponent entityComponent, PortalData data)
        {
            IDataTable<DRPortal> dtPortal = GameEntry.DataTable.GetDataTable<DRPortal>();
            DRPortal dataRow = dtPortal.GetDataRow(data.PortalId);
            if (dataRow == null)
            {
                Log.Warning("Can not load portal '{0}' from data table.", data.PortalId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, typeof(Portal), AssetUtility.GetPortalAsset(dataRow.ResourceName), "SceneShedObject", data);
        }

        public static void ShowWeapon(this EntityComponent entityComponent, WeaponData data)
        {
            IDataTable<DRWeapon> DRWeapon = GameEntry.DataTable.GetDataTable<DRWeapon>();
            DRWeapon dataRow = DRWeapon.GetDataRow(data.WeaponId);
            if (dataRow == null)
            {
                Log.Warning("Can not load weapon '{0}' from data table.", data.WeaponId.ToString());
                return;
            }

            string resourceName = dataRow.ResourceName;
            switch (data.ShowType)
            {
                case WeaponData.WeaponShowType.Lobby:
                    resourceName = dataRow.ResourceNameForLobby;
                    break;
                case WeaponData.WeaponShowType.ForShow:
                    resourceName = dataRow.ResourceNameForShow;
                    break;
            }
            entityComponent.ShowEntity(data.Id, typeof(Weapon), AssetUtility.GetWeaponAsset(resourceName), "Weapon", data);
        }

        public static void HideEntity(this EntityComponent entityComponent, Entity entity)
        {
            entityComponent.HideEntity(entity.Entity);
        }

        public static Entity GetGameEntity(this EntityComponent entityComponent, int entityId)
        {
            UnityGameFramework.Runtime.Entity entity = entityComponent.GetEntity(entityId);
            if (entity == null)
            {
                return null;
            }

            return entity.Logic as Entity;
        }

        public static void AddLock(this EntityComponent entityComponent, Entity entity)
        {
            if (s_LockedEntities.Contains(entity.Entity))
            {
                return;
            }

            s_LockedEntities.Add(entity.Entity);
            entityComponent.SetInstanceLocked(entity.Entity, true);
        }

        public static void RemoveAllLocks(this EntityComponent entityComponent)
        {
            for (int i = 0; i < s_LockedEntities.Count; i++)
            {
                entityComponent.SetInstanceLocked(s_LockedEntities[i], false);
            }

            s_LockedEntities.Clear();
        }

        public static int GetSerialId(this EntityComponent entityComponent)
        {
            return s_SerialId--;
        }

        private static void ShowCharacter(this EntityComponent entityComponent, Type characterLogicType, string entityGroup, bool isLobby, CharacterData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DRCharacter> dtCharacter = GameEntry.DataTable.GetDataTable<DRCharacter>();
            DRCharacter dataRow = dtCharacter.GetDataRow(data.CharacterId);
            if (dataRow == null)
            {
                Log.Warning("Can not load character '{0}' from data table.", data.CharacterId.ToString());
                return;
            }

            var resourceName = isLobby ? dataRow.ResourceNameForLobby : dataRow.ResourceName;
            entityComponent.ShowEntity(data.Id, characterLogicType, AssetUtility.GetCharacterAsset(resourceName), entityGroup, data);
        }
    }
}
