using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform orientation;

    Vector2 inputDir; 
    Vector3 movDir;
    private Rigidbody rb;
    private bool grounded = false;
    [SerializeField] private float groundDetectionDistance;
    [SerializeField] private float fallSpeed;
    private Animator playerAnimator;

    private string currentAnimationTrigger = "";

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerAnimator = GetComponent<Animator>();
    }


    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, groundDetectionDistance);
        MovePlayer();
        if(!grounded)
        {
            rb.AddForce(Vector3.down.normalized * fallSpeed, ForceMode.Force);
        }
    }

    public void OnMove(InputValue _value)
    {
        inputDir = _value.Get<Vector2>();
    }

    private void MovePlayer()
    {
        movDir = orientation.forward * inputDir.y + orientation.right * inputDir.x;
        rb.AddForce(movDir.normalized * moveSpeed, ForceMode.Force);
        if(movDir.magnitude <= 0.2f)
        {
            SetAnimation("StopWalking");
        }
        else
        {
            SetAnimation("StartWalking");
        }
    }

    private void SetAnimation(string _animation)
    {
        if (_animation == currentAnimationTrigger) return;
        playerAnimator.SetTrigger(_animation);
    }
}
