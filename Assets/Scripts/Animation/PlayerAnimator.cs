using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimatorMachine))]
public class PlayerAnimator : MonoBehaviour
{
    public const string IdleState = "Idle";
    public const string WalkState = "Walk";
    public const string JumpState = "Jump";
    public const string HitState = "Hit";

    public const int HandsLayer = 1;

    public AnimatorMachine AnimatorMachine => GetComponent<AnimatorMachine>();

    public void Idle()
    {
        AnimatorMachine.ChangeState(IdleState);
    }

    public void Walk()
    {
        AnimatorMachine.ChangeState(WalkState);
    }

    public void Jump()
    {
        AnimatorMachine.ChangeState(JumpState);
    }

    public void Hit()
    {
        AnimatorMachine.ChangeState(HitState, 0.2f, HandsLayer);
    }

}
