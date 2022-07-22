using System;
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

        public void HandleInput()
        {
            isWalking = false;
            if (Input.GetKey(leftShiftKey))
            {
                Translate(Vector3.left);
                isWalking = true;

            }
            else if (Input.GetKey(rightShiftKey))
            {
                Translate(Vector3.right);
                isWalking = true;

            }
            int runAnim = isWalking ? 1 : 0;
            animator.SetInteger("Run", runAnim);
            if (Input.GetKeyDown(jumpKey) && CanJump())
            {
                Jump();
            }
        }





        public void Start()
        {
            isGrounded = false;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            gravityHandler = GetComponent<IPlayerGravityHandler>();
        }

        public void Jump()
        {
            isGrounded = false;
            isJumping = true;
            animator.Play("Jump");
            rb.velocity = Vector3.zero;
            rb.AddForce(jumpForce * -gravityHandler.GetGravity(), ForceMode.Impulse);

        }

        public void Translate(Vector3 direction)
        {
            if(lastDir != direction)
            {
                currentSpeed = 0;
            }

            lastDir = direction;
            if (direction == Vector3.zero)
            {
                isWalking = false;
                return;
            }
         

            isWalking = true;
            int gDir = gravityHandler.IsReversed;
            transform.rotation = Quaternion.LookRotation(direction, -Vector3.up * gDir);
            currentSpeed += moveSpeed * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
            rb.velocity = new Vector3(direction.x * currentSpeed, rb.velocity.y);
            //transform.transform.Translate(direction * moveSpeed * Time.deltaTime, 0);
        }

        public void FlipPlayer()
        {
            int gravityDir = gravityHandler.IsReversed;
            bool reversed = gravityDir == 1;
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
