using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SG.GamePlay
{
    public partial class Player
    {

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
            FlipPlayer();

            yield return new WaitForSeconds(gravityChangeDelay);
            gravity *= -1;
            co = null;
        }

        /// <summary>
        /// Set the player in the  desired reversed status, then checks if the gravity sign is correct for it 
        /// otherwise changes it
        /// </summary>
        /// <param name="reverse"></param>
        private void SetAndCheckReversedStatus(bool reverse)
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

    }
}
