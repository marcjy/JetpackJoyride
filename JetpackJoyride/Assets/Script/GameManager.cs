using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event EventHandler<int> OnPlayerScores;
    public static event EventHandler<int> OnPlayerReachedNewHighScore;

    public ObstacleGenerator ObstacleGenerator;
    public BackgroundManager BackgroundManager;

    [Header("Score Gains")]
    [Tooltip("How many seconds the player has to survive to score points.")]
    public float ScoreIntervalSeconds = 3.0f;
    [Tooltip("How many points the player will get per interval survived.")]
    public int ScorePerInterval = 20;

    private int _currentScore = 0;
    private Coroutine _scorePointsForSurvivingCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.OnGameStart += OnPlayerStartedGame;
    }

    private void OnPlayerStartedGame(object sender, System.EventArgs e) => StartGame();

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        BackgroundManager.TurnOn();
        ObstacleGenerator.TurnOn();

        _scorePointsForSurvivingCoroutine = StartCoroutine(PlayerScoresForSurviving());
    }

    public void PauseGame()
    {
        BackgroundManager.TurnOff();
        ObstacleGenerator.TurnOff();
    }
    public void ResetGame()
    {
        PauseGame();

        BackgroundManager.ResetPositions();
        ObstacleGenerator.ReleaseAllObjects();
        ResetScores();

        StartGame();
    }
    private void ResetScores()
    {
        _currentScore = 0;

        StopCoroutine(_scorePointsForSurvivingCoroutine);
        _scorePointsForSurvivingCoroutine = null;
    }

    private IEnumerator PlayerScoresForSurviving()
    {
        while (true)
        {
            yield return new WaitForSeconds(ScoreIntervalSeconds);

            _currentScore += ScorePerInterval;
            OnPlayerScores?.Invoke(this, _currentScore);
        }
    }


}
