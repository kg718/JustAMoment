using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 moveDir;
    private Vector2 camMoveDir;
    //private Rigidbody rb;

    [SerializeField] private float camSensitivity;
    [SerializeField] private float camSensRas =1f;
    [SerializeField] private Transform orientation;
    [SerializeField] float minPitch =-89f, MaxPitch = 89f;
    private InputAction _inputAction;
    private PlayerInput _playerInput;
    float xRot;
    float yRot;

    private void Awake()
    {
#if ENABLE_INPUT_SYSTEM
        _playerInput = GetComponent<PlayerInput>();
        _inputAction =_playerInput.actions["CameraMove"];
#else
        Debug.LogError("Input System is not enabled for the player")
#endif
    }
    void Start()
    {
    }

    void Update()
    { 
    //    float _rotX = camMoveDir.x * Time.deltaTime * camSensitivity;
    //    float _rotY = camMoveDir.y * Time.deltaTime * camSensitivity;

    //    yRot += _rotX;
    //    xRot -= _rotY;
    //    xRot = Mathf.Clamp(xRot, -90f, 90f);

    //    transform.rotation = Quaternion.Euler(xRot, yRot, 0);
    //    orientation.rotation = Quaternion.Euler(0, yRot, 0);
    }
    private void LateUpdate()
    {
        CameraRotation();
    }
    private void CameraRotation()
    {
        Vector2 input = _inputAction.ReadValue<Vector2>();
        if (input.sqrMagnitude < 1e-4f) return;

        yRot += input.x * camSensRas;
        xRot  -= input.y * camSensRas;
        xRot = Mathf.Clamp(xRot, minPitch, MaxPitch);


        transform.rotation = Quaternion.Euler(xRot, yRot, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRot, 0f);

    }
    public void OnMove(InputValue _value)
    {
        moveDir = _value.Get<Vector2>();
    }

    public void OnCameraMove(InputValue _value)
    {
        camMoveDir = _value.Get<Vector2>();
    }
}
