using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent : MonoBehaviour
    {
        private const float MoveDistanceLimit = 0.5f;

        public void UpdateNearbyPlayersPosition()
        {
            if (GameEntry.OfflineMode.OfflineModeEnabled)
            {
                return;
            }
            CLUpdatePosition msg = new CLUpdatePosition();
            msg.Position = GameEntry.Data.Player.LoginPostion;
            GameEntry.Network.Send(msg);
        }

        public void ShowNearbyPlayers()
        {
            var nearbyPlayers = GameEntry.Data.NearbyPlayers.Data;
            for (int i = 0; i < nearbyPlayers.Count; i++)
            {
                if (GameEntry.Entity.GetEntity(nearbyPlayers[i].Player.Id) != null)
                {
                    continue;
                }
                HeroData nearbyHero = new HeroData(nearbyPlayers[i].Player.Id, false);
                nearbyHero.HeroId = nearbyPlayers[i].MainHeroType;
                nearbyPlayers[i].RandomMovement = GameEntry.Data.NearbyPlayers.GenerateRandomMovement();
                DRHero drHero = GameEntry.DataTable.GetDataTable<DRHero>().GetDataRow(nearbyHero.HeroId);
                if (drHero == null)
                {
                    continue;
                }

                nearbyHero.CharacterId = drHero.CharacterId;
                nearbyHero.Speed = drHero.Speed;
                nearbyHero.Scale = drHero.Scale;
                nearbyHero.Position = new Vector2(nearbyPlayers[i].RandomMovement.TargetPosition.x, nearbyPlayers[i].RandomMovement.TargetPosition.z);
                nearbyHero.WeaponSuiteId = drHero.DefaultWeaponSuiteId;
                GameEntry.Entity.ShowLobbyHero(nearbyHero);
            }
        }

        public void RefreshNearbyPlayers()
        {
            if (GameEntry.OfflineMode.OfflineModeEnabled)
            {
                return;
            }
            CLRefreshNearbyPlayers msg = new CLRefreshNearbyPlayers();
            var players = GameEntry.Data.NearbyPlayers;
            msg.Count = players.MaxPlayerCount;
            msg.NeedHeroTypes.AddRange(players.CurHeroTypes);
            msg.CurrentNearbyPlayerIds.AddRange(players.CurPlayerIds);
            GameEntry.Network.Send(msg);
        }

        public void RefreshNearbyPlayerMoveState(NearbyPlayerData player)
        {
            if (player == null)
            {
                return;
            }

            if (player.RandomMovement.IsArriveTargetPos)
            {
                return;
            }
            var entity = GameEntry.Entity.GetEntity(player.Player.Id);
            if (entity == null)
            {
                return;
            }

            var nearbyPlayer = entity.Logic as HeroCharacter;
            if (nearbyPlayer == null)
            {
                return;
            }

            if (Vector3.SqrMagnitude(player.RandomMovement.TargetPosition - nearbyPlayer.CachedTransform.position) < MoveDistanceLimit)
            {
                player.RandomMovement.IsArriveTargetPos = true;
                player.RandomMovement.ArrivalTime = GameEntry.Time.LobbyServerUtcTime;
                player.RandomMovement.IsStartMove = true;
                return;
            }
        }

        public void MoveNearbyPlayer(NearbyPlayerData player)
        {
            if (player == null || player.RandomMovement.IsArriveTargetPos)
            {
                return;
            }
            var entity = GameEntry.Entity.GetEntity(player.Player.Id);
            if (entity == null)
            {
                return;
            }

            var nearbyPlayer = entity.Logic as HeroCharacter;
            if (nearbyPlayer == null)
            {
                return;
            }

            if (Vector3.SqrMagnitude(player.RandomMovement.TargetPosition - nearbyPlayer.CachedTransform.position) < MoveDistanceLimit)
            {
                player.RandomMovement.IsArriveTargetPos = true;
                player.RandomMovement.ArrivalTime = GameEntry.Time.LobbyServerUtcTime;
                player.RandomMovement.IsStartMove = true;
                return;
            }

            if (player.RandomMovement.IsStartMove)
            {
                return;
            }
            player.RandomMovement.IsStartMove = true;
            nearbyPlayer.Motion.StartMove(player.RandomMovement.TargetPosition);
        }
    }
}
