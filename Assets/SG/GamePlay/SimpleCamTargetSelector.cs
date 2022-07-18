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
            if (plOne.IsActive() && !plTwo.IsActive())
            {
                previousSelected = targetTrs = tOne;
            }
            else if (!plOne.IsActive() && plTwo.IsActive())
            {
                previousSelected = targetTrs = tTwo;
            }
           
            return targetTrs;
        }
    }
}