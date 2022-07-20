using UnityEngine;

namespace Assets.SG.GamePlay
{
    public interface IPlayerMovement
    {
        bool IsWalking { get; set; }
        bool IsJumping { get; set; }
        void Jump();
        void Translate(Vector3 direction);

        void FlipPlayer();

        void EndJump();

        bool IsMoving();

        bool CanJump();

    }

}
