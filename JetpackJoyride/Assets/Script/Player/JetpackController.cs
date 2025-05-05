using System;
using UnityEngine;

public class JetpackController : MonoBehaviour
{
    public float ThrustForce = 30.0f;
    [Range(0, 1)]
    public float DampingFactor = 0.90f;

    [Header("Machine Gun Paricle System")]
    public ParticleSystem MachineGunParticleSystem;
    public Vector2 BoxSize;
    public Vector2 BoxOffset;
    public LayerMask EnemyLayer;

    private bool _jetpackOn;
    private Rigidbody2D _playerRigidBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _jetpackOn = false;
        _playerRigidBody = GetComponent<Rigidbody2D>();

        MachineGunParticleSystem.gameObject.SetActive(false);

        InputManager.Instance.OnThrusting += HandleStartThrusting;
        InputManager.Instance.OnStopThrusting += HandleStopThrusting;
    }

    private void HandleStartThrusting(object sender, System.EventArgs e)
    {
        _jetpackOn = true;
        StartShooting();
    }
    private void HandleStopThrusting(object sender, EventArgs e)
    {
        _jetpackOn = false;
        StopShooting();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_jetpackOn)
        {
            _playerRigidBody.AddForceY(ThrustForce, ForceMode2D.Force);
            KillEnemiesBeneath();
        }
        else
        {
            if (_playerRigidBody.linearVelocityY > 0)
            {
                _playerRigidBody.linearVelocityY *= DampingFactor;
            }
        }
    }

    private void StartShooting()
    {
        MachineGunParticleSystem.gameObject.SetActive(true);
    }

    private void StopShooting()
    {
        MachineGunParticleSystem.gameObject.SetActive(false);
    }

    private void KillEnemiesBeneath()
    {
        Vector2 boxCenter = (Vector2)transform.position + BoxOffset;
        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, BoxSize, 0.0f, EnemyLayer);

        foreach (Collider2D hit in hits)
            hit.GetComponent<BaseObstacle>().Kill();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector2 boxCenter = (Vector2)transform.position + BoxOffset;
        Gizmos.DrawWireCube(boxCenter, BoxSize);
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnThrusting -= HandleStartThrusting;
        InputManager.Instance.OnStopThrusting -= HandleStopThrusting;
    }
}
