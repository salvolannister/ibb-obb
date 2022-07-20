using UnityEngine;

namespace Assets.SG.GamePlay
{
    public interface IPlayerGravityHandler
    {
        int IsReversed { get; }
        void AddGravity();
        void ReverseGravity();
        void SetAndCheckReversedStatus(bool reverse);
        void HandleMaxYSpeed();
        Vector3 GetGravity();
    }
}