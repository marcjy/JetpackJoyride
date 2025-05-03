using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverManager : MonoBehaviour
{
    public event EventHandler OnUITryAgainPressed;
    public event EventHandler OnUIQuitPressed;

    public RectTransform GameOverWindowTransform;
    public float ScaleUpAnimationDuration = 2.0f;

    [Header("New High Score")]
    public GameObject NewHighScoreWindow;
    public TextMeshProUGUI HighScoreValue;

    [Header("Buttons")]
    public Button TryAgainButton;
    public Button QuitButton;

    private Vector3 _initialGameOverWindowScale;

    private void Awake()
    {
        _initialGameOverWindowScale = GameOverWindowTransform.localScale;
        GameOverWindowTransform.gameObject.SetActive(false);
        NewHighScoreWindow.SetActive(false);

        InitButtons();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.OnGameEnds += HandleGameEnds;

        GameManager.OnPlayerReachedNewHighScore += HandlePlayerReachedNewHighScore;
        GameManager.OnResetGame += HandleResetGame;
    }

    private void HandleGameEnds(object sender, EventArgs e)
    {
        GameOverWindowTransform.gameObject.SetActive(true);
        StartCoroutine(ScaleUpAnimation());
    }

    private void HandlePlayerReachedNewHighScore(object sender, int newHighScroe)
    {
        HighScoreValue.text = newHighScroe.ToString();
        NewHighScoreWindow.SetActive(true);
    }
    private void HandleResetGame(object sender, EventArgs e)
    {
        NewHighScoreWindow.SetActive(false);

        GameOverWindowTransform.localScale = _initialGameOverWindowScale;
        GameOverWindowTransform.gameObject.SetActive(false);

        DisableButtons();
    }

    private void InitButtons()
    {
        TryAgainButton.onClick.AddListener(() => OnUITryAgainPressed?.Invoke(this, EventArgs.Empty));
        QuitButton.onClick.AddListener(() => OnUIQuitPressed?.Invoke(this, EventArgs.Empty));
    }
    private void EnableButtons()
    {
        TryAgainButton.interactable = true;
        QuitButton.interactable = true;
    }
    private void DisableButtons()
    {
        TryAgainButton.interactable = false;
        QuitButton.interactable = false;
    }

    private IEnumerator ScaleUpAnimation()
    {
        float elapsedTime = 0.0f;
        Vector3 scaleTarget = new Vector3(1.0f, 1.0f, 1.0f);

        while (elapsedTime < ScaleUpAnimationDuration)
        {
            GameOverWindowTransform.localScale = Vector3.Lerp(_initialGameOverWindowScale, scaleTarget, elapsedTime / ScaleUpAnimationDuration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        GameOverWindowTransform.localScale = scaleTarget;

        EnableButtons();
    }
}
