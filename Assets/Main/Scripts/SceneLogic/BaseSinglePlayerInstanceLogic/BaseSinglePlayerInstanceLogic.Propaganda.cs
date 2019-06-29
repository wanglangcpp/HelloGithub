using GameFramework;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class BaseSinglePlayerInstanceLogic
    {
        private class PropogandaManager : IInstancePropagandaManager
        {
            public event GameFrameworkAction<InstancePropagandaData> OnPropagandaBegin;

            public event GameFrameworkAction OnPropagandaEnd;

            private LinkedList<InstancePropagandaData> m_Queue = new LinkedList<InstancePropagandaData>();
            private InstancePropagandaData m_Current = null;
            private float m_CurrentRemainingTime;
            private int m_Capacity;

            public InstancePropagandaData Current
            {
                get
                {
                    return m_Current;
                }
            }

            public PropogandaManager(int capacity)
            {
                m_Capacity = capacity;
            }

            public void SetCapacity(int capacity)
            {
                m_Capacity = capacity;
                CheckCount();
            }

            public void Add(InstancePropagandaData data)
            {
                if (data == null)
                {
                    Log.Warning("Propagation data is invalid.");
                    return;
                }

                var dataCopy = new InstancePropagandaData(data);

                // The queue must be empty under this condition.
                if (m_Current == null || m_Current.IsOverrideable)
                {
                    SetCurrent(dataCopy);
                    return;
                }

                // When current data exists and is not overrideable, check the queue itself.
                bool removed = false;
                if (m_Queue.Last != null && m_Queue.Last.Value.IsOverrideable)
                {
                    m_Queue.RemoveLast();
                    removed = true;
                }

                m_Queue.AddLast(dataCopy);

                if (!removed)
                {
                    CheckCount();
                }
            }

            public void Update(float realElapseTime)
            {
                if (m_Current == null)
                {
                    return;
                }

                m_CurrentRemainingTime -= realElapseTime;

                if (m_CurrentRemainingTime > 0f)
                {
                    return;
                }

                if (m_Queue.Count <= 0)
                {
                    m_Current = null;
                    m_CurrentRemainingTime = 0f;
                    if (OnPropagandaEnd != null)
                    {
                        OnPropagandaEnd();
                    }
                    return;
                }

                SetCurrent(m_Queue.First.Value);
                m_Queue.RemoveFirst();
            }

            public void Clear()
            {
                m_Queue.Clear();
                if (m_Current != null)
                {
                    m_Current = null;
                    m_CurrentRemainingTime = 0f;
                    if (OnPropagandaEnd != null)
                    {
                        OnPropagandaEnd();
                    }
                }
            }

            private void SetCurrent(InstancePropagandaData data)
            {
                m_Current = data;
                m_CurrentRemainingTime = data.Duration;
                if (OnPropagandaBegin != null)
                {
                    OnPropagandaBegin(m_Current);
                }
            }

            private void CheckCount()
            {
                while (m_Queue.Count > m_Capacity)
                {
                    m_Queue.RemoveFirst();
                }
            }
        }

        private IInstancePropagandaManager m_PropagandaManager;

        public InstancePropagandaData CurrentPropagandaData
        {
            get
            {
                if (m_PropagandaManager == null)
                {
                    return null;
                }

                return m_PropagandaManager.Current;
            }
        }

        public void AddPropaganda(InstancePropagandaData data)
        {
            if (m_PropagandaManager == null)
            {
                Log.Error("Propaganda manager is invalid.");
                return;
            }

            m_PropagandaManager.Add(data);
        }

        private void UpdatePropaganda(float realElapseTime)
        {
            if (m_PropagandaManager == null)
            {
                return;
            }

            m_PropagandaManager.Update(realElapseTime);
        }

        private void InitPropagandaManager()
        {
            m_PropagandaManager = new PropogandaManager(Constant.InstancePropagandaQueueCapacity);
            m_PropagandaManager.OnPropagandaBegin += OnPropagandaBegin;
            m_PropagandaManager.OnPropagandaEnd += OnPropagandaEnd;
        }

        private void DeinitPropagandaManager()
        {
            if (m_PropagandaManager != null)
            {
                m_PropagandaManager.Clear();
                m_PropagandaManager.OnPropagandaBegin -= OnPropagandaBegin;
                m_PropagandaManager.OnPropagandaEnd -= OnPropagandaEnd;
                m_PropagandaManager = null;
            }
        }

        private void OnPropagandaBegin(InstancePropagandaData data)
        {
            GameEntry.Event.Fire(this, new InstancePropagandaBeginEventArgs(data));
        }

        private void OnPropagandaEnd()
        {
            GameEntry.Event.Fire(this, new InstancePropagandaEndEventArgs());
        }
    }
}
