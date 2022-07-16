using System;
using System.Collections.Generic;
using System.Linq;
using DataModel;
using Enums;
using Events;
using Mirror;
using Player;
using UnityEngine;
using Utils;

namespace Network
{
    public class NetworkManagerPong : NetworkManager
    {
        [SerializeField] private float ballRespawnTime;
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private List<RacketModel> positionList;

        private GameObject ballInstance;
        
        private readonly List<PlayerModel> playersList = new List<PlayerModel>();

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            GameEvents.GoalEvent.AddListener(HandleGoal);
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            PlayerModel newPlayer = new PlayerModel(conn.connectionId, GetPlayerSide());
            playersList.Add(newPlayer);

            GameObject player = Instantiate(playerPrefab, GetSideStartPosition(newPlayer.Side), Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player);

            if (numPlayers == maxConnections)
            {
                SpawnBall();
            }
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (ballInstance != null)
                NetworkServer.Destroy(ballInstance);

            playersList.RemoveAll(model => model.ConnectionId == conn.connectionId);

            base.OnServerDisconnect(conn);
        }

        [Server]
        private void HandleGoal(PlayerSide playerSide)
        {
            PlayerModel playerModel = playersList.First(model => model.Side == playerSide);
            playerModel.AddScore();

            NetworkServer.Destroy(ballInstance);
            this.CallWithDelay(SpawnBall, ballRespawnTime);

            GameEvents.ShowScoreEvent.Invoke(new ShowScoreData(playerModel.Score, playerModel.Side));
        }

        private PlayerSide GetPlayerSide()
        {
            if (numPlayers == 0)
                return PlayerSide.Left;

            return playersList[0].Side switch
            {
                PlayerSide.Left => PlayerSide.Right,
                PlayerSide.Right => PlayerSide.Left,
                _ => throw new ArgumentOutOfRangeException(nameof(PlayerModel.Side), "Side not implemented!")
            };
        }

        private Vector3 GetSideStartPosition(PlayerSide side)
        {
            return positionList.First(model => model.Side == side).Transform.position;
        }

        private void SpawnBall()
        {
            ballInstance = Instantiate(ballPrefab);
            NetworkServer.Spawn(ballInstance);
        }
    }
}