using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public partial class RoomLogicComponent : MonoBehaviour
    {
        [SerializeField]
        private RoomConfig m_Config = null;

        public RoomConfig Config
        {
            get
            {
                return m_Config;
            }
        }

        private int m_PacketSerialId = 0;
        private Vector2 m_LastPosition = Vector2.zero;
        private float m_LastRotation = 0f;
        private float m_LastSentTime = 0f;

        private float m_RoomMovementSyncInterval;
        private float m_PositionDeltaThreshold;
        private float m_RotationDeltaThreshold;

        private IRoomLogger m_Logger = null;
        private readonly Queue<CRPacketWrapper> m_SentPackets = new Queue<CRPacketWrapper>();

        public bool AddLog(string tag, string logFormat, params object[] logParams)
        {
            if (m_Logger == null)
            {
                return false;
            }

            m_Logger.AddLog(tag, logFormat, logParams);
            return true;
        }

        public bool SaveLogFile()
        {
            if (m_Logger == null)
            {
                return false;
            }

            m_Logger.SaveLogFile();
            return true;
        }

        public void Init()
        {
            m_RoomMovementSyncInterval = .2f; // GameEntry.ServerConfig.GetFloat(Constant.ServerConfig.RoomMovementSyncInterval, 0.5f);
            m_PositionDeltaThreshold = .1f; // GameEntry.ServerConfig.GetFloat(Constant.ServerConfig.RoomPositionDeltaThreshold, 0.1f);
            m_RotationDeltaThreshold = 1f; // GameEntry.ServerConfig.GetFloat(Constant.ServerConfig.RoomRotationDeltaThreshold, 1f);
            InitLogger();
        }

        public void Reset()
        {
            m_PacketSerialId = 0;
            m_RoomMovementSyncInterval = 0f;
            m_PositionDeltaThreshold = 0f;
            m_RotationDeltaThreshold = 0f;
            DeinitLogger();
            m_SentPackets.Clear();
        }

        public void UpdateMoving(int entityId, Vector2 position, float rotation, bool forceSendPacket)
        {
            if (!forceSendPacket)
            {
                if (PositionDeltaIsTooSmall(position) && RotationDeltaIsTooSmall(rotation))
                {
                    return;
                }

                if (Time.unscaledTime - m_LastSentTime < m_RoomMovementSyncInterval)
                {
                    return;
                }
            }

            //Log.Info("Position: position: {0}, rotation: {1}", position, rotation);
            var request = new CREntityMove
            {
                SerialId = GetPacketSerialId(),
                EntityId = entityId,
                Transform = CacheAndGenerateTransform(position, rotation),
                IsKey = forceSendPacket,
            };

            m_SentPackets.Enqueue(new CRPacketWrapper(request.SerialId, request));
            GameEntry.Network.Send(request);
        }

        public void UpdateSkillRushing(int entityId, int skillId, Vector2 position, float rotation, bool hasJustStarted)
        {
            if (!hasJustStarted)
            {
                if (PositionDeltaIsTooSmall(position) && RotationDeltaIsTooSmall(rotation))
                {
                    return;
                }

                if (Time.unscaledTime - m_LastSentTime < m_RoomMovementSyncInterval)
                {
                    return;
                }
            }

            var request = new CREntitySkillRushing
            {
                SerialId = GetPacketSerialId(),
                EntityId = entityId,
                Transform = CacheAndGenerateTransform(position, rotation),
                SkillId = skillId,
            };

            m_SentPackets.Enqueue(new CRPacketWrapper(request.SerialId, request));
            GameEntry.Network.Send(request);
        }

        public void FastForwardMySkill(MeHeroCharacter myHeroCharacter, int skillId, float targetTime)
        {
            Transform myTransform = myHeroCharacter.CachedTransform;

            var request = new CREntityPerformSkillFF
            {
                SerialId = GetPacketSerialId(),
                EntityId = myHeroCharacter.Id,
                SkillId = skillId,
                TargetTime = targetTime,
                Transform = new PBTransformInfo
                {
                    PositionX = myTransform.localPosition.x,
                    PositionY = myTransform.localPosition.z,
                    Rotation = myTransform.localEulerAngles.y,
                }
            };

            AddLog("Fast Forward My Skill", "EntityId: {0}, Position: {1}, Rotation: {2}, SkillId: {3}, TargetTime: {4}",
                request.EntityId.ToString(), new Vector2(request.Transform.PositionX, request.Transform.PositionY).ToString(), request.Transform.Rotation.ToString(), request.SkillId.ToString(), request.TargetTime.ToString());
            m_SentPackets.Enqueue(new CRPacketWrapper(request.SerialId, request));
            GameEntry.Network.Send(request);
        }

        public void ClaimRoomReady()
        {
            AddLog("Claim Room Ready", "RoomData: {{{0}}}", GameEntry.Data.Room.ToString());
            GameEntry.Network.Send(new CRReadySingleMatch { });
            //GameEntry.Network.Send(new CRRoomReady { });
            //CRReadySingleMatch request = new CRReadySingleMatch();
            //GameEntry.Network.Send(request);
        }
        public void ClaimRoomReconnectReady()
        {
            AddLog("Claim Room Reconnect Ready", "RoomData: {{{0}}}", GameEntry.Data.Room.ToString());
            GameEntry.Network.Send(new CRReConnection());
        }
        /// <summary>
        /// 获取当前服务器战斗状态
        /// </summary>
        public void GetRoomBattleInfo()
        {
            AddLog("GetRoomBattleInfo", "GetRoomBattleInfo");
            CRGetRoomBattleStatus request = new CRGetRoomBattleStatus();
            GameEntry.Network.Send(request);
        }

        public void PerformSkillStart(int entityId, Vector2 position, float rotation, int skillId)
        {
            var request = new CREntityPerformSkillStart
            {
                SerialId = GetPacketSerialId(),
                EntityId = entityId,
                Transform = CacheAndGenerateTransform(position, rotation),
                SkillId = skillId,
            };

            AddLog("Perform Skill Start", "EntityId: {0}, Position: {1}, Rotation: {2}, SkillId: {3}",
                request.EntityId.ToString(), new Vector2(request.Transform.PositionX, request.Transform.PositionY).ToString(), request.Transform.Rotation.ToString(), request.SkillId.ToString());
            m_SentPackets.Enqueue(new CRPacketWrapper(request.SerialId, request));
            GameEntry.Network.Send(request);
        }

        public void PerformSkillEnd(int entityId, Vector2 position, float rotation, int skillId, SkillEndReasonType reason)
        {
            var request = new CREntityPerformSkillEnd
            {
                SerialId = GetPacketSerialId(),
                EntityId = entityId,
                Transform = CacheAndGenerateTransform(position, rotation),
                SkillId = skillId,
                Reason = (int)reason,
            };

            AddLog("Perform Skill End", "EntityId: {0}, Position: {1}, Rotation: {2}, SkillId: {3}, Reason: {4}",
                request.EntityId.ToString(), new Vector2(request.Transform.PositionX, request.Transform.PositionY).ToString(), request.Transform.Rotation.ToString(), request.SkillId.ToString(), request.Reason.ToString());
            m_SentPackets.Enqueue(new CRPacketWrapper(request.SerialId, request));
            GameEntry.Network.Send(request);
        }

        public void RequestSwitchHero(int oldEntityId, int newEntityId, int hp)
        {
            //return;
            var request = new CREntitySwitchHero
            {
                SerialId = GetPacketSerialId(),
                OldEntityId = oldEntityId,
                NewEntityId = newEntityId,
                HP = hp,
            };

            AddLog("Request Switch Hero", "OldEntityId: {0}, NewEntityId: {1}", oldEntityId.ToString(), newEntityId.ToString());
            m_SentPackets.Enqueue(new CRPacketWrapper(request.SerialId, request));
            GameEntry.Network.Send(request);
        }

        public void RequestImpact(CREntityImpact request)
        {
            AddLog("Request Impact", "Begins");
            request.SerialId = GetPacketSerialId();

            AddLog("Request Impact", "{{OriginOwnerEntityId: {0}}}", request.OriginOwnerEntityId);

            for (int i = 0; i < request.HPDamageImpacts.Count; ++i)
            {
                var impact = request.HPDamageImpacts[i];
                AddLog("Request Impact", "HP: {{Damage: {0}, Recover: {1}, SkillRecover: {2}, Counter: {3}, IsCritical: {4}, ImpactId: {5}}}",
                    impact.DamageHP.ToString(), impact.RecoverHP.ToString(), impact.SkillRecoverHP.ToString(), impact.CounterHP.ToString(), impact.IsCritical.ToString(), impact.ImpactId.ToString());
            }

            for (int i = 0; i < request.SteadyDamageImpacts.Count; ++i)
            {
                var impact = request.SteadyDamageImpacts[i];
                AddLog("Request Impact", "Steady: {{Damage: {0}, ImpactId: {1}}}", impact.DamageSteady.ToString(), impact.ImpactId.ToString());
            }

            AddLog("Request Impact", "Ends");
            m_SentPackets.Enqueue(new CRPacketWrapper(request.SerialId, request));
            GameEntry.Network.Send(request);
        }

        public void AddBuff(int originEntityId, int targetEntityId, int buffId)
        {
            var request = new CREntityAddBuff
            {
                SerialId = GetPacketSerialId(),
                OriginEntityId = originEntityId,
                TargetEntityId = targetEntityId,
            };

            //request.BuffTypeIds.Add(buffId);
            request.Buffs.Add(new PBBuffInfo() { SerialId = request.SerialId, BuffId = buffId, BuffStartTime = 0 });
            AddLog("Add Buff", "OriginalEntityId: {0}, TargetEntityId: {1}, BuffId: {2}", originEntityId.ToString(), targetEntityId.ToString(), buffId.ToString());
            m_SentPackets.Enqueue(new CRPacketWrapper(request.SerialId, request));
            GameEntry.Network.Send(request);
        }

        public void RemoveBuff(int originEntityId, int targetEntityId, List<PBBuffInfo> buffs)
        {
            var request = new CREntityRemoveBuff
            {
                SerialId = GetPacketSerialId(),
                OriginEntityId = originEntityId,
                TargetEntityId = targetEntityId,
            };

            request.Buffs.AddRange(buffs);
            AddLog("Remove Buff", "OriginalEntityId: {0}, TargetEntityId: {1}", originEntityId.ToString(), targetEntityId.ToString());
            m_SentPackets.Enqueue(new CRPacketWrapper(request.SerialId, request));
            GameEntry.Network.Send(request);
        }

        public void RequestGiveUpPvp()
        {
            CRGiveUpBattle request = new CRGiveUpBattle();
            GameEntry.Network.Send(request);

        }
        public void SetRequestResult(int serialId, bool result)
        {
            while (m_SentPackets.Count > 0 && m_SentPackets.Peek().SerialId < serialId)
            {
                m_SentPackets.Dequeue();
            }

            if (m_SentPackets.Count <= 0)
            {
                Log.Warning("You forgot to enqueue packet '{0}'. (Queue is empty.)", serialId.ToString());
                return;
            }

            if (m_SentPackets.Peek().SerialId > serialId)
            {
                Log.Warning("You forgot to enqueue packet '{0}'. (Wrong serial ID.)", serialId.ToString());
                return;
            }

            var request = m_SentPackets.Dequeue();
            if (result)
            {
                return;
            }

            UndoRequest(request);
        }

        private void UndoRequest(CRPacketWrapper request)
        {
            int actionId = request.Packet.PacketActionId;
            Log.Info("Undo action '{0}'.", actionId.ToString());
        }

        private bool PositionDeltaIsTooSmall(Vector2 position)
        {
            return Vector2.SqrMagnitude(position - m_LastPosition) < m_PositionDeltaThreshold * m_PositionDeltaThreshold;
        }

        private bool RotationDeltaIsTooSmall(float rotation)
        {
            float lastRotation = m_LastRotation;
            return Mathf.Abs(rotation - lastRotation) < m_RotationDeltaThreshold
                || Mathf.Abs(rotation + 360f - lastRotation) < m_RotationDeltaThreshold
                || Mathf.Abs(rotation - (lastRotation + 360f)) < m_RotationDeltaThreshold;
        }

        private PBTransformInfo CacheAndGenerateTransform(Vector2 position, float rotation)
        {
            m_LastPosition = position;
            m_LastRotation = rotation;
            m_LastSentTime = Time.unscaledTime;

            return new PBTransformInfo()
            {
                PositionX = position.x,
                PositionY = position.y,
                Rotation = rotation,
            };
        }

        private int GetPacketSerialId()
        {
            return m_PacketSerialId++;
        }

        private void InitLogger()
        {
            m_Logger = RoomLoggerFactory.Create();
            m_Logger.Init(true, Utility.Path.GetCombinePath(Application.temporaryCachePath, string.Format("RoomLog_{0:yyyyMMddHHmmss}.txt", GameEntry.Time.ClientTime)));
        }

        private void DeinitLogger()
        {
            if (m_Logger != null)
            {
                m_Logger.Shutdown();
                m_Logger = null;
            }
        }

        private class CRPacketWrapper
        {
            public CRPacketWrapper(int serialId, PacketBase packet)
            {
                SerialId = serialId;
                Packet = packet;
            }

            public int SerialId
            {
                get;
                private set;
            }

            public PacketBase Packet
            {
                get;
                private set;
            }
        }
    }
}
