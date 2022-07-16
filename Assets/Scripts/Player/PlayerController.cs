using Enums;
using Mirror;
using UnityEngine;

namespace Player
{
    public class PlayerController : NetworkBehaviour
    {
        public PlayerSide playerSide { get; set; }
        
        [SerializeField] private float speed;
        [SerializeField] private Rigidbody2D rigidbody2d;
        
        private Vector2 ballVelocity = Vector2.zero;

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            playerSide = transform.position.x < 0 ? PlayerSide.Left : PlayerSide.Right;
        }

        private void FixedUpdate()
        {
            if (isLocalPlayer)
            {
                ballVelocity.y = Input.GetAxisRaw("Vertical");
                rigidbody2d.velocity = ballVelocity * speed * Time.fixedDeltaTime;
            }
        }
    }
}