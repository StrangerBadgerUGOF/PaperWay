using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Waypoint to the next checkpoint
public class CheckpointWaypoint : MonoBehaviour
{
    // Waypoint image
    [SerializeField]
    private Image _waypointImage;
    // Waypoint target
    private Transform _targetWaypointTransform;

    // Sets new target for waypoint to point to
    public void SetTargetTransform(Transform targetWaypoint) { _targetWaypointTransform = targetWaypoint; }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf) { return; }
        // Show image at the edge of the screen
        float minX = _waypointImage.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;
        float minY = _waypointImage.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;
        // Get waypoint position according to the target
        Vector2 waypointPosition = Camera.main.WorldToScreenPoint(_targetWaypointTransform.position);
        // Check, if waypoint is behind the camera
        if (Vector3.Dot(_targetWaypointTransform.position - Camera.main.transform.position,
            Camera.main.transform.forward) < 0) 
        {
            // Target is behind the camera
            waypointPosition.x = waypointPosition.x < Screen.width / 2 ? maxX : minX;
        }
        // Calculate position
        waypointPosition.x = Mathf.Clamp(waypointPosition.x, minX, maxX);
        waypointPosition.y = Mathf.Clamp(waypointPosition.y, minY, maxY);
        // Update position
        _waypointImage.transform.position = waypointPosition;
    }
}
