using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public event EventHandler OnUIStartsNewGame;

    [Header("UI Managers")]
    public UIGameOverManager UIGameOverManager;

    public float StartGameAnimationDuration = 2.0f;
    public Button StartGameButton;

    [Header("Starship")]
    public Image Starship;
    public Vector3 StarshipTargetDestination;

    public TextMeshProUGUI StartGameText;
    public CanvasGroup BackgroundCanvasGroup;


    private Vector3 _starshipInitialPosition;
    private AudioSource _starshipAudioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _starshipInitialPosition = Starship.rectTransform.anchoredPosition;
        _starshipAudioSource = Starship.GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameManager.OnResetGame += HandleResetGame;
    }

    private void HandleResetGame(object sender, EventArgs e) => ResetUI();

    //Called from an invisible button in the UI.
    public void StartGame()
    {
        StartGameText.gameObject.SetActive(false);
        StartCoroutine(MoveStarship());
        StartCoroutine(FadeOutBackground());

        StartGameButton.gameObject.SetActive(false);
        OnUIStartsNewGame?.Invoke(this, EventArgs.Empty);
    }


    public void SubscribeToUIPlayAgainEvent(EventHandler handler) => UIGameOverManager.OnUITryAgainPressed += handler;
    public void SubscribeToUIQuitEvent(EventHandler handler) => UIGameOverManager.OnUIQuitPressed += handler;

    private void ResetUI()
    {
        StartGameText.gameObject.SetActive(true);
        BackgroundCanvasGroup.alpha = 1.0f;
        StartGameButton.gameObject.SetActive(true);

        Starship.gameObject.SetActive(true);
        Starship.rectTransform.anchoredPosition = _starshipInitialPosition;
    }

    #region Animations
    private IEnumerator MoveStarship()
    {
        _starshipAudioSource.Play();

        float elapsedTime = 0.0f;

        while (elapsedTime < StartGameAnimationDuration)
        {
            Starship.rectTransform.anchoredPosition = Vector3.Lerp(_starshipInitialPosition, StarshipTargetDestination, elapsedTime / StartGameAnimationDuration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Starship.rectTransform.position = StarshipTargetDestination;
        Starship.gameObject.SetActive(false);
    }
    private IEnumerator FadeOutBackground()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < StartGameAnimationDuration)
        {
            BackgroundCanvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / StartGameAnimationDuration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        BackgroundCanvasGroup.alpha = 0.0f;
    }
    #endregion
}
