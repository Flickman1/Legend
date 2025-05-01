using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player character
    public float distance = 5.0f; // Distance from the player
    public float height = 2.0f; // Height above the player
    public float damping = 5.0f; // How smoothly the camera follows
    public float rotationSpeed = 100.0f; // Camera rotation speed
    public float verticalAngle = 30f; // The vertical angle for the camera's up/down rotation
    public float maxVerticalAngle = 60f; // Maximum vertical angle
    public float minVerticalAngle = -30f; // Minimum vertical angle
    private float verticalRotation = 0f;

    private void LateUpdate()
    {
        // Calculate the desired position
        Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);

        // Move the camera to the desired position
        transform.position = smoothPosition;

        // Rotate the camera based on player input (mouse or joystick)
        float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float verticalInput = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // Apply vertical rotation limits
        verticalRotation = Mathf.Clamp(verticalRotation + verticalInput, minVerticalAngle, maxVerticalAngle);

        // Rotate the camera horizontally
        transform.RotateAround(target.position, Vector3.up, horizontalInput);

        // Rotate the camera vertically (only on the X-axis)
        transform.localRotation = Quaternion.Euler(verticalRotation, transform.localRotation.eulerAngles.y, 0);

        // Keep the camera facing the player
        transform.LookAt(target);
    }
}
