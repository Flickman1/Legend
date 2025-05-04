using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the character moves
    public float turnSpeed = 700f; // Speed at which the character turns
    public float jumpForce = 5f; // Force applied when the player jumps

    private Rigidbody rb;
    private Camera mainCamera;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main; // Reference to the main camera
    }

    void Update()
    {
        // Get the input for movement
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float vertical = Input.GetAxis("Vertical"); // W/S or Up/Down Arrow

        // Calculate movement direction relative to the camera's rotation
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0f; // Keep the forward vector parallel to the ground
        right.y = 0f;   // Same for right vector

        forward.Normalize();
        right.Normalize();

        // Calculate the movement vector
        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;

        // Apply movement to the Rigidbody
        MoveCharacter(moveDirection);

        // Rotate the character based on input
        if (moveDirection != Vector3.zero)
        {
            RotateCharacter(moveDirection);
        }

        // Jump logic (only allow jumping if on the ground)
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void MoveCharacter(Vector3 moveDirection)
    {
        // Apply movement force based on input
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.linearVelocity.y; // Maintain current vertical velocity (gravity and jumping)
        rb.linearVelocity = velocity; // Apply the velocity to the Rigidbody
    }

    private void RotateCharacter(Vector3 moveDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply an instant upward force to simulate jumping
    }

    // Ground detection to prevent double-jumping
    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
