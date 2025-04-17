using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private PlayerCollisionManager _collisionManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _collisionManager = GetComponent<PlayerCollisionManager>();

        _collisionManager.OnGroundCollision += HandleGrounded;
        _collisionManager.OnLeftGround += HandleAirborne;
        _collisionManager.OnEnemyCollision += HandleDeath;

        InputManager.Instance.OnThrusting += HandleJetpackOn;
        InputManager.Instance.OnStopThrusting += HandleJetpackOff;
    }

    private void HandleJetpackOn(object sender, System.EventArgs e) => _animator.SetBool(PlayerAnimatorParams.IsJetpackOn, true);
    private void HandleJetpackOff(object sender, EventArgs e) => _animator.SetBool(PlayerAnimatorParams.IsJetpackOn, false);

    private void HandleGrounded(object sender, System.EventArgs e) => _animator.SetBool(PlayerAnimatorParams.IsGrounded, true);
    private void HandleAirborne(object sender, System.EventArgs e) => _animator.SetBool(PlayerAnimatorParams.IsGrounded, false);

    private void HandleDeath(object sender, System.EventArgs e) => _animator.SetBool(PlayerAnimatorParams.IsDead, true);
}
