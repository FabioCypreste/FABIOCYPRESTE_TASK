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
    private QuestGiver currentNPCNearby;
    void Awake()
    {
        cameraTransform = Camera.main.transform;
        playerRigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Constrain rotation to keep the player upright
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
        if (Input.GetKeyDown(KeyCode.F)) {
            if (currentItemNearby != null)
            {
                NotificationUI.Instance.ShowMessage($"Getting {currentItemNearby.itemData.ItemName}");
                currentItemNearby.Interact();
                currentItemNearby = null;
                return;
            }
            if (currentNPCNearby != null)
            {
                currentNPCNearby.Interact();
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            currentItemNearby = other.GetComponent<ItemPickup>();
            if (NotificationUI.Instance != null)
            {
                NotificationUI.Instance.ShowMessage("Press F to collect");
            }
        }

        if (other.CompareTag("NPC"))
        {
            currentNPCNearby = other.GetComponent<QuestGiver>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            currentItemNearby = null;
        }

        if (other.CompareTag("NPC"))
        {
            currentNPCNearby = null;
        }

    }
    private void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        float flatCamera = 0;
        camForward.y = flatCamera;
        camRight.y = flatCamera;
        camForward.Normalize();
        camRight.Normalize();

        // Calculate move direction based on camera orientation
        inputVector = ((camForward * verticalInput) + (camRight * horizontalInput)).normalized; //Prevent him to be faster in diagonal
        hasInput = inputVector.magnitude > 0.1f;
    }

    private void ApplyMovement()
    {
        if (!hasInput)
        {
            // Reset horizontal velocity when no input is detected
            playerRigidBody.linearVelocity = new Vector3(0f, playerRigidBody.linearVelocity.y, 0f);
        }
        Vector3 targetVelocity = inputVector * movementSpeed;
        playerRigidBody.linearVelocity = new Vector3(targetVelocity.x, playerRigidBody.linearVelocity.y, targetVelocity.z);
    }

    private void ApplyRotation()
    {
        if (hasInput)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputVector);
            Quaternion nextRotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);

            playerRigidBody.MoveRotation(nextRotation);
        }
    }

    private void HandleAnimations()
    {
        if (animator == null) return;

        // Send magnitude to Blend Tree (0 = Idle, 1 = Running)
        animator.SetFloat("Speedf", inputVector.magnitude, 0.1f, Time.deltaTime);
    }
}
