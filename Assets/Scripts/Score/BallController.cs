using Mirror;
using ObjectPool.Instances;
using Player;
using UnityEngine;

namespace Score
{
    public class BallController : NetworkBehaviour
    {
        [SerializeField] private float speed = 30;
        [SerializeField] private float colliderHeight;
        [SerializeField] private HitVfx particleEffect;
        [SerializeField] private Rigidbody2D rigidbody2d;

        public override void OnStartServer()
        {
            base.OnStartServer();

            rigidbody2d.simulated = true;
            rigidbody2d.velocity = Vector2.right * speed;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            TrailRenderer trail = gameObject.AddComponent<TrailRenderer>();
            trail.time = 1f;
            trail.emitting = true;
            trail.widthCurve = AnimationCurve.EaseInOut(0, 0.5f, 1, 0);
            trail.sharedMaterial = Resources.Load<Material>("Materials/CyanMaterial");
        }

        float HitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
        {
            return (ballPos.y - racketPos.y) / racketHeight;
        }

        [ServerCallback]
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.transform.GetComponent<PlayerController>())
            {
                float y = HitFactor(transform.position, col.transform.position, colliderHeight);
                float x = col.relativeVelocity.x > 0 ? 1 : -1;

                Vector2 dir;
                dir.x = x;
                dir.y = y;

                rigidbody2d.velocity = dir.normalized * speed;

                DisplayVfx(col.GetContact(0).point);
            }
        }

        [ClientRpc]
        private void DisplayVfx(Vector3 position)
        {
            HitVfx pooledInstance = particleEffect.GetPooledInstance<HitVfx>();
            pooledInstance.transform.position = position;
        }
    }
}