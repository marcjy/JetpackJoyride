
using System.Collections;
using UnityEngine;

public class FlyingEnemyController : BaseObstacle
{
    [Header("Flying Enemy")]
    public float MaxPositionY;
    public float MinPositionY;
    public float MaxDistanceTraveled;
    public float SpeedY;

    private Coroutine _moveCoroutine;

    public override void Init()
    {
        transform.position = GetRandomPosition();
    }

    void OnEnable()
    {
        _moveCoroutine = StartCoroutine(Move());
    }

    private void OnDisable()
    {
        StopCoroutine(_moveCoroutine);
        _moveCoroutine = null;
    }

    private IEnumerator Move()
    {
        while (true)
        {
            yield return MoveToPosition(MaxPositionY, Vector3.up);
            yield return MoveToPosition(MinPositionY, Vector3.down);
        }
    }

    private IEnumerator MoveToPosition(float targetPosition, Vector3 directionY)
    {
        float distanceTraveled = 0.0f;

        while (Mathf.Abs(transform.position.y - targetPosition) > 0.1f && distanceTraveled < MaxDistanceTraveled)
        {
            Vector3 previousPosition = transform.position;
            transform.position += directionY * SpeedY * Time.deltaTime;
            distanceTraveled += Mathf.Abs(transform.position.y - previousPosition.y);
            yield return null;
        }
    }

    private Vector3 GetRandomPosition() => new Vector3(SpawnPositionX, Random.Range(MinPositionY, MaxPositionY));
}
