using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 moveDir;
    private Vector2 camMoveDir;
    //private Rigidbody rb;

    [SerializeField] private float camSensitivity;
    [SerializeField] private Transform orientation;

    float xRot;
    float yRot;

    void Start()
    {
    }

    void Update()
    {
        float _rotX = camMoveDir.x * Time.deltaTime * camSensitivity;
        float _rotY = camMoveDir.y * Time.deltaTime * camSensitivity;

        yRot += _rotX;
        xRot -= _rotY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);
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
