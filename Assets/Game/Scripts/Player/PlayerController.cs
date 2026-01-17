using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float turnSpeed = 10f;
    private Transform cameraTransform;
    private Rigidbody playerRigidBody;
    private Animator animator;
    private Vector3 inputVector;
    private bool hasInput;
    private ItemPickup currentItemNearby;
    void Awake()
    {
        cameraTransform = Camera.main.transform;
        playerRigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerRigidBody.freezeRotation = true;
        playerRigidBody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update()
    {
        HandleInput();
        HandleAnimations();
        HandlerInteraction();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();
    }

    private void HandlerInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentItemNearby != null)
        {
            Debug.Log($"Getting {currentItemNearby.itemData.ItemName}");
            currentItemNearby.Interact();
            currentItemNearby = null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       ItemPickup item = other.GetComponent<ItemPickup>();
        if (item != null)
        {
            currentItemNearby = item;
            Debug.Log($"Press F to pickup {item.itemData.ItemName}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ItemPickup item = other.GetComponent<ItemPickup>();
        if (item != null)
        {
            currentItemNearby = null;
            Debug.Log("Exited from item area");
        }

    }
    private void HandleInput()
    {
        float flatCamera = 0;
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = flatCamera;
        camRight.y = flatCamera;

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
            playerRigidBody.linearVelocity = new Vector3(targetVelocity.x, playerRigidBody.linearVelocity.y, targetVelocity.z);
        }
        else
        {
            // Freio imediato (No slip)
            playerRigidBody.linearVelocity = new Vector3(0f, playerRigidBody.linearVelocity.y, 0f);
        }
    }

    private void ApplyRotation()
    {
        if (hasInput)
        {
            // Smooth rotation based on postman direction
            Quaternion targetRotation = Quaternion.LookRotation(inputVector);
            Quaternion nextRotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);

            playerRigidBody.MoveRotation(nextRotation);
        }
    }

    private void HandleAnimations()
    {
        if (animator == null) return;

        // Send magnitude to Blend Tree (0 = Idle, 1 = Walk)
        animator.SetFloat("Speedf", inputVector.magnitude, 0.1f, Time.deltaTime);
    }
}
