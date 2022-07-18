using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Rigidbody2D rigidbody2d;

        private Vector2 ballVelocity = Vector2.zero;
        private List<GameObject> childrenObjects = new List<GameObject>();

        public void Start()
        {
            childrenObjects = gameObject
                .GetChildren()
                .OrderBy(child => child.name)
                .ToList();
        }

        private void FixedUpdate()
        {
            if (isLocalPlayer)
            {
                ballVelocity.y = Input.GetAxisRaw("Vertical");
                rigidbody2d.velocity = ballVelocity * speed * Time.fixedDeltaTime;
            }
        }

        [ServerCallback]
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Ball"))
            {
                GameObject colliderGameObject = other.GetContact(0).otherCollider.gameObject;

                Debug.Log(colliderGameObject.name);
                Debug.Log(childrenObjects.IndexOf(colliderGameObject));
                colliderGameObject.SetActive(false);
                DeactivateObjectOnPlayer(childrenObjects.IndexOf(colliderGameObject));
            }
        }

        [ClientRpc]
        private void DeactivateObjectOnPlayer(int childIndex)
        {
            childrenObjects[childIndex].SetActive(false);
        }
    }
}