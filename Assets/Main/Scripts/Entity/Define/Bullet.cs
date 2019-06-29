using BehaviorDesigner.Runtime;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class Bullet : ShedObject, ICampable
    {
        [SerializeField]
        protected BulletData m_BulletData = null;

        private ITimeLineInstance<Entity> m_LastTimeLineInstance = null;

        public new BulletData Data
        {
            get
            {
                return m_BulletData;
            }
        }

        public BehaviorTree Behavior
        {
            get;
            private set;
        }

        public float? LeftTime
        {
            get;
            private set;
        }

        public CampType Camp
        {
            get
            {
                return Data.Camp;
            }
        }

        public bool IsReboundable
        {
            get
            {
                return Data.IsReboundable;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            GameEntry.SceneLogic.BaseInstanceLogic.RegisterBullet(this);

            GameEntry.Event.Subscribe(EventId.LoadBehaviorSuccess, OnLoadBehaviorSuccess);
            GameEntry.Event.Subscribe(EventId.LoadBehaviorFailure, OnLoadBehaviorFailure);
            GameEntry.Event.Subscribe(EventId.OwnerAskToPerformSkill, OnOwnerAskToPerformSkill);

            m_BulletData = userData as BulletData;
            if (m_BulletData == null)
            {
                Log.Error("Bullet data is invalid.");
                return;
            }

            CachedTransform.localPosition += Vector3.up * m_BulletData.Height;

            IDataTable<DRBullet> dtBullet = GameEntry.DataTable.GetDataTable<DRBullet>();
            DRBullet dataRow = dtBullet.GetDataRow(m_BulletData.BulletId);
            if (dataRow == null)
            {
                Log.Warning("Can not load bullet '{0}' from data table.", m_BulletData.BulletId.ToString());
                return;
            }

            if (dataRow.KeepTime >= 0f)
            {
                LeftTime = dataRow.KeepTime;
            }
            else
            {
                LeftTime = null;
            }

            Behavior = gameObject.AddComponent<BehaviorTree>();
            Behavior.StartWhenEnabled = false;
            Behavior.ExternalBehavior = null;
            GameEntry.Behavior.LoadBehavior(Behavior, dataRow.AIBehavior);

            if (m_BulletData.TransformParentType == TransformParentType.Owner)
            {
                if (Owner != null && Owner.IsAvailable)
                {
                    var targetOwner = Owner as ITargetable;
                    if (targetOwner != null && targetOwner.IsDead)
                    {
                        GameEntry.Entity.HideEntity(this.Entity);
                    }
                    else
                    {
                        GameEntry.Entity.AttachEntity(this.Entity, Owner.Entity);
                    }

                }
                else
                {
                    GameEntry.Entity.HideEntity(this.Entity);
                }
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            // TODO: Tackle movement when TransformParentType is not Default.
            if (Data.TransformParentType == TransformParentType.Default && Data.Speed > 0f)
            {
                CachedTransform.Translate(CachedTransform.forward * Data.Speed * Time.deltaTime, Space.World);
            }

            if (LeftTime != null)
            {
                LeftTime -= elapseSeconds;
                if (LeftTime <= 0f)
                {
                    LeftTime = null;
                    GameEntry.Entity.HideEntity(this.Entity);
                }
            }
        }

        protected override void OnHide(object userData)
        {
            if (!GameEntry.IsAvailable) return;

            GameEntry.Event.Unsubscribe(EventId.LoadBehaviorSuccess, OnLoadBehaviorSuccess);
            GameEntry.Event.Unsubscribe(EventId.LoadBehaviorFailure, OnLoadBehaviorFailure);
            GameEntry.Event.Unsubscribe(EventId.OwnerAskToPerformSkill, OnOwnerAskToPerformSkill);

            DestroyLastTimeLineInstance();

            if (Behavior != null)
            {
                Behavior.DisableBehavior();
                GameEntry.Behavior.UnloadBehavior(Behavior.ExternalBehavior);
                Destroy(Behavior);
                Behavior = null;
            }

            GameEntry.SceneLogic.BaseInstanceLogic.UnregisterBullet(Id);
            base.OnHide(userData);
        }

        public void PerformSkill(int skillId)
        {
            DestroyLastTimeLineInstance();
            m_LastTimeLineInstance = GameEntry.TimeLine.Entity.CreateTimeLineInstance(this, skillId, new Dictionary<string, object>
            {
                { Constant.TimeLineOwnerSkillIndexKey, Data.OwnerSkillIndex },
                { Constant.TimeLineOwnerSkillLevelKey, Data.OwnerSkillLevel },
            });

            if (m_LastTimeLineInstance == null)
            {
                Log.Warning("Can not create bullet time line instance '{0}'.", skillId.ToString());
                return;
            }
        }

        private void DestroyLastTimeLineInstance()
        {
            if (m_LastTimeLineInstance != null)
            {
                GameEntry.TimeLine.Entity.DestroyTimeLineInstance(m_LastTimeLineInstance);
                m_LastTimeLineInstance = null;
            }
        }

        private void OnLoadBehaviorSuccess(object sender, GameEventArgs e)
        {
            LoadBehaviorSuccessEventArgs ne = e as LoadBehaviorSuccessEventArgs;
            if (ne.Behavior != Behavior)
            {
                return;
            }

            if (GameEntry.IsAvailable && !GameEntry.SceneLogic.BaseInstanceLogic.ShouldProhibitAI)
            {
                Behavior.EnableBehavior();
            }

            //Log.Debug("Load behavior '{0}' OK.", ne.BehaviorName);
        }

        private void OnLoadBehaviorFailure(object sender, GameEventArgs e)
        {
            LoadBehaviorFailureEventArgs ne = e as LoadBehaviorFailureEventArgs;
            if (ne.Behavior != Behavior)
            {
                return;
            }

            Log.Warning("Can not load behavior '{0}' from '{1}' with error message '{2}'.", ne.BehaviorName, ne.BehaviorAssetName, ne.ErrorMessage);
        }

        private void OnOwnerAskToPerformSkill(object sender, GameEventArgs e)
        {
            var ne = e as OwnerAskToPerformSkillEventArgs;
            if (!HasOwner || ne.OwnerEntityId != Owner.Id || ne.TargetBulletTypeId != Data.BulletId)
            {
                return;
            }

            PerformSkill(ne.SkillId);
        }
    }
}
