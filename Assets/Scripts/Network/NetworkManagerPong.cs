using System;
using System.Collections.Generic;
using System.Linq;
using DataModel;
using Enums;
using Events;
using Mirror;
using Player;
using UnityEngine;

namespace Network
{
    public class NetworkManagerPong : NetworkManager
    {
        [SerializeField] private GameObject ball;
        [SerializeField] private List<RacketModel> positionList;

        private readonly List<PlayerModel> playersList = new List<PlayerModel>();

        public override void OnStartServer()
        {
            RegisterEvents();

            base.OnStartServer();
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
                ball = Instantiate(ball);
                NetworkServer.Spawn(ball);
            }
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (ball != null)
                NetworkServer.Destroy(ball);

            playersList.RemoveAll(model => model.ConnectionId == conn.connectionId);

            base.OnServerDisconnect(conn);
        }

        private void HandleGoal(PlayerSide playerSide)
        {
            playersList.First(model => model.Side == playerSide).AddScore();

            foreach (PlayerModel scores in playersList)
            {
                Debug.Log(scores.Side);
                Debug.Log(scores.Score);
            }
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
    }
}