using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event EventHandler<int> OnPlayerScores;
    public static event EventHandler<int> OnPlayerReachedNewHighScore;

    public static event EventHandler OnStartNewGame;
    public static event EventHandler OnResetGame;
    public static event EventHandler OnGameEnds;

    public ObstacleGenerator ObstacleGenerator;
    public BackgroundManager BackgroundManager;
    public PlayerCollisionManager PlayerCollisionManager;

    [Header("Score Gains")]
    [Tooltip("How many seconds the player has to survive to score points.")]
    public float ScoreIntervalSeconds = 3.0f;
    [Tooltip("How many points the player will get per interval survived.")]
    public int ScorePerInterval = 20;

    private int _highestScore;
    private int _currentScore;
    private Coroutine _scorePointsForSurvivingCoroutine;

    private Vector3 _initialPlayerPosition;

    private const string PLAYER_PREF_HIGHEST_SCORE = "HighestScore";

    private void Awake()
    {
        _highestScore = PlayerPrefs.GetInt(PLAYER_PREF_HIGHEST_SCORE);
        _currentScore = 0;

        _initialPlayerPosition = PlayerCollisionManager.transform.position;
    }

    void Start()
    {
        UIManager.Instance.OnUIStartsNewGame += HandleUIStartsNewGame;
        UIManager.Instance.SubscribeToUIPlayAgainEvent((object sender, EventArgs e) => ResetGame());
        UIManager.Instance.SubscribeToUIQuitEvent((object sender, EventArgs e) => QuitGame());


        PlayerCollisionManager.OnEnemyCollision += HandlePlayerDied;
    }


    private void HandlePlayerDied(object sender, EventArgs e)
    {
        if (_currentScore > _highestScore)
        {
            PlayerPrefs.SetInt(PLAYER_PREF_HIGHEST_SCORE, _currentScore);
            PlayerPrefs.Save();

            OnPlayerReachedNewHighScore?.Invoke(this, _currentScore);
        }


        StopCoroutine(_scorePointsForSurvivingCoroutine);
        _scorePointsForSurvivingCoroutine = null;

        OnGameEnds?.Invoke(this, EventArgs.Empty);
    }
    private void HandleUIStartsNewGame(object sender, System.EventArgs e) => StartGame();

    private void StartGame()
    {
        _scorePointsForSurvivingCoroutine = StartCoroutine(PlayerScoresForSurviving());
        OnStartNewGame?.Invoke(this, EventArgs.Empty);
    }
    private void ResetGame()
    {
        _currentScore = 0;
        OnResetGame?.Invoke(this, EventArgs.Empty);

        PlayerCollisionManager.transform.position = _initialPlayerPosition;
        Camera.main.GetComponent<AudioSource>().Play();
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


    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
