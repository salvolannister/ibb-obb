using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SG.GamePlay
{
    public partial class Player
    {
        [Header("Set in inspector")]
        [SerializeField]
        private KeyCode jumpKey = KeyCode.None;
        [SerializeField]
        private KeyCode leftShiftKey = KeyCode.None;
        [SerializeField]
        private KeyCode rightShiftKey = KeyCode.None;

        private bool isJumping = false;
        [SerializeField]
        private float moveSpeed = 0.5f;
        public float jumpForce = 5f;
        private Vector3 lastDir = Vector3.zero;
        private bool isGrounded;
        private void HandleInput()
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

            int runAnim = isWalking? 1 : 0;
            animator.SetInteger("Run", runAnim);

            if (Input.GetKeyDown(jumpKey) && !isJumping && isGrounded)
            {
                Jump();
            }
        }
        /// <summary>
        /// Decelerates the player to prevent it going out of screen bounds
        /// </summary>
        public void HandleMaxYSpeed()
        {
            if (Math.Abs(rb.velocity.y) > PlayerSettings.Y_VELOCITY_LIMIT)
            {
                rb.velocity *= PlayerSettings.VEL_DECELERATION_FACTOR;
                rb.AddForce(-gravity * PlayerSettings.FORCE_DECELERATION_FACTOR, ForceMode.Impulse);
            }
        }

        private void Jump()
        {
            isGrounded = false;
            isJumping = true;
            animator.Play("Jump");
            rb.velocity = Vector3.zero;
            rb.AddForce(jumpForce * -gravity, ForceMode.Impulse);

        }

        private void Translate(Vector3 direction)
        {
            lastDir = direction;
            int gDir = IsReversed;
            transform.rotation = Quaternion.LookRotation(direction, -Vector3.up * gDir);
            transform.transform.Translate(direction * moveSpeed * Time.deltaTime, 0);
        }

        private void FlipPlayer()
        {
            int gravityDir = IsReversed;
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

        private void EndJump()
        {
            isGrounded = true;
            if (isJumping)
            {
                isJumping = false;
            }
        }
    }
}
