using UnityEngine;

public class GroundedEnemy : BaseObstacle
{
    [Header("Grounded Enemy")]
    public float SpawnPositionY;

    private Animator _animator;

    protected override void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Init()
    {
        transform.position = new Vector3(SpawnPositionX, SpawnPositionY);
    }

    public void Kill() => _animator.SetBool("isDead", true);
}
