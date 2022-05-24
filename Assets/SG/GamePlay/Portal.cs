using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SG.GamePlay
{
    /// <summary>
    /// Portal manages the behaviour of the player passes throught it, it could be extened to add particular
    /// effects of differen kind of acting in base of the tipology of the portal
    /// </summary>
    public class Portal : MonoBehaviour
    {
        [Header("Dynamically Set")]
        [SerializeField]
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

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == playerLayer)
            {
                Player pl = other.gameObject.GetComponent<Player>();
                if (pl != null)
                {
                    pl.HandleMaxYSpeed();
                }
            }

        }
    }
}