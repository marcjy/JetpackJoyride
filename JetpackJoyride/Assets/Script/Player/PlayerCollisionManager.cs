using System;
using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
    public event EventHandler OnGroundCollision;
    public event EventHandler OnLeftGround;
    public event EventHandler OnEnemyCollision;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            OnGroundCollision?.Invoke(this, EventArgs.Empty);
        if (collision.gameObject.CompareTag("Enemy"))
            OnEnemyCollision?.Invoke(this, EventArgs.Empty);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            OnLeftGround?.Invoke(this, EventArgs.Empty);
    }
}
