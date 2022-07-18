using Assets.SG.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.SG.GamePlay
{

    public class CameraController : MonoBehaviour
    {
        public GameObject target;
        public GameObject targetTwo;
        public int smoothvalue = 2;
        public float posY = 1;

        private ICamTargetSelector targetSelector;
        private Transform targetOneTrs;
        private Transform targetTwoTrs;
        private Transform targetTrs;
        private Vector3 oldCamPos;
        private const float MIN_ZOOM_VALUE = -10f;

        private void OnEnable()
        {
            Assert.IsNotNull(target, " Set the first target for the cameraCameraController");
            Assert.IsNotNull(targetTwo, " Set the second target for the cameraController");
            GameManager.Get().OnGameOver += ResetVision;

        }

        void Start()
        {
            targetOneTrs = target.transform;
            targetTwoTrs = targetTwo.transform;
            targetTrs = targetOneTrs;
            targetSelector = GetComponent<ICamTargetSelector>();
            Assert.IsNotNull(targetSelector, " Cam target selector script missing");
        }
        private void OnDisable()
        {
            GameManager.Get().OnGameOver -= ResetVision;
        }
        private void ResetVision()
        {
            transform.position = targetOneTrs.position;
        }

        void Update()
        {
            LerpCameraToTarget();

        }

        private void LerpCameraToTarget()
        {
            targetTrs = targetSelector.ChooseTargetToFollow();
            oldCamPos = transform.position;
            Vector3 targetpos = new Vector3(targetTrs.position.x, targetTrs.position.y + posY, MIN_ZOOM_VALUE);
            transform.position = Vector3.Lerp(transform.position, targetpos, Time.deltaTime * smoothvalue);

            if (!IsCameraPosValid())
            {
                transform.position = oldCamPos;
            }
        }



        /// <summary>
        /// Return true only if both player are inside screen bounds
        /// </summary>
        /// <returns></returns>
        private bool IsCameraPosValid ()
        {
            if (targetTrs != targetOneTrs && ScreenBounds.OOB(targetOneTrs.position))
            {
                return false;
            }
            else if (ScreenBounds.OOB(targetTwoTrs.position))
            {
                return false;
            }

            return true;
        }
    }
}