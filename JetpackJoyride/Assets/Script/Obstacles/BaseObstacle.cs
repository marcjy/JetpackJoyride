using System;
using UnityEngine;

public abstract class BaseObstacle : MonoBehaviour
{
    public event EventHandler OnShouldBeReleased;

    [Header("Base")]
    public float SpawnPositionX = 9.5f;

    [Tooltip("When the GO reaches X, it will be destroyed")]
    public float LimitPositionX = -10;
    public float SpeedX = 2;

    public GameObject ExplosionPrefab;

    public abstract void Init();
    public virtual void Kill()
    {
        GameObject explosion = Instantiate(ExplosionPrefab);
        explosion.transform.position = gameObject.transform.position;

        OnShouldBeReleased?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        transform.position += Vector3.left * SpeedX * Time.deltaTime;

        if (transform.position.x < LimitPositionX)
            OnShouldBeReleased?.Invoke(this, EventArgs.Empty);
    }
}
