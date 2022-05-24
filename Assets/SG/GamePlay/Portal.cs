using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SG.GamePlay
{
    public class Portal : MonoBehaviour
    {
        [Header("Dynamically Set")]
        private int playerLayer;
        void Start()
        {
            playerLayer = LayerMask.NameToLayer("Player");
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == playerLayer)
            {
                Player pl = other.gameObject.GetComponent<Player>();
                if (pl != null)
                {
                    pl.ReverseGravity();
                }
            }
        }
    }
}