﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SG.GamePlay
{


    public class PlayerMovementManager : MonoBehaviour, IPlayerMovement, IPlayerInputHandler
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

        private bool isJumping = false;
        private Vector3 lastDir = Vector3.zero;
        private bool isGrounded;

        private bool isWalking;
        private Animator animator;
        private Rigidbody rb;
        private IPlayerGravityHandler gravityHandler;
        private float currentSpeed = 0;
        private Vector3 direction;
        private Vector3 dirBeforeStop;
        public float maxSpeed = 5;
        public bool IsJumping { get => isJumping; set => isJumping = value; }
        public bool IsWalking
        {

            get
            {

                return isWalking;
            }

            set
            {
                isWalking = value;

            }
        }

        public bool InPortal { get => inPortal; set => inPortal = value; }

        private bool inPortal;


        public void HandleInput()
        {
            isWalking = false;
            if (direction == Vector3.zero && lastDir != Vector3.zero)
            {
                dirBeforeStop = lastDir;
            }

            lastDir = direction;
            if (Input.GetKey(leftShiftKey))
            {
                direction = Vector3.left;
                isWalking = true;

            }
            else if (Input.GetKey(rightShiftKey))
            {
                direction = Vector3.right;
                isWalking = true;

            }
            else
            {
                direction = Vector3.zero;

            }
            int runAnim = isWalking ? 1 : 0;
            animator.SetInteger("Run", runAnim);
            if (Input.GetKeyDown(jumpKey) && CanJump())
            {
                Jump();
            }
        }



        public void FlipPlayer()
        {
            int gDir = gravityHandler.IsReversed;
            if (direction != Vector3.zero)
            {

                transform.rotation = Quaternion.LookRotation(direction, -Vector3.up * gDir);
            }
            else if (dirBeforeStop != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(dirBeforeStop, -Vector3.up * gDir);
            }
            else
            {
                Debug.Log("dir before stop" + dirBeforeStop);
            }
        }

        public void FixedUpdate()
        {
            FlipPlayer();
            Translate(direction);
        }

        public void Start()
        {
            isGrounded = false;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            gravityHandler = GetComponent<IPlayerGravityHandler>();
            dirBeforeStop = Vector3.right;
        }

        public void Jump()
        {
            isGrounded = false;
            isJumping = true;
            animator.Play("Jump");
            rb.velocity = new Vector3(rb.velocity.x, -jumpForce * gravityHandler.IsReversed);

        }

        public void Translate(Vector3 direction)
        {
            if (lastDir != direction && (lastDir == Vector3.left || lastDir == Vector3.right))
            {
                currentSpeed = 0;
            }

            if (direction == Vector3.zero)
            {
                isWalking = false;
                Decelerate();
                return;
            }


            isWalking = true;

            currentSpeed += moveSpeed * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
            Vector3 targetVelocity = new Vector3(direction.x * currentSpeed, rb.velocity.y);
            rb.velocity = targetVelocity;
        }

        public void Decelerate()
        {
            Debug.Log("rb velocity " + rb.velocity.x);
            if (currentSpeed != 0)
            {

                float velocityDir = Mathf.Sign(rb.velocity.x);
                currentSpeed -= moveSpeed * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
                Vector3 targetVelocity = new Vector3(dirBeforeStop.x * currentSpeed, rb.velocity.y);
                rb.velocity = targetVelocity;
                
            }
        }



        void IPlayerMovement.EndJump()
        {
            isGrounded = true;
            if (IsJumping)
            {
                IsJumping = false;
            }
        }

        public bool IsMoving()
        {
            return IsJumping || isWalking;
        }

        public bool CanJump()
        {
            return !IsJumping && isGrounded;
        }

    }
}
