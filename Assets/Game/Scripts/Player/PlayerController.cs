using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float turnSpeed = 10f;
    private Transform cameraTransform;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 inputVector;
    private bool hasInput;
    void Awake()
    {
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true; //Guarantees that rb don't will fall when the postman moves
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update()
    {
        HandleInput();
        HandleAnimations();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"), verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 camForward = cameraTransform.forward, camRight = cameraTransform.right;
        camForward.y = 0; camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        inputVector = (camForward * verticalInput + camRight * horizontalInput).normalized;
        hasInput = inputVector.magnitude > 0.1f;
    }

    private void ApplyMovement()
    {
        if (hasInput)
        {
            Vector3 targetVelocity = inputVector * movementSpeed;
            rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
        }
        else
        {
            // Freio imediato (No slip)
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }

    private void ApplyRotation()
    {
        if (hasInput)
        {
            // Smooth rotation based on postman direction
            Quaternion targetRotation = Quaternion.LookRotation(inputVector);
            Quaternion nextRotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);

            rb.MoveRotation(nextRotation);
        }
    }

    private void HandleAnimations()
    {
        if (animator == null) return;

        // Send magnitude to Blend Tree (0 = Idle, 1 = Walk)
        animator.SetFloat("Speedf", inputVector.magnitude, 0.1f, Time.deltaTime);
    }
}
