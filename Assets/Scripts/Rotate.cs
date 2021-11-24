using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.forward * _rotateSpeed * Time.deltaTime);
    }
}
