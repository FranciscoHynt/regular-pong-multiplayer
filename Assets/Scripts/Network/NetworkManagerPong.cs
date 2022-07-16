using System.Collections.Generic;
using Enums;
using Events;
using Mirror;
using UnityEngine;

namespace Network
{
    public class NetworkManagerPong : NetworkManager
    {
        [SerializeField] private Transform leftRacketSpawn;
        [SerializeField] private Transform rightRacketSpawn;

        private GameObject ball;
        
        private readonly Dictionary<PlayerSide, int> scores = new Dictionary<PlayerSide, int>();

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
            Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
            GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
            NetworkServer.AddPlayerForConnection(conn, player);

            if (numPlayers == 2)
            {
                ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
                NetworkServer.Spawn(ball);
            }
            
        }
        
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (ball != null)
                NetworkServer.Destroy(ball);

            base.OnServerDisconnect(conn);
        }
        
        private void HandleGoal(PlayerSide playerSide)
        {
            scores[playerSide]++;
        }
    }
}