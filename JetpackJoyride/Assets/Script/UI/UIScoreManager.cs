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
        UIManager.OnGameStart += OnPlayerStartedGame;
    }

    private void OnPlayerStartedGame(object sender, System.EventArgs e)
    {
        StartCoroutine(MoveScoresDownwards());
    }

    public void UpdateCurrentScore(int score) => CurrentScoreText.text = score.ToString();
    public void SetHighestScore(int highestScore) => CurrentScoreText.text = highestScore.ToString();

    public void ResetScores()
    {
        CurrentScoreText.text = 0.ToString();
        HighestScoreText.text = 0.ToString();

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
