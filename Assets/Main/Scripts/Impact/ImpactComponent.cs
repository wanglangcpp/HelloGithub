using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 伤害组件。
    /// </summary>
    public partial class ImpactComponent : MonoBehaviour
    {
        /// <summary>
        /// 全局性屏蔽血量伤害的谓词型委托。
        /// </summary>
        public GameFrameworkFunc<ITargetable, bool> ShouldScreenHPDamage;

        [SerializeField]
        private ColliderTriggers m_ColliderTriggers = new ColliderTriggers();

        [SerializeField]
        private HudTexts m_HudTexts = new HudTexts();

        [SerializeField]
        private NameBoards m_NameBoards = new NameBoards();

        [SerializeField]
        private Bubbles m_Bubbles = new Bubbles();

        [SerializeField]
        private MonsterPositionArrows m_ArrowPrompts = new MonsterPositionArrows();

        private IDictionary<int, ImpactBase> m_Impacts = new Dictionary<int, ImpactBase>();
        private int m_HidingNameBoardCount = 0;

        public bool PreloadComplete
        {
            get
            {
                return m_HudTexts.PreloadComplete && m_NameBoards.PreloadComplete && m_Bubbles.PreloadComplete && m_ArrowPrompts.PreloadComplete;
            }
        }

        private void Start()
        {
            GameEntry.Event.Subscribe(EventId.WillChangeScene, OnWillChangeScene);

            m_ColliderTriggers.Init();
            m_HudTexts.Init();
            m_NameBoards.Init();
            m_Bubbles.Init();
            m_ArrowPrompts.Init();

            RegisterImpact(new HPDamageImpact());
            RegisterImpact(new HPRecoverImpact());
            RegisterImpact(new SteadyDamageImpact());
            RegisterImpact(new StiffnessImpact());
            RegisterImpact(new FloatImpact());
            RegisterImpact(new BlownAwayImpact());
            RegisterImpact(new StunImpact());
            RegisterImpact(new FreezeImpact());
            RegisterImpact(new SoundAndEffectImpact());
            RegisterImpact(new HardHitImpact());

            Log.Info("Impact component has been initialized.");
        }

        private void OnDestroy()
        {
            if (GameEntry.IsAvailable && GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(EventId.WillChangeScene, OnWillChangeScene);
            }

            m_ColliderTriggers.Shutdown();
            m_HudTexts.Shutdown();
            m_NameBoards.Shutdown();
            m_Bubbles.Shutdown();
            m_ArrowPrompts.Shutdown();
        }

        private void Update()
        {
            m_ColliderTriggers.Update();
            m_HudTexts.Update();
            m_NameBoards.Update();
            m_Bubbles.Update();
            m_ArrowPrompts.Update();
        }

        public void Preload()
        {
            m_HudTexts.Preload();
            m_NameBoards.Preload();
            m_Bubbles.Preload();
            m_ArrowPrompts.Preload();
        }

        public ColliderTrigger CreateColliderTrigger(string colliderTriggerName)
        {
            return m_ColliderTriggers.CreateColliderTrigger(colliderTriggerName);
        }

        public void DestroyColliderTrigger(ColliderTrigger colliderTrigger)
        {
            m_ColliderTriggers.DestroyColliderTrigger(colliderTrigger);
        }

        public HudText CreateHudText(int type, Vector3 worldOffset, string content, TargetableObject target)
        {
            if (!GameEntry.SceneLogic.BaseInstanceLogic.ShouldShowHud)
            {
                m_HudTexts.Clear();
                return null;
            }

            var building = target as Building;
            if (building != null)
            {
                var dataTable = GameEntry.DataTable.GetDataTable<DRBuilding>();

                DRBuilding dataRow = dataTable.GetDataRow(building.Data.BuildingTypeId);
                if (dataRow == null)
                {
                    Log.Error("Building '{0}' not found.", building.Data.BuildingTypeId.ToString());
                    return null;
                }

                if (!dataRow.ShowHudText)
                {
                    return null;
                }
            }
            return m_HudTexts.CreateHudText(type, worldOffset, content);
        }

        public BaseBubble CreateBubble(TargetableObject targetableObject, string content, float startTime, float keepTime)
        {
            return m_Bubbles.CreateBubble(targetableObject, content, startTime, keepTime);
        }

        public BaseMonsterPositionArrow CreateArrowPrompt(TargetableObject targetableObject)
        {
            return m_ArrowPrompts.CreateArrowPrompt(targetableObject);
        }

        public BaseNameBoard CreateNameBoard(Entity entity, NameBoardMode mode)
        {
            return m_NameBoards.CreateNameBoard(entity, mode);
        }

        public BaseNameBoard CreateNameBoard(TargetableObject targetableObject, NameBoardMode mode, float hpRatio, float animSeconds)
        {
            return m_NameBoards.CreateNameBoard(targetableObject, mode, hpRatio, animSeconds);
        }

        public void DestroyAllArrowPrompt()
        {
            m_ArrowPrompts.DestroyAllArrowPrompt();
        }

        public BaseNameBoard GetNameBoard(TargetableObject targetableObject)
        {
            return m_NameBoards.GetNameBoard(targetableObject);
        }

        public void DestroyNameBoard(BaseNameBoard hpBar)
        {
            m_NameBoards.DestroyNameBoard(hpBar);
        }

        public void IncreaseHidingNameBoardCount()
        {
            m_HidingNameBoardCount++;
            m_NameBoards.SetNameBoardVisible(m_HidingNameBoardCount <= 0);
        }

        public void DecreaseHidingNameBoardCount()
        {
            m_HidingNameBoardCount--;
            m_NameBoards.SetNameBoardVisible(m_HidingNameBoardCount <= 0);
        }

        public void PerformImpacts(ICampable origin, EntityData originData, ITargetable target, EntityData targetData, ImpactSourceType impactSourceType,
            IList<KeyValuePair<int, BuffType>> impactIdWithBuffConditions, int? skillId, int? skillLevel, int? skillIndex, SkillBadgesData skillBadges, float? currentTime, ImpactAuxData auxData)
        {
            if (!ShouldPerformImpacts(originData))
            {
                return;
            }

            CREntityImpact impactRequest = InitImpactRequest(origin, originData, target, targetData, impactSourceType, skillId, currentTime);

            IDataTable<DRImpact> dtImpact = GameEntry.DataTable.GetDataTable<DRImpact>();
            for (int i = 0; i < impactIdWithBuffConditions.Count; i++)
            {
                DRImpact dataRow = dtImpact.GetDataRow(impactIdWithBuffConditions[i].Key);
                if (dataRow == null)
                {
                    Log.Warning("Can not load impact '{0}' from data table.", impactIdWithBuffConditions[i].Key.ToString());
                    return;
                }

                ImpactBase impact = null;
                if (!m_Impacts.TryGetValue(dataRow.ImpactType, out impact))
                {
                    Log.Warning("Can not perform impact type '{0}'.", dataRow.ImpactType.ToString());
                    continue;
                }

                BasePerformImpactData performImpactData = GeneratePerformImpactData(origin, originData, target, targetData,
                    impactSourceType, dataRow, auxData, skillLevel, skillIndex, skillBadges, impactIdWithBuffConditions[i].Value);

                BaseApplyImpactData applyImpactData = impact.PerformImpact(performImpactData);
                if (applyImpactData == null)
                {
                    continue;
                }

                applyImpactData.SkillId = skillId;
                applyImpactData.AIId = null;
                applyImpactData.CurrentTime = currentTime;

                impact.ApplyImpact(applyImpactData);
                if (impactRequest != null)
                {
                    impact.FillPacket(impactRequest, applyImpactData);
                }
            }

            if (impactRequest != null)
            {
                if (auxData.BuffIdsToAddToTarget != null)
                {
                    impactRequest.BuffIdsAddedToTarget.AddRange(auxData.BuffIdsToAddToTarget);
                }
                if (auxData.CausingBuffId != null)
                {
                    impactRequest.BuffId = auxData.CausingBuffId.Value;
                }
                GameEntry.RoomLogic.RequestImpact(impactRequest);
            }
        }

        public void ApplyImpacts(BaseApplyImpactData[] impactDatas)
        {
            ImpactBase impact = null;
            for (int i = 0; i < impactDatas.Length; i++)
            {
                if (!m_Impacts.TryGetValue(impactDatas[i].DataRow.ImpactType, out impact))
                {
                    Log.Warning("Can not perform impact '{0}'.", impactDatas[i].DataRow.ImpactType.ToString());
                    continue;
                }

                impact.ApplyImpact(impactDatas[i]);
            }
        }

        private static bool ShouldPerformImpacts(EntityData originData)
        {
            return !GameEntry.Data.Room.InRoom || AIUtility.EntityDataIsMine(originData);
        }

        private static CREntityImpact InitImpactRequest(ICampable origin, EntityData originData,
            ITargetable target, EntityData targetData, ImpactSourceType impactSourceType, int? skillId, float? currentTime)
        {
            if (!GameEntry.Data.Room.InRoom)
            {
                return null;
            }

            var impactRequest = new CREntityImpact();

            if (originData != null)
            {
                impactRequest.OriginEntityId = originData.Id;

                Entity originEntity = origin as Entity;
                if (originEntity != null)
                {
                    impactRequest.OriginTransform = new PBTransformInfo()
                    {
                        PositionX = originEntity.CachedTransform.localPosition.x,
                        PositionY = originEntity.CachedTransform.localPosition.z,
                        Rotation = originEntity.CachedTransform.localRotation.eulerAngles.y,
                    };
                }

                var originOwnerEntityData = AIUtility.GetFinalOwnerData(originData);
                impactRequest.OriginOwnerEntityId = originOwnerEntityData.Id;
                var ownerEntity = originOwnerEntityData.Entity;
                if (ownerEntity != null)
                {
                    impactRequest.OriginOwnerTransform = new PBTransformInfo()
                    {
                        PositionX = ownerEntity.CachedTransform.localPosition.x,
                        PositionY = ownerEntity.CachedTransform.localPosition.z,
                        Rotation = ownerEntity.CachedTransform.localRotation.eulerAngles.y,
                    };
                }
            }

            if (targetData != null)
            {
                Entity targetEntity = target as Entity;
                impactRequest.TargetEntityId = targetData.Id;
                impactRequest.TargetTransform = new PBTransformInfo()
                {
                    PositionY = targetEntity.CachedTransform.localPosition.z,
                    PositionX = targetEntity.CachedTransform.localPosition.x,
                    Rotation = targetEntity.CachedTransform.localRotation.eulerAngles.y,
                };
            }

            impactRequest.ImpactSourceType = (int)impactSourceType;

            if (skillId.HasValue)
            {
                impactRequest.SkillId = skillId.Value;
            }

            if (currentTime.HasValue)
            {
                impactRequest.CurrentTime = currentTime.Value;
            }

            return impactRequest;
        }

        private BasePerformImpactData GeneratePerformImpactData(ICampable origin, EntityData originData, ITargetable target, EntityData targetData,
            ImpactSourceType sourceType, DRImpact dataRow, ImpactAuxData auxData, int? skillLevel, int? skillIndex, SkillBadgesData skillBadges, BuffType buffCondition)
        {
            BasePerformImpactData data = null;
            switch ((ImpactType)dataRow.ImpactType)
            {
                case ImpactType.HardHit:
                    data = new HardHitPerformImpactData();
                    break;
                case ImpactType.Stiffness:
                    data = new StiffnessPerformImpactData();
                    break;
                case ImpactType.BlownAway:
                    data = new BlownAwayPerformImpactData();
                    break;
                case ImpactType.Float:
                    data = new FloatPerformImpactData();
                    break;
                default:
                    data = new OtherPerformImpactData();
                    break;
            }

            data.SetDataRow(dataRow);
            data.Origin = origin;
            data.OriginData = originData;
            data.Target = target;
            data.TargetData = targetData;
            data.SourceType = sourceType;
            data.AuxData = auxData;
            data.SkillLevel = skillLevel ?? 1;
            data.SkillIndex = skillIndex ?? -1;
            data.SkillBadges = skillBadges;
            data.BuffCondition = buffCondition;
            return data;
        }

        private void RegisterImpact(ImpactBase impact)
        {
            if (m_Impacts.ContainsKey(impact.Type))
            {
                Log.Warning("Impact '{0}' is already exist.", impact.Type.ToString());
                return;
            }

            m_Impacts.Add(impact.Type, impact);
        }

        private void OnWillChangeScene(object sender, GameEventArgs e)
        {
            m_ColliderTriggers.Clear();
            m_HudTexts.Clear();
            m_NameBoards.Clear();
        }

        public void SpecialStateDispose(Character targetCharacter, ref float raiseFloatingDistance, ref ImpactAnimationType raiseAnimation, ref float fallingFloatingDistance, ref ImpactAnimationType fallingAnimation)
        {
            bool hasImmuneFloatImpactBuff = targetCharacter.HasImmuneFloatImpactBuff;
            if (hasImmuneFloatImpactBuff)
            {
                raiseFloatingDistance = 0f;
                raiseAnimation = ImpactAnimationType.HitAnimation;
                fallingFloatingDistance = 0f;
                fallingAnimation = ImpactAnimationType.HitAnimation;
            }
        }
    }
}
