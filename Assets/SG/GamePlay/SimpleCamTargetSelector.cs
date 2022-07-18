using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.SG.GamePlay
{
    public class SimpleCamTargetSelector : MonoBehaviour, ICamTargetSelector
    {
        public Player plOne;
        public Player plTwo;

        private Transform tOne;
        private Transform tTwo;
        private Transform previousSelected;

        public void Awake()
        {
            Assert.IsNotNull(plOne);
            Assert.IsNotNull(plTwo);    
        }
        private void Start()
        {
            previousSelected = tOne = plOne.transform;
            tTwo = plTwo.transform;
        }
        public Transform ChooseTargetToFollow()
        {
            Transform targetTrs = previousSelected;
            if (plOne.IsWalking() && !plTwo.IsWalking())
            {
                previousSelected = targetTrs = tOne;
            }
            else if (!plOne.IsWalking() && plTwo.IsWalking())
            {
                previousSelected = targetTrs = tTwo;
            }
           
            return targetTrs;
        }
    }
}