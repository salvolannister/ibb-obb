using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace Assets.SG.GamePlay
{

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Animator))]
    // handle gravity, delegates input, enemy interactions and life and death

    // Delegates other script to act in its order
    public class PlayerController : MonoBehaviour
    {

        [Tooltip("Layers that the player will collide with")]
        public LayerMask layerMaskThatEndsJump;
        private Coroutine co;
        private int enemyLayer;
        private IPlayerMovement playerMovement;
        private IPlayerInputHandler playerInputHandler;
        private IPlayerGravityHandler playerGravityHandler;


        public bool IsActive()
        {
            return playerMovement.IsMoving();
        }
        private void Start()
        {
            co = null;
            enemyLayer = LayerMask.NameToLayer("Enemy");
            playerMovement = GetComponent<IPlayerMovement>();
            playerInputHandler = GetComponent<IPlayerInputHandler>();
            playerGravityHandler = GetComponent<IPlayerGravityHandler>();
        
            
        }

        private void Update()
        {
            playerInputHandler.HandleInput();

        }

        private void FixedUpdate()
        {
            playerGravityHandler.AddGravity();
        }

        private void OnCollisionEnter(Collision coll)
        {
            int goLayer = coll.gameObject.layer;
            LayerMask layerMask2 = 1 << goLayer;

            if ((layerMaskThatEndsJump & layerMask2) != 0)
            {
                playerMovement.EndJump();
            }

        }


        #region Enemy
        private void OnTriggerEnter(Collider other)
        {
            int goLayer = other.gameObject.layer;
            if (goLayer == enemyLayer)
            {
                InteractWithEnemy(other.gameObject);
            }
        }

        private void InteractWithEnemy(GameObject enemyGO)
        {
            Enemy enemy = enemyGO.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                switch (enemyGO.tag)
                {
                    case EnemyParts.Spirit:
                        {
                            enemy.Die();
                            break;
                        }
                    case EnemyParts.Crawler:
                        {
                            GameManager.GameOver(this.tag);
                            break;
                        }

                }

            }

        }

        public void Die(Action callback = null)
        {
            if (co != null)
                StopCoroutine(co);
            //This callback will be the other player Die() function, like in the original game I would like the player who hit the enemy
            //to die first after showing an explosion effect
            callback?.Invoke();
            gameObject.SetActive(false);

        }

        public void Respawn(Transform checkPointTrs)
        {
            transform.position = checkPointTrs.position;
            gameObject.SetActive(true);

            if ((checkPointTrs.parent.gameObject.CompareTag("UnderWorld")))
            {
                playerGravityHandler.SetAndCheckReversedStatus(true);

            }
            else
            {
                playerGravityHandler.SetAndCheckReversedStatus(false);
            }

            playerMovement.FlipPlayer();
        }
        #endregion



    }
}