using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Variables

    // Player speed limits 
    [SerializeField]
    float _playerMaxSpeed = 95;
    const float PLAYER_MIN_SPEED = 0;

    // Rise speed
    const float RISE_SPEED = 1.5f;
    // Dive speed
    const float DIVE_SPEED = 1f;

    // Player rigidbody
    [SerializeField]
    private Rigidbody _playerRigidbody;

    // Player movement speed
    [SerializeField]
    private float _playerForwardSpeed;
    // Player pitch rotation speed
    [SerializeField]
    private float _pitchRotationSpeed;
    // Player pitch rotation speed
    [SerializeField]
    private float _yawRotationSpeed;
    // Player roll rotation speed
    [SerializeField]
    private float _rollRotationSpeed;

    // Player input
    private Vector2 _playerInput;

    private bool _rotateUnlocking;
    private bool _rotateUnlock;
    private sbyte _rollInput;

    #endregion

    #region Unity

    // Start is called before the first frame update
    void Start()
    {
        _rotateUnlock = true;
        _rotateUnlocking = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    private void FixedUpdate()
    {
        UpdatePosition();
    }

    private void OnCollisionStay(Collision collision)
    {
        _playerForwardSpeed = _playerRigidbody.velocity.magnitude;
    }

    private void OnTriggerStay(Collider other)
    {
        _playerForwardSpeed +=  _playerRigidbody.mass;
    }

    #endregion

    #region Methods

    // Gets player input
    void PlayerInput()
    {
        // Getting input
        _playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        // Getting rotation input
        if (Input.GetKey(KeyCode.Q)) {  _rollInput = 1;  }
        else if (Input.GetKey(KeyCode.E)) { _rollInput = -1; }
        else 
        { 
            _rollInput = 0;
            if (_rotateUnlock == false &&  _rotateUnlocking == false)
            {
                CancelInvoke();
                _rotateUnlocking = true;
                Invoke(nameof(RotateUnlock), 5.0f);
            }
        }
        if (_rollInput != 0) { _rotateUnlock = false; }
    }

    // Unblocks rotation
    void RotateUnlock()
    {
        _rotateUnlock = true;
        _rotateUnlocking = false;
    }

    // Updates position
    void UpdatePosition()
    {
        // Calculating speed
        _playerForwardSpeed -= transform.forward.y * (transform.forward.y <= 0 ? DIVE_SPEED : RISE_SPEED) * _playerRigidbody.mass * Time.deltaTime;
        if (_playerForwardSpeed < PLAYER_MIN_SPEED) { _playerForwardSpeed = PLAYER_MIN_SPEED; }
        if (_playerForwardSpeed > _playerMaxSpeed) { _playerForwardSpeed = _playerMaxSpeed; }
        // Adding force
        _playerRigidbody.AddForce(transform.forward * _playerForwardSpeed);
        // Rotating player
        _playerRigidbody.AddTorque(-_playerInput.y * transform.right * _pitchRotationSpeed, ForceMode.Force);
        _playerRigidbody.AddTorque(_playerInput.x * transform.up * _yawRotationSpeed, ForceMode.Force);
        _playerRigidbody.AddTorque(_rollInput * transform.forward * _rollRotationSpeed, ForceMode.Force);
        // Rotate player according to the camera
        if (_rotateUnlock)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Camera.main.transform.rotation, Time.deltaTime);
        }
    }

    #endregion
}