using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the character moves
    public float turnSpeed = 700f; // Speed at which the character turns
    
    private CharacterController characterController;
    private Camera mainCamera;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
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

        // Move the character
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Rotate the character based on input
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }
}