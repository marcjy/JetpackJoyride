using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObstacleGenerator : MonoBehaviour
{
    public BaseObstacle[] ObstaclePrefabs;
    public float SecondsPerEnemy;

    private Coroutine _generatorCoroutine;
    private Dictionary<Type, ObjectPool<BaseObstacle>> _obstaclePools;
    private Dictionary<int, Tuple<Type, BaseObstacle>> _activeObjectsInPools;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _generatorCoroutine = null;

        _activeObjectsInPools = new Dictionary<int, Tuple<Type, BaseObstacle>>();
        _obstaclePools = new Dictionary<Type, ObjectPool<BaseObstacle>>();

        foreach (BaseObstacle baseObstacle in ObstaclePrefabs)
        {
            _obstaclePools.Add(baseObstacle.GetType(), new ObjectPool<BaseObstacle>(
                createFunc: () => CreateObstalceInPool(baseObstacle),
                actionOnGet: obstacle => GetObstacleFromPool(obstacle),
                actionOnRelease: obstacle => ReleaseObstacleFromPool(obstacle),
                actionOnDestroy: obstacle => DestroyObstacleFromPool(obstacle),
                false, 2, 4));
        }
    }

    private void Start()
    {
        GameManager.OnStartNewGame += HandleStartNewGame;
        GameManager.OnResetGame += HandleResetGame;
        GameManager.OnGameEnds += HandleGameEnded;
    }

    private void TurnOn() => _generatorCoroutine = StartCoroutine(GeneratorLoop());
    private void TurnOff()
    {
        StopCoroutine(_generatorCoroutine);
        _generatorCoroutine = null;

        StopAllObstacles();
    }

    private void StopAllObstacles()
    {
        foreach (Tuple<Type, BaseObstacle> tuple in _activeObjectsInPools.Values)
            tuple.Item2.enabled = false;
    }

    private void HandleStartNewGame(object sender, EventArgs e) => TurnOn();
    private void HandleResetGame(object sender, EventArgs e) => ReleaseAllObjects();
    private void HandleGameEnded(object sender, EventArgs e) => TurnOff();

    private void HandleObstacleShouldBeReleased(object sender, System.EventArgs e)
    {
        Type obstacleType = sender.GetType();
        _obstaclePools[obstacleType].Release((BaseObstacle)sender);
    }

    #region ObjectPool
    private BaseObstacle CreateObstalceInPool(BaseObstacle obstacle)
    {
        BaseObstacle newObstacle = Instantiate(obstacle);
        newObstacle.OnShouldBeReleased += HandleObstacleShouldBeReleased;
        return newObstacle;
    }
    private void GetObstacleFromPool(BaseObstacle obstacle)
    {
        obstacle.Init();
        _activeObjectsInPools.Add(obstacle.GetInstanceID(), new Tuple<Type, BaseObstacle>(obstacle.GetType(), obstacle));
        obstacle.gameObject.SetActive(true);
        obstacle.enabled = true;
    }
    private void ReleaseObstacleFromPool(BaseObstacle obstacle)
    {
        _activeObjectsInPools.Remove(obstacle.GetInstanceID());
        obstacle.gameObject.SetActive(false);
    }
    private void DestroyObstacleFromPool(BaseObstacle obstacle)
    {
        obstacle.OnShouldBeReleased -= HandleObstacleShouldBeReleased;
    }
    private void ReleaseAllObjects()
    {
        List<Tuple<Type, BaseObstacle>> obstaclesToRelease = new List<Tuple<Type, BaseObstacle>>();

        foreach (Tuple<Type, BaseObstacle> tuple in _activeObjectsInPools.Values)
            obstaclesToRelease.Add(tuple);

        foreach (Tuple<Type, BaseObstacle> tuple in obstaclesToRelease)
            _obstaclePools[tuple.Item1].Release(tuple.Item2);
    }
    #endregion


    private IEnumerator GeneratorLoop()
    {
        while (true)
        {
            Type obstacleType = GetRandomObstacleFromList();
            _obstaclePools[obstacleType].Get();

            yield return new WaitForSeconds(SecondsPerEnemy);
        }
    }

    private Type GetRandomObstacleFromList() => ObstaclePrefabs[UnityEngine.Random.Range(0, ObstaclePrefabs.Length)].GetType();
}
