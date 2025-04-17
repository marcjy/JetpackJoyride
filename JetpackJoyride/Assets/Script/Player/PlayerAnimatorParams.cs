using UnityEngine;

public static class PlayerAnimatorParams
{
    public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    public static readonly int IsJetpackOn = Animator.StringToHash("IsJetpackOn");
    public static readonly int IsDead = Animator.StringToHash("IsDead");
}
