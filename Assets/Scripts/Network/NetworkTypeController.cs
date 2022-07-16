using Mirror;
using UnityEngine;

namespace Network
{
    public class NetworkTypeController : MonoBehaviour
    {
        private enum NetworkType {Server, Client}

        [SerializeField] private NetworkType type;

        private NetworkManager manager;

        private void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }

        private void Start()
        {
            switch (type)
            {
                case NetworkType.Server:
                    manager.StartServer();
                    break;
                case NetworkType.Client:
                    manager.StartClient();
                    break;
            }
        }
    }
}