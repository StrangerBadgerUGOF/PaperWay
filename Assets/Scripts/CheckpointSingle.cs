using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Single checkpoint
public class CheckpointSingle : MonoBehaviour
{
    // Time, which will be added for passing this checkpoint
    [SerializeField]
    private int _addTime;

    // Interface, where waypoint will be displayed
    [SerializeField]
    private GameObject _interfaceForWaypoint;
    // Checkpoint waypoint prefab
    [SerializeField]
    private GameObject _checkpointWaypointPrefab;

    // Checkpoint waypoint 
    private CheckpointWaypoint _checkpointWaypoint;

    // Event, which signals passing of the checkpoint
    public event Action<GameObject, int> CheckpointWasPassedEvent;

    private void Start()
    {
        // Instantiate checkpoint
        _checkpointWaypoint = Instantiate(_checkpointWaypointPrefab, transform.position,
            Quaternion.identity).GetComponent<CheckpointWaypoint>();
        _checkpointWaypoint.gameObject.transform.SetParent(_interfaceForWaypoint.transform);
        _checkpointWaypoint.SetTargetTransform(transform);
    }

    private void OnEnable()
    {
        // Activate checkpoint
        if (_checkpointWaypoint != null)
        {
            _checkpointWaypoint.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        // Deactivate checkpoint
        if (_checkpointWaypoint != null)
        {
            _checkpointWaypoint.gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        // Set active state of object to false
        gameObject.SetActive(false);
        // Trigger event
        CheckpointWasPassedEvent?.Invoke(gameObject, _addTime);
    }
}
