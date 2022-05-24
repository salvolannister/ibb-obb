using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SG.GamePlay
{
    public class CheckPoint : MonoBehaviour
    {
        [Header("Set dynamically")]
        [SerializeField]
        private bool isChecked = false;
        private int playerLayer;
        public void Start()
        {
            playerLayer = LayerMask.NameToLayer("Player");
        }
        void OnTriggerEnter(Collider other)
        {
            if (!isChecked && other.gameObject.layer == playerLayer)
            {
                GameManager.CheckPointReached(gameObject.transform.GetChild(0).transform, gameObject.transform.GetChild(1).transform);
                isChecked = true;
                gameObject.SetActive(false);
            }
        }

    }
}