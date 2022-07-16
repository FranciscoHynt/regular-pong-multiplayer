using Mirror;
using UnityEngine;

namespace Player
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Rigidbody2D rigidbody2d;

        private Vector2 ballVelocity = Vector2.zero;

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