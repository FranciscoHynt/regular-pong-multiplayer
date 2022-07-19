using System.Collections.Generic;
using System.Linq;
using Events;
using Mirror;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Rigidbody2D rigidbody2d;

        private Vector2 startPos;
        private NetworkTransform networkTransform;
        private Vector2 ballVelocity = Vector2.zero;
        private List<GameObject> childrenObjects = new List<GameObject>();

        public override void OnStartServer()
        {
            base.OnStartServer();

            GameEvents.GoalEvent.AddListener(side => RestartPlayers());
        }

        public void Start()
        {
            startPos = transform.position;

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

        private void RestartPlayers()
        {
            childrenObjects.ForEach(child => child.SetActive(true));
            RestartObjectsOnPlayer();
        }

        [ServerCallback]
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Ball"))
            {
                GameObject colliderGameObject = other.GetContact(0).otherCollider.gameObject;

                if (childrenObjects.Count(child => child.activeSelf) > 1)
                {
                    colliderGameObject.SetActive(false);
                    DeactivateObjectOnPlayer(childrenObjects.IndexOf(colliderGameObject));
                }
            }
        }

        [ClientRpc]
        private void DeactivateObjectOnPlayer(int childIndex)
        {
            childrenObjects[childIndex].SetActive(false);
        }

        [ClientRpc]
        private void RestartObjectsOnPlayer()
        {
            transform.position = startPos;
            childrenObjects.ForEach(child => child.SetActive(true));
        }
    }
}