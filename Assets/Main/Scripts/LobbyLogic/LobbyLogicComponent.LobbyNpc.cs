using GameFramework.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        public void ShowAllLobbyNpcs()
        {
            var allRows = GameEntry.DataTable.GetDataTable<DRLobbyNpc>().GetAllDataRows();
            for (int i = 0; i < allRows.Length; i++)
            {
                var row = allRows[i];
                LobbyNpcData data = new LobbyNpcData(GameEntry.Entity.GetSerialId(), row.Id);
                GameEntry.Entity.ShowLobbyNpc(data);
            }
        }
    }
}
