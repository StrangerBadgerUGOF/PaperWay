using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Default name for player
    private const string DEFAULT_NAME = "PLAYER"; 

    // Game over state
    private bool _gameOver;

    // Game over text
    [SerializeField]
    private GameObject _gameOverInterface;
    // Player name text
    [SerializeField]
    private Text _playerNameText;

    // Player
    [SerializeField]
    PlayerController _playerController;
    // Checkpoint manager
    [SerializeField]
    CheckpointManager _checkpointManager;
    // Timer controller
    [SerializeField]
    TimerController _timerController;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to events
        _playerController.PlayerHasCrashed += GameOver;
        _timerController.TimeRunnedOutEvent += GameOver;
        // Game over state
        _gameOver = false;
        // Set player control pitch inversion
        _playerController.IsPitchInverted = GameSettings.IsPitchInverted;
        // Set player control type
        _playerController.PlayerControlType = GameSettings.ControlType;
    }

    // Game over
    private void GameOver()
    {
        if (_gameOver) { return; }
        // Stop timer
        _timerController.IsCounting = false;
        // Show game over screen
        _gameOverInterface.SetActive(true);
        // Set game over state to true
        _gameOver = true;
    }

    // Saves recent player data
    private void SavePlayerData()
    {
        if (string.IsNullOrWhiteSpace(_playerNameText.text) || string.IsNullOrEmpty(_playerNameText.text))
        {
            _playerNameText.text = DEFAULT_NAME;
        }
        // TEST
        Debug.Log(_playerNameText.text);
        // SAVE DATA
    }

    // Reloads scene and saves player data
    public void RetryGame()
    {
        SavePlayerData();
        // Reload scene
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    // Submits player and exits to the menu
    public void BackToMenu()
    {
        SavePlayerData();
        // Exit to menu
        SceneManager.LoadScene(MenuManager.MENU_SCENE);
    }
}
