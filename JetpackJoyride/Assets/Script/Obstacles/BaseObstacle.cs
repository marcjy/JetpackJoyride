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
        DisableObstacle();

        GameObject explosion = Instantiate(ExplosionPrefab, gameObject.transform);

        Animator animator = explosion.GetComponent<Animator>();
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
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

    private void DisableObstacle()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false;
    }
}
