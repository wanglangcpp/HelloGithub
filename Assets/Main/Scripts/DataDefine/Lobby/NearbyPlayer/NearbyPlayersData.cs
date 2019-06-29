using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class NearbyPlayersData
    {
        [SerializeField]
        private List<NearbyPlayerData> m_Data = new List<NearbyPlayerData>();

        [SerializeField]
        private List<int> m_RemoveList = new List<int>();

        [SerializeField]
        private int m_MaxModelCount = 10;

        [SerializeField]
        private int m_MaxPlayerCount = 20;

        [SerializeField]
        private List<int> m_RandomPositionWeightList = new List<int>();

        [SerializeField]
        private List<int> m_RandomPositionIdsList = new List<int>();

        [SerializeField]
        private List<int> m_CurHeroTypes = new List<int>();

        [SerializeField]
        private int m_SumWeigth = 0;

        public int MaxModelCount
        {
            get
            {
                return m_MaxModelCount;
            }
            set
            {
                m_MaxModelCount = value;
            }
        }

        public int MaxPlayerCount
        {
            get
            {
                return m_MaxPlayerCount;
            }
            set
            {
                m_MaxPlayerCount = value;
            }
        }

        public List<NearbyPlayerData> Data
        {
            get
            {
                return m_Data;
            }
        }

        public List<int> CurHeroTypes
        {
            get
            {
                return m_CurHeroTypes;
            }
        }

        public List<int> CurPlayerIds
        {
            get
            {
                List<int> playerIds = new List<int>();
                for (int i = 0; i < m_Data.Count; i++)
                {
                    playerIds.Add(m_Data[i].Player.Id);
                }
                return playerIds;
            }
        }

        public void InitCurHeroTypes()
        {
            if (m_CurHeroTypes.Count > 0)
            {
                return;
            }
            m_MaxModelCount = GameEntry.ClientConfig.GetMaxNearbyPlayerModelTypeCount();
            m_MaxPlayerCount = GameEntry.ClientConfig.GetMaxNearbyPlayerCount();
            var rows = GameEntry.DataTable.GetDataTable<DRHero>().GetAllDataRows();
            List<int> allHeroIds = new List<int>();
            for (int i = 0; i < rows.Length; i++)
            {
                allHeroIds.Add(rows[i].Id);
            }
            for (int i = 0; m_CurHeroTypes.Count <= MaxModelCount && i < allHeroIds.Count;)
            {
                int index = UnityEngine.Random.Range(0, allHeroIds.Count);
                m_CurHeroTypes.Add(allHeroIds[index]);
                allHeroIds.Remove(allHeroIds[index]);
            }
        }

        public List<int> RemoveList
        {
            get
            {
                return m_RemoveList;
            }
        }

        public Vector2 GenerateRandomPosition()
        {
            Vector2 vec = Vector2.zero;
            InitRandomPositionWeightList();
            int randomWeight = UnityEngine.Random.Range(0, m_SumWeigth);
            int randomId = 0;
            for (int i = 0; i < m_RandomPositionWeightList.Count; i++)
            {
                if (randomWeight <= m_RandomPositionWeightList[i])
                {
                    randomId = m_RandomPositionIdsList[i];
                    break;
                }
            }

            if (randomId == 0)
            {
                randomId = m_RandomPositionWeightList[UnityEngine.Random.Range(0, m_RandomPositionWeightList.Count)];
            }

            var row = GameEntry.DataTable.GetDataTable<DRNearbyPlayerRandomPosition>().GetDataRow(randomId);
            if (row == null)
            {
                Log.Warning("DRNearbyPlayerRandomPosition' Data is invalid,table id is {0}.", randomId);
                return vec;
            }

            vec = new Vector2(row.PositionX, row.PositionY);
            vec += UnityEngine.Random.insideUnitCircle * row.RandomRadius;
            return vec;
        }

        public NearbyPlayerMovement InitRandomMovement(Vector2 initPosition)
        {
            var position = new NearbyPlayerMovement();
            position.TargetPosition = AIUtility.SamplePosition(initPosition);
            int randomId = m_RandomPositionWeightList[UnityEngine.Random.Range(0, m_RandomPositionWeightList.Count)];
            var row = GameEntry.DataTable.GetDataTable<DRNearbyPlayerRandomPosition>().GetDataRow(randomId);
            if (row == null)
            {
                Log.Warning("DRNearbyPlayerRandomPosition' Data is invalid,table id is {0}.", randomId);
                return null;
            }

            position.StayTime = UnityEngine.Random.Range(row.MinStayTime, row.MaxStayTime);
            return position;
        }

        public NearbyPlayerMovement GenerateRandomMovement()
        {
            InitRandomPositionWeightList();
            int randomWeight = UnityEngine.Random.Range(0, m_SumWeigth);
            int randomId = 0;
            for (int i = 0; i < m_RandomPositionWeightList.Count; i++)
            {
                if (randomWeight <= m_RandomPositionWeightList[i])
                {
                    randomId = m_RandomPositionIdsList[i];
                    break;
                }                
            }

            if (randomId == 0)
            {
                randomId = m_RandomPositionWeightList[UnityEngine.Random.Range(0, m_RandomPositionWeightList.Count)];
            }

            var row = GameEntry.DataTable.GetDataTable<DRNearbyPlayerRandomPosition>().GetDataRow(randomId);
            if (row == null)
            {
                Log.Warning("DRNearbyPlayerRandomPosition' Data is invalid,table id is {0}.", randomId);
                return null;
            }

            NearbyPlayerMovement position = new NearbyPlayerMovement();
            Vector2 pos = new Vector2(row.PositionX, row.PositionY);
            pos += UnityEngine.Random.insideUnitCircle * row.RandomRadius;
            position.TargetPosition = AIUtility.SamplePosition(pos);
            position.StayTime = UnityEngine.Random.Range(row.MinStayTime, row.MaxStayTime);
            return position;
        }

        public void ClearAllData()
        {
            m_Data.Clear();
            m_CurHeroTypes.Clear();
        }

        public NearbyPlayerData GetData(int key)
        {
            for (int i = 0; i < m_Data.Count; i++)
            {
                if (m_Data[i].Key == key)
                {
                    return m_Data[i];
                }
            }

            return null;
        }

        private void InitRandomPositionWeightList()
        {
            if (m_RandomPositionWeightList.Count == 0)
            {
                m_RandomPositionIdsList.Clear();
                m_SumWeigth = 0;
                var rows = GameEntry.DataTable.GetDataTable<DRNearbyPlayerRandomPosition>().GetAllDataRows();
                for (int i = 0; i < rows.Length; i++)
                {
                    m_SumWeigth += rows[i].Weight;
                    m_RandomPositionWeightList.Add(m_SumWeigth);
                    m_RandomPositionIdsList.Add(rows[i].Id);
                }
            }
        }

        public void UpdateData(LCRefreshNearbyPlayers data)
        {
            InitRandomPositionWeightList();
            var reserveNearbyPlayerIds = data.ReserveNearbyPlayerIds;
            for (int i = 0; i < m_Data.Count;)
            {
                if (!reserveNearbyPlayerIds.Contains(m_Data[i].Player.Id))
                {
                    m_RemoveList.Add(m_Data[i].Player.Id);
                    m_Data.Remove(m_Data[i]);
                    continue;
                }
                i++;
            }

            var newNearbyPlayers = data.NewNearbyPlayers;
            for (int i = 0; i < newNearbyPlayers.Count; i++)
            {
                var nearbyPlayer = new NearbyPlayerData();
                nearbyPlayer.UpdateData(newNearbyPlayers[i]);
                m_Data.Add(nearbyPlayer);
            }
        }
    }
}
