using Enums;
using Events;
using Mirror;
using TMPro;
using UnityEngine;

namespace Interface
{
    public class InterfaceController : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI leftScore;
        [SerializeField] private TextMeshProUGUI rightScore;

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            RegisterEvents();
        }
        
        private void RegisterEvents()
        {
            GameEvents.ShowScoreEvent.AddListener(UpdateScores);
        }

        [ClientRpc]
        private void UpdateScores(ShowScoreData data)
        {
            switch (data.side)
            {
                case PlayerSide.Left:
                    leftScore.text = data.score.ToString();
                    break;
                case PlayerSide.Right:
                    rightScore.text = data.score.ToString();
                    break;
            }
        }
    }
}