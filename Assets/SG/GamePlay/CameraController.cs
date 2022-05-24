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

        private Transform targetOneTrs;
        private Transform targetTwoTrs;
        private Transform targetTrs;
        private Vector3 lastPosOne;
        private Vector3 lastPosTwo;

        // Use this for initialization
        public Coroutine my_co;

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
            lastPosOne = targetOneTrs.position;
            lastPosTwo = targetTwoTrs.position;
            targetTrs = targetOneTrs;
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
            // TODO: include also the second target in those calculation

            targetTrs = ChooseTargetToFollow(targetOneTrs, targetTwoTrs);
            lastPosOne = targetOneTrs.position;
            lastPosTwo = targetTwoTrs.position;
            Vector3 oldPos = transform.position;
            Vector3 targetpos = new Vector3(targetTrs.position.x, targetTrs.position.y + posY, -10);
            transform.position = Vector3.Lerp(transform.position, targetpos, Time.deltaTime * smoothvalue);

            if (!IsPositionValid())
            {
                transform.position = Vector3.Lerp(transform.position, oldPos, Time.deltaTime * smoothvalue);
            }


        }

        private Transform ChooseTargetToFollow(Transform tOne, Transform tTwo)
        {

            if (tOne.position != lastPosOne && tTwo.position != lastPosTwo)
            {
                return targetTrs;
            }
            else if (tOne.position != lastPosOne && tTwo.position == lastPosTwo)
            {
                return tOne;
            }
            else
            {
                return tTwo;
            }

        }

        private bool IsPositionValid()
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