using Mirror;
using UnityEngine;

namespace Manager
{
    public class NetworkManagerPong : NetworkManager
    {
        [SerializeField] private Transform leftRacketSpawn;
        [SerializeField] private Transform rightRacketSpawn;

        private GameObject ball;

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
    }
}