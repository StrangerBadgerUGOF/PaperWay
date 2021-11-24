using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Camera bias
    [SerializeField][Range(0, 1.0f)]
    private float _cameraBias;
    // Camera look offset
    [SerializeField]
    private float _cameraLookOffset;
    // Camera offset
    [SerializeField]
    private float _cameraOffset;
    // Observable object
    [SerializeField]
    private Transform _observableObject;

    private void Start()
    {
        // ...
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Getting camera position
        Vector3 cameraPos = _observableObject.position - _observableObject.forward * _cameraOffset 
            + Vector3.up * _cameraOffset / 2;
        // Setting new camera position
        transform.position = transform.position * _cameraBias + cameraPos * (1.0f - _cameraBias);
        // Rotating camera to look point
        transform.LookAt(_observableObject.position + _observableObject.forward * _cameraLookOffset);
    }
}
