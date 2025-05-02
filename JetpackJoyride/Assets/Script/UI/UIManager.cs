using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static event EventHandler OnGameStart;

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
        _starshipInitialPosition = Starship.rectTransform.anchoredPosition;
        _starshipAudioSource = Starship.GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        StartGameText.gameObject.SetActive(false);
        StartCoroutine(MoveStarship());
        StartCoroutine(FadeOutBackground());

        StartGameButton.gameObject.SetActive(false);
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }

    public void ResetUI()
    {
        StartGameText.gameObject.SetActive(true);
        BackgroundCanvasGroup.alpha = 1.0f;
        StartGameButton.gameObject.SetActive(true);

        Starship.gameObject.SetActive(true);
        Starship.rectTransform.anchoredPosition = _starshipInitialPosition;
    }

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
}
