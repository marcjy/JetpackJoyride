using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public event EventHandler OnThrusting;
    public event EventHandler OnStopThrusting;

    private bool _isPlaying = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.OnGameStart += OnPlayerStartedGame;
    }

    private void OnPlayerStartedGame(object sender, EventArgs e) => _isPlaying = true;


    public void Thrust(CallbackContext callbackContext)
    {
        if (!_isPlaying)
            return;

        if (callbackContext.phase == UnityEngine.InputSystem.InputActionPhase.Performed)
            OnThrusting?.Invoke(this, EventArgs.Empty);
        if (callbackContext.phase == UnityEngine.InputSystem.InputActionPhase.Canceled)
            OnStopThrusting?.Invoke(this, EventArgs.Empty);
    }
}
