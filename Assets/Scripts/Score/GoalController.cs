using Enums;
using Events;
using Mirror;
using UnityEngine;

namespace Score
{
    public class GoalController : NetworkBehaviour
    {
        private PlayerSide scoringSide;

        public override void OnStartServer()
        {
            base.OnStartServer();

            scoringSide = transform.position.x > 0 ? PlayerSide.Left : PlayerSide.Right;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (isServer && other.collider.CompareTag("Ball"))
            {
                GameEvents.GoalEvent.Invoke(scoringSide);
            }
        }
    }
}