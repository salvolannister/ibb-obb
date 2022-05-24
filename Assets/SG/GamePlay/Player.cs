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
        [Header("Set in inspector")]
        [SerializeField]
        private KeyCode jumpKey = KeyCode.None;
        [SerializeField]
        private KeyCode leftShiftKey = KeyCode.None;
        [SerializeField]
        private KeyCode rightShiftKey = KeyCode.None;

        
        [SerializeField]
        private float moveSpeed = 0.5f;      
        public float jumpForce = 5f;

        private Rigidbody rb;
        private Animator animator;
        private bool isJumping = false;
        private Coroutine co = null;
        private Vector3 lastDir = Vector3.zero;
        private int floorLayer;
        private int playerLayer;
        private int enemyLayer;
        private int portalLayer;
        private bool grounded;

        private void FlipPlayer()
        {
            int gravityDir = reversed ? 1 : -1;
            if (lastDir == Vector3.right)
            {
                transform.rotation = Quaternion.LookRotation(
                    (reversed) ? Vector3.back : Vector3.forward, -Vector3.up * gravityDir);
            }
            else if (lastDir == Vector3.left)
            {
                transform.rotation = Quaternion.LookRotation(
                    (reversed) ? Vector3.forward : Vector3.back, -Vector3.up * gravityDir);
            }
        }

        private void Start()
        {
            co = null;
            floorLayer = LayerMask.NameToLayer("Floor");
            playerLayer = LayerMask.NameToLayer("Player");
            enemyLayer = LayerMask.NameToLayer("Enemy");
            portalLayer = LayerMask.NameToLayer("Portal");

            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            grounded = false;
        }

        private void Update()
        {
            int run = 0;
            if (Input.GetKey(leftShiftKey))
            {
                Translate(Vector3.left);
                run = 1;
            }
            else if (Input.GetKey(rightShiftKey))
            {
                Translate(Vector3.right);
                run = 1;
            }

            animator.SetInteger("Run", run);

            if (Input.GetKeyDown(jumpKey) && !isJumping && grounded)
            {
                Jump();
            }

        }


        private void OnTriggerExit(Collider coll)
        {
            if (coll.gameObject.layer == portalLayer)
            {
                HandleMaxYSpeed();
            }

        }

        private void HandleMaxYSpeed()
        {
            if (Math.Abs(rb.velocity.y) > PlayerSettings.Y_VELOCITY_LIMIT)
            {
                rb.velocity *= PlayerSettings.VEL_DECELERATION_FACTOR;
                rb.AddForce(-gravity * PlayerSettings.FORCE_DECELERATION_FACTOR, ForceMode.Impulse);
            }
        }

        private void FixedUpdate()
        {
            rb.AddForce(gravity, ForceMode.Acceleration);
        }

        private void OnCollisionEnter(Collision coll)
        {
            int goLayer = coll.gameObject.layer;
            if (goLayer == floorLayer || goLayer == playerLayer || goLayer == enemyLayer) //TOOD: use bit ?? how ?
            {
                grounded = true;
                if (isJumping)
                {
                    isJumping = false;
                }

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

        private void Jump()
        {
            grounded = false;
            isJumping = true;
            animator.Play("Jump");
            rb.velocity = Vector3.zero;
            rb.AddForce(jumpForce * -gravity, ForceMode.Impulse);

        }

        public void Respawn(Transform checkPointTrs)
        {
            transform.position = checkPointTrs.position;
            gameObject.SetActive(true);

            if ((checkPointTrs.position.y < 0))
            {
                reversed = true;
                if (gravity.y < 0)
                    gravity *= -1;

            }
            else
            {
                reversed = false;
                if (gravity.y > 0)
                    gravity *= -1;
            }

            FlipPlayer();
        }
        private void Translate(Vector3 direction)
        {
            lastDir = direction;
            int gDir = IsReversed;
            transform.rotation = Quaternion.LookRotation(direction, -Vector3.up * gDir);
            transform.transform.Translate(direction * moveSpeed * Time.deltaTime, 0);
        }


    
    }
}