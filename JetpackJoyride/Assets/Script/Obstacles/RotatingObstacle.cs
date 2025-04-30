using UnityEngine;

public class RotatingObstacle : BaseObstacle
{
    [Header("Rotating Obstacle")]
    public float MaxSpawnPointY;
    public float MinSpawnPointY;

    public float RotationSpeed = 25;
    public override void Init()
    {
        transform.position = GetRandomPosition();
    }

    protected override void Update()
    {
        base.Update();
        transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);
    }

    private Vector3 GetRandomPosition() => new Vector3(SpawnPositionX, Random.Range(MinSpawnPointY, MaxSpawnPointY));
}
