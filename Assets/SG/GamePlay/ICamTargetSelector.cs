using UnityEngine;

namespace Assets.SG.GamePlay
{
    public interface ICamTargetSelector
    {
         Transform ChooseTargetToFollow();

    }
}