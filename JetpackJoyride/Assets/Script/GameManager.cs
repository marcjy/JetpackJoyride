using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ObstacleGenerator ObstacleGenerator;
    public BackgroundManager BackgroundManager;

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

        StartGame();
    }


}
