using System;
using System.Collections;
using UnityEngine;


namespace Assets.SG.GamePlay
{
    public class PlayerGravityHandler : MonoBehaviour, IPlayerGravityHandler
    {
        public Rigidbody rb;
        public IPlayerMovement playerMovement;
        [SerializeField]
        private Vector3 gravity = new Vector3(0, -9.8f, 0);
        private const float gravityChangeDelay = 0.5f; // 0.05f;
        private bool reversed = false;
        private Coroutine co = null;

        public int IsReversed
        {
            get
            {
                return reversed ? 1 : -1;
            }
        }

        public void ReverseGravity()
        {
            co = StartCoroutine(ReverseGravityCo());

        }

        private IEnumerator ReverseGravityCo()
        {
            reversed = !reversed;
            playerMovement.FlipPlayer();

            yield return new WaitForSeconds(gravityChangeDelay);
            gravity *= -1;
            co = null;
        }

        public void Start()
        {
            rb = GetComponent<Rigidbody>();
            playerMovement = GetComponent<IPlayerMovement>();
        }

        public void AddGravity()
        {
            rb.AddForce(gravity, ForceMode.Acceleration);
            

        }

        /// <summary>
        /// Set the player in the  desired reversed status, then checks if the gravity sign is correct for it 
        /// otherwise changes it
        /// </summary>
        /// <param name="reverse"></param>
        public void SetAndCheckReversedStatus(bool reverse)
        {

            if (reverse)
            {
                reversed = reverse;
                if (gravity.y < 0)
                    gravity *= -1;
            }
            else
            {
                reversed = reverse;
                if (gravity.y > 0)
                    gravity *= -1;
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

        public Vector3 GetGravity()
        {
            return gravity;
        }
    }
}