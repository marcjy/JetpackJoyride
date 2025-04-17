using System;
using UnityEngine;

public class JetpackController : MonoBehaviour
{
    public float ThrustForce = 30.0f;
    [Range(0, 1)]
    public float DampingFactor = 0.90f;

    private bool _jetpackOn;
    private Rigidbody2D _playerRigidBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _jetpackOn = false;
        _playerRigidBody = GetComponent<Rigidbody2D>();

        InputManager.Instance.OnThrusting += HandleStartThrusting;
        InputManager.Instance.OnStopThrusting += HandleStopThrusting;
    }

    private void HandleStartThrusting(object sender, System.EventArgs e) => _jetpackOn = true;
    private void HandleStopThrusting(object sender, EventArgs e) => _jetpackOn = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_jetpackOn)
        {
            _playerRigidBody.AddForceY(ThrustForce, ForceMode2D.Force);
        }
        else
        {
            if (_playerRigidBody.linearVelocityY > 0)
            {
                _playerRigidBody.linearVelocityY *= DampingFactor;
            }
        }
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnThrusting -= HandleStartThrusting;
        InputManager.Instance.OnStopThrusting -= HandleStopThrusting;
    }

}
