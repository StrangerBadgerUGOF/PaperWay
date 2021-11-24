using UnityEngine;

public class BoosterObject : MonoBehaviour
{
    // Push force
    [SerializeField]
    private float _pushForce;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.attachedRigidbody.AddForce
                (transform.up * _pushForce, ForceMode.Force);
            // Rotate object according to the up force
            Quaternion lookUp = Quaternion.LookRotation(transform.up);
            other.transform.rotation = Quaternion.Slerp(other.transform.rotation, lookUp, Time.deltaTime);
        }
    }
}
