using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        // Set player control pitch inversion
        _playerController.IsPitchInverted = GameSettings.IsPitchInverted;
        // Set player control type
        _playerController.PlayerControlType = GameSettings.ControlType;
    }
}
