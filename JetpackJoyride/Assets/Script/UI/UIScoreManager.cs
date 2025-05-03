using System.Collections;
using TMPro;
using UnityEngine;

public class UIScoreManager : MonoBehaviour
{
    public TextMeshProUGUI CurrentScoreText;
    public TextMeshProUGUI HighestScoreText;

    public float AnimationDuration = 1.5f;
    public RectTransform Scores;
    public float ScoresTargetPositionY;

    private float _scoresInitialPositionY;

    private void Awake()
    {
        _scoresInitialPositionY = Scores.anchoredPosition.y;

        ResetScores();
    }

    private void Start()
    {
        GameManager.OnStartNewGame += HandleStartNewGame;
        GameManager.OnResetGame += HandleResetGame;

        GameManager.OnPlayerScores += HandlePlayerScores;
        GameManager.OnPlayerReachedNewHighScore += HandlePlayerReachedNewHighscore;
    }

    #region Event Handling
    private void HandlePlayerReachedNewHighscore(object sender, int newHighScore) => SetHighestScore(newHighScore);
    private void HandlePlayerScores(object sender, int score) => UpdateCurrentScore(score);

    private void HandleStartNewGame(object sender, System.EventArgs e) => StartCoroutine(MoveScoresDownwards());
    private void HandleResetGame(object sender, System.EventArgs e) => ResetScores();
    #endregion

    private void UpdateCurrentScore(int score) => CurrentScoreText.text = score.ToString();
    private void SetHighestScore(int highestScore) => HighestScoreText.text = highestScore.ToString();
    private void ResetScores()
    {
        int highestScore = PlayerPrefs.GetInt("HighestScore");

        CurrentScoreText.text = 0.ToString();
        HighestScoreText.text = highestScore != 0 ? highestScore.ToString() : "---";

        Scores.anchoredPosition = new Vector2(Scores.anchoredPosition.x, _scoresInitialPositionY);
    }


    private IEnumerator MoveScoresDownwards()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < AnimationDuration)
        {
            Scores.anchoredPosition = new Vector2(Scores.anchoredPosition.x, Mathf.Lerp(_scoresInitialPositionY, ScoresTargetPositionY, elapsedTime / AnimationDuration));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Scores.anchoredPosition = new Vector2(Scores.anchoredPosition.x, ScoresTargetPositionY);
    }

}
