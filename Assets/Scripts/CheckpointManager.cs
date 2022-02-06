using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointManager : MonoBehaviour
{
    // How many checkpoints must be passed to increase checkpoint limit
    [SerializeField]
    private int _checkpointsPassedForLimitIncrease = 1;
    // Check point limir counter
    private int _checkpointLimitCounter;
    // Limit of checkpoints, which can be active simultaneously
    private int _checkpointLimit;
    // Passed checkpoints counter
    [SerializeField]
    private Text _allPassedCheckpointsCountText;
    // Counter of passed checkpoint
    private int _allPassedCheckpointsCount;

    // List of checkpoints
    [SerializeField]
    private List<CheckpointSingle> _checkpointSinglesList;

    // Timer controller
    [SerializeField]
    private TimerController _timerController;
    // Total time, which will be added to the timer
    private int _totalTimeIncrease;

    // Start is called before the first frame update
    private void Awake()
    {
        // Initialize variables
        _checkpointLimit = 1;
        _checkpointLimitCounter = _allPassedCheckpointsCount = 0;
        // Restart checkpoints
        RestartCheckpoints();
    }

    private void RestartCheckpoints()
    {
        _totalTimeIncrease = 0;
        // Subscribe to all checkpoints
        for (int i = 0; i < _checkpointSinglesList.Count; i++)
        {
            _checkpointSinglesList[i].CheckpointWasPassedEvent += CheckpointPassed;
            _checkpointSinglesList[i].gameObject.SetActive(false);
        }
        // Generate random waypoint
        int randWaypointIndex = Random.Range(0, _checkpointSinglesList.Count);
        // Set one of the waypoints active
        _checkpointSinglesList[randWaypointIndex].gameObject.SetActive(true);
    }

    // Checks, if all checkpoints were passed
    private bool AreCheckpointsPassed()
    {
        for (int i = 0; i < _checkpointSinglesList.Count; i++)
        {
            // Finding corresponding checkpoint
            if (_checkpointSinglesList[i].gameObject.activeSelf){ return false; }
        }
        return true;
    }

    // Checkpoint passed event process
    private void CheckpointPassed(GameObject _checkpointSingleGameObject, int addTime)
    {
        // Increase limit of checkpoints, if limit counter reached limit
        if (_checkpointLimitCounter >= _checkpointsPassedForLimitIncrease)
        {
            _checkpointLimitCounter = 0;
            _checkpointLimit++;
        }
        else
        {
            _checkpointLimitCounter++;
        }
        // Increase total add time
        _totalTimeIncrease += addTime;
        // Increase counter of passed checkpoints
        _allPassedCheckpointsCount++;
        // Update passed checkpoints counter
        _allPassedCheckpointsCountText.text = _allPassedCheckpointsCount.ToString();
        // If not all checkpoints were passed, break the function
        if (!AreCheckpointsPassed()) { return; }
        // Finding index of the checkpoint
        int indexToIgnore = 0;
        for (int i = 0; i < _checkpointSinglesList.Count; i++)
        {
            // Finding corresponding checkpoint
            if (_checkpointSinglesList[i].gameObject == _checkpointSingleGameObject)
            { 
                indexToIgnore = i;
                break;
            }
        }
        // Form list of checkpoints and activate them
        List<GameObject> activeCheckpoints = new List<GameObject>();
        for (int i = 0; i < _checkpointSinglesList.Count; i++)
        {
            activeCheckpoints.Add(_checkpointSinglesList[i].gameObject);
            activeCheckpoints[i].SetActive(true);
        }
        activeCheckpoints[indexToIgnore].SetActive(false);
        activeCheckpoints.RemoveAt(indexToIgnore);
        // Deactivate checkpoints
        while (activeCheckpoints.Count > _checkpointLimit) 
        {
            // Generate index
            int index = Random.Range(0, activeCheckpoints.Count);
            // Set this checkpoint active
            activeCheckpoints[index].gameObject.SetActive(false);
            // Remove checkpoint from list
            activeCheckpoints.RemoveAt(index);
        } 
        // Update timer
        _timerController.AddTime(_totalTimeIncrease);
        // Set total time increase to zero
        _totalTimeIncrease = 0;
    }
}
