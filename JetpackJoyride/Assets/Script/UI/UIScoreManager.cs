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
        UIManager.OnGameStart += PlayerStartedGame;

        GameManager.OnPlayerScores += PlayerHasScored;
        GameManager.OnPlayerReachedNewHighScore += PlayerHasNewHighScore;
    }

    #region Event Handling
    private void PlayerHasNewHighScore(object sender, int newHighScore) => SetHighestScore(newHighScore);
    private void PlayerHasScored(object sender, int score) => UpdateCurrentScore(score);

    private void PlayerStartedGame(object sender, System.EventArgs e) => StartCoroutine(MoveScoresDownwards());
    #endregion

    private void UpdateCurrentScore(int score) => CurrentScoreText.text = score.ToString();
    private void SetHighestScore(int highestScore) => CurrentScoreText.text = highestScore.ToString();

    public void ResetScores()
    {
        CurrentScoreText.text = 0.ToString();
        HighestScoreText.text = "---";

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
