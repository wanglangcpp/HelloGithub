using UnityEngine;
using GameFramework.Fsm;
using System.Collections.Generic;
using GameFramework.Event;
using GameFramework;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 战斗界面中使用的小地图。
    /// </summary>
    public class MiniMap : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_UnitTemplate = null;

        [SerializeField]
        private UITexture m_MapTexture = null;

        [SerializeField]
        private string m_MeSpriteName = "icon_mine";

        [SerializeField]
        private Vector2 m_MeSpriteSize = new Vector2(22, 22);

        [SerializeField]
        private int m_MeDisplayDepth = 10;

        [SerializeField]
        private Color m_DeadUnitColorTint = new Color(127, 127, 127);

        private IDictionary<int, UISprite> m_UnitsInUse = new Dictionary<int, UISprite>();

        private IDictionary<int, UISprite> m_UnitsNpcs = new Dictionary<int, UISprite>();

        private IFsm<MiniMap> m_Fsm = null;

        private MimicMeleeInstanceLogic m_InstanceLogic = null;

        #region MonoBehaviour

        private void Awake()
        {
            m_Fsm = GameEntry.Fsm.CreateFsm(GetType().ToString() + GetHashCode().ToString(), this,
                new StateInit(),
                new StateNormal());
        }

        private void Start()
        {
            m_Fsm.Start<StateInit>();
        }

        private void OnDestroy()
        {
            GameEntry.Fsm.DestroyFsm(m_Fsm);
            m_Fsm = null;
        }

        #endregion MonoBehaviour

        private void OnEntityShow(int entityId)
        {
            var entity = GameEntry.Entity.GetGameEntity(entityId);
            DestroyUnitForEntityIdIfNeeded(entityId);

            var myHeroCharacter = entity as MeHeroCharacter;
            if (myHeroCharacter != null)
            {
                OnMyHeroCharacterShow(entityId, entity, myHeroCharacter);
                return;
            }

            var npc = entity as NpcCharacter;
            if (npc != null)
            {
                OnNpcShow(entityId, entity, npc);
                return;
            }
        }

        private void OnNpcShow(int entityId, Entity entity, NpcCharacter npc)
        {
            var npcId = npc.Data.NpcId;

            var dr = GameEntry.DataTable.GetDataTable<DRNpcInMimicMelee>().GetDataRow(npcId);
            if (dr == null || string.IsNullOrEmpty(dr.SpriteNameOnMiniMap))
            {
                return;
            }

            var go = NGUITools.AddChild(gameObject, m_UnitTemplate);
            go.SetActive(true);
            var sprite = go.GetComponent<UISprite>();
            sprite.color = npc.Data.IsFakeHero && npc.IsDead ? m_DeadUnitColorTint : Color.white;
            sprite.spriteName = dr.SpriteNameOnMiniMap;
            sprite.width = Mathf.RoundToInt(dr.SpriteSizeOnMiniMap.x);
            sprite.height = Mathf.RoundToInt(dr.SpriteSizeOnMiniMap.y);
            sprite.cachedTransform.SetLocalPositionX(m_InstanceLogic.MiniMapScale * entity.CachedTransform.position.x + m_InstanceLogic.MiniMapOffset.x);
            sprite.cachedTransform.SetLocalPositionY(m_InstanceLogic.MiniMapScale * entity.CachedTransform.position.z + m_InstanceLogic.MiniMapOffset.y);
            sprite.depth = m_MapTexture.depth + dr.DisplayDepthOnMiniMap;
            if (npcId == Constant.MimicMeleeMiniMapOneNpcId || npcId == Constant.MimicMeleeMiniMapTwoNpcId)
            {
                m_UnitsInUse.Add(entityId, sprite);
            }
            else
            {
                m_UnitsNpcs.Add(entityId, sprite);
            }
        }

        private void OnMyHeroCharacterShow(int entityId, Entity entity, MeHeroCharacter myHeroCharacter)
        {
            var go = NGUITools.AddChild(gameObject, m_UnitTemplate);
            go.SetActive(true);
            var sprite = go.GetComponent<UISprite>();
            sprite.color = myHeroCharacter.IsDead ? m_DeadUnitColorTint : Color.white;
            sprite.spriteName = m_MeSpriteName;
            sprite.width = Mathf.RoundToInt(m_MeSpriteSize.x);
            sprite.height = Mathf.RoundToInt(m_MeSpriteSize.y);
            sprite.cachedTransform.SetLocalPositionX(m_InstanceLogic.MiniMapScale * entity.CachedTransform.position.x + m_InstanceLogic.MiniMapOffset.x);
            sprite.cachedTransform.SetLocalPositionY(m_InstanceLogic.MiniMapScale * entity.CachedTransform.position.z + m_InstanceLogic.MiniMapOffset.y);
            sprite.depth = m_MapTexture.depth + m_MeDisplayDepth;
            m_UnitsInUse.Add(entityId, sprite);
        }

        private void DestroyUnitForEntityIdIfNeeded(int entityId)
        {
            UISprite currentUnit;
            if (m_UnitsInUse.TryGetValue(entityId, out currentUnit))
            {
                m_UnitsInUse.Remove(entityId);
                Destroy(currentUnit.gameObject);
            }

            UISprite currentUnitNpc;
            if (m_UnitsNpcs.TryGetValue(entityId, out currentUnitNpc))
            {
                m_UnitsInUse.Remove(entityId);
                Destroy(currentUnitNpc.gameObject);
            }
        }

        private void OnEntityHide(int entityId)
        {
            UISprite currentNpc;
            if (m_UnitsNpcs.TryGetValue(entityId, out currentNpc))
            {
                m_UnitsNpcs.Remove(entityId);
                Destroy(currentNpc.gameObject);
            }

            UISprite currentUnit;
            if (m_UnitsInUse.TryGetValue(entityId, out currentUnit))
            {
                m_UnitsInUse.Remove(entityId);
                Destroy(currentUnit.gameObject);
            }
        }

        private void OnEntityDead(int entityId)
        {
            var entity = GameEntry.Entity.GetGameEntity(entityId);
            var npcCharacter = entity as NpcCharacter;
            var myHeroCharacter = entity as MeHeroCharacter;

            if (npcCharacter != null && npcCharacter.Data.IsFakeHero || myHeroCharacter != null)
            {
                UISprite currentUnit;
                if (m_UnitsInUse.TryGetValue(entityId, out currentUnit))
                {
                    currentUnit.color = m_DeadUnitColorTint;
                }
            }
        }

        private void OnEntityRevive(int entityId)
        {
            UISprite currentUnit;
            if (m_UnitsInUse.TryGetValue(entityId, out currentUnit))
            {
                currentUnit.color = Color.white;
            }
        }

        #region FSM States

        private abstract class StateBase : FsmState<MiniMap>
        {
            protected IFsm<MiniMap> m_Fsm;

            protected MiniMap Owner
            {
                get
                {
                    if (m_Fsm == null) return null;
                    return m_Fsm.Owner;
                }
            }

            protected override void OnInit(IFsm<MiniMap> fsm)
            {
                base.OnInit(fsm);
                m_Fsm = fsm;
            }

            protected override void OnLeave(IFsm<MiniMap> fsm, bool isShutdown)
            {
                if (isShutdown)
                {
                    Owner.m_MapTexture.mainTexture = null;
                    NGUIExtensionMethods.ReleaseTextureIfNeeded(Owner.m_MapTexture.GetHashCode());
                }

                base.OnLeave(fsm, isShutdown);
            }
        }

        private class StateInit : StateBase
        {
            private bool m_MapTextureLoaded = false;

            protected override void OnEnter(IFsm<MiniMap> fsm)
            {
                base.OnEnter(fsm);
                Owner.m_InstanceLogic = GameEntry.SceneLogic.MimicMeleeInstanceLogic;
                m_MapTextureLoaded = false;
                Owner.m_MapTexture.LoadAsync(Owner.m_InstanceLogic.MiniMapTextureId, OnLoadTextureSuccess, OnLoadTextureFailure);
            }

            private void OnLoadTextureFailure(UITexture uiTexture, object userData)
            {
                m_MapTextureLoaded = true;
            }

            private void OnLoadTextureSuccess(UITexture uiTexture, object userData)
            {
                m_MapTextureLoaded = true;
            }

            protected override void OnUpdate(IFsm<MiniMap> fsm, float elapseSeconds, float realElapseSeconds)
            {
                if (!m_MapTextureLoaded)
                {
                    return;
                }

                foreach (int entityId in Owner.m_InstanceLogic.GetTargetableObjectEntityIds())
                {
                    Owner.OnEntityShow(entityId);
                }

                ChangeState<StateNormal>(fsm);
            }
        }

        private class StateNormal : StateBase
        {
            protected override void OnEnter(IFsm<MiniMap> fsm)
            {
                base.OnEnter(fsm);
                GameEntry.Event.Subscribe(EventId.TargetableObjectShowInMimicMelee, OnEntityShow);
                GameEntry.Event.Subscribe(EventId.TargetableObjectHideInMimicMelee, OnEntityHide);
                GameEntry.Event.Subscribe(EventId.CharacterDead, OnCharacterDead);
                GameEntry.Event.Subscribe(EventId.Revive, OnMyHeroRevive);
            }

            protected override void OnLeave(IFsm<MiniMap> fsm, bool isShutdown)
            {
                GameEntry.Event.Unsubscribe(EventId.TargetableObjectShowInMimicMelee, OnEntityShow);
                GameEntry.Event.Unsubscribe(EventId.TargetableObjectHideInMimicMelee, OnEntityHide);
                GameEntry.Event.Unsubscribe(EventId.CharacterDead, OnCharacterDead);
                GameEntry.Event.Unsubscribe(EventId.Revive, OnMyHeroRevive);
                base.OnLeave(fsm, isShutdown);
            }

            private void OnMyHeroRevive(object sender, GameEventArgs e)
            {
                var ne = e as ReviveEventArgs;
                Owner.OnEntityRevive(ne.HeroCharacter.Id);
            }

            private void OnCharacterDead(object sender, GameEventArgs e)
            {
                var ne = e as CharacterDeadEventArgs;
                Owner.OnEntityDead(ne.CharacterData.Id);
            }

            private void OnEntityHide(object sender, GameEventArgs e)
            {
                var ne = e as TargetableObjectHideInMimicMeleeEventArgs;
                Owner.OnEntityHide(ne.EntityId);
            }

            private void OnEntityShow(object sender, GameEventArgs e)
            {
                var ne = e as TargetableObjectShowInMimicMeleeEventArgs;
                Owner.OnEntityShow(ne.EntityId);
            }

            protected override void OnUpdate(IFsm<MiniMap> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                var enumerator = Owner.m_UnitsInUse.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    int entityId = enumerator.Current.Key;
                    var unitOnMap = enumerator.Current.Value;
                    var entity = GameEntry.Entity.GetGameEntity(entityId);
                    if (entity == null)
                    {
                        continue;
                    }

                    unitOnMap.cachedTransform.SetLocalPositionX(Owner.m_InstanceLogic.MiniMapScale * entity.CachedTransform.position.x + Owner.m_InstanceLogic.MiniMapOffset.x);
                    unitOnMap.cachedTransform.SetLocalPositionY(Owner.m_InstanceLogic.MiniMapScale * entity.CachedTransform.position.z + Owner.m_InstanceLogic.MiniMapOffset.y);
                }
            }
        }

        #endregion FSM States
    }
}
