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
    public partial class Player : MonoBehaviour
    {

        [Tooltip("Layers that the player will collide with")]
        public LayerMask collisionLayerMask;
        private Rigidbody rb;
        private Animator animator;
        private int enemyLayer;
        
        private void Start()
        {
            co = null;
            enemyLayer = LayerMask.NameToLayer("Enemy");

            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            isGrounded = false;
        }

        private void Update()
        {
            HandleInput();

        }

        private void FixedUpdate()
        {
            rb.AddForce(gravity, ForceMode.Acceleration);
        }

        private void OnCollisionEnter(Collision coll)
        {
            int goLayer = coll.gameObject.layer;
            LayerMask layerMask2 = 1 << goLayer;

            if ((collisionLayerMask & layerMask2) != 0)
            {
                EndJump();
            }

        }

        
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
                SetAndCheckReversedStatus(true);

            }
            else
            {
                SetAndCheckReversedStatus(false);
            }

            FlipPlayer();
        }
      

       

    }
}