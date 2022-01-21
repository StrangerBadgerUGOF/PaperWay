using UnityEngine;
using UnityEngine.SceneManagement;

// Control type of the player
public enum ControlType
{
    YawControl,
    PitchControl,
}

public class PlayerController : MonoBehaviour
{
    #region Variables

    // Player speed limits 
    [SerializeField]
    float _playerMaxSpeed = 95;
    const float PLAYER_MIN_SPEED = 0;

    // Rise speed
    const float RISE_SPEED_DECREASE = 1f;
    // Dive speed
    const float DIVE_SPEED_INCREASE = 2f;

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

    // Is pitch inverted
    [SerializeField]
    bool _isPitchInverted;
    public bool IsPitchInverted
    {
        get { return _isPitchInverted; }
        set { _isPitchInverted = value; }
    }

    // Control type
    [SerializeField]
    private ControlType _playerControlType;
    public ControlType PlayerControlType
    {
        get { return _playerControlType; }
        set {  _playerControlType = value;  }
    }

    // Player input
    private Vector2 _playerInput;

    // Rotate
    private sbyte _rollInput;
    private sbyte _yawInput;

    #endregion

    #region Unity

    // Start is called before the first frame update
    void Start()
    {
        PlayerControlType = _playerControlType;
    }

    // Update is called once per frame
    void Update()
    {
        // Player input
        PlayerInput();
        // Check special keys
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reload scene
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Exit to menu
            SceneManager.LoadScene(MenuManager.MENU_SCENE);
        }
    }

    private void FixedUpdate()
    {
        UpdatePosition();
    }

    private void OnTriggerStay(Collider other)
    {
        _playerForwardSpeed += Mathf.Abs(_playerRigidbody.velocity.normalized.magnitude);
    }

    private void OnCollisionStay(Collision collision)
    {
        _playerForwardSpeed = _playerRigidbody.velocity.magnitude;
    }

    #endregion

    #region Methods

    // Gets player input
    void PlayerInput()
    {
        // Getting input
        _playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _playerInput.y *= _isPitchInverted ? -1 : 1;
        // Getting rotation input
        switch (_playerControlType)
        {
            // Yaw control
            case ControlType.YawControl:
                if (Input.GetKey(KeyCode.Q)) { _rollInput = 1; }
                else if (Input.GetKey(KeyCode.E)) { _rollInput = -1; }
                else { _rollInput = 0; }
                break;
            // Pitch control
            case ControlType.PitchControl:
                if (Input.GetKey(KeyCode.Q)) { _yawInput = -1; }
                else if (Input.GetKey(KeyCode.E)) { _yawInput = 1; }
                else { _yawInput = 0; }
                _rollInput = (sbyte)-_playerInput.x;
                break;
         }
    }

    // Updates position
    void UpdatePosition()
    {
        PlaneControl();
    }

    // Plane control depends on choosen type of control
    void PlaneControl()
    {
        // Calculating speed
        _playerForwardSpeed -= transform.forward.y *
            (transform.forward.y <= 0 ? DIVE_SPEED_INCREASE : RISE_SPEED_DECREASE)
            * _playerRigidbody.mass * Time.deltaTime;
        if (_playerForwardSpeed < PLAYER_MIN_SPEED) { _playerForwardSpeed = PLAYER_MIN_SPEED; }
        if (_playerForwardSpeed > _playerMaxSpeed) { _playerForwardSpeed = _playerMaxSpeed; }
        // Adding force
        _playerRigidbody.AddForce(transform.forward * _playerForwardSpeed);
        // Rotating player
        _playerRigidbody.AddTorque(_rollInput * transform.forward * _rollRotationSpeed, ForceMode.Force);
        _playerRigidbody.AddTorque(-_playerInput.y * transform.right * _pitchRotationSpeed, ForceMode.Force);
        switch(_playerControlType)
        {
            case ControlType.YawControl:
                _playerRigidbody.AddTorque(_playerInput.x * transform.up * _yawRotationSpeed, ForceMode.Force);
                break;
            case ControlType.PitchControl:
                _playerRigidbody.AddTorque(_yawInput * transform.up * _yawRotationSpeed, ForceMode.Force);
                break;
        }
        // Rotate player according to the camera
        transform.rotation = Quaternion.Lerp(transform.rotation,
               Camera.main.transform.rotation,
               Time.deltaTime);
    }

    #endregion
}