using UnityEngine;

public class GravityControl : MonoBehaviour
{
    public float gravityStrength = 9.81f; // Gravity force strength (similar to Earth's gravity)
    public float fallSpeed = 0f; // The fall speed of the XR Rig
    public float terminalVelocity = -53f; // Maximum falling speed (like terminal velocity)
    public bool useGravity = true; // Toggle gravity on/off
    private CharacterController characterController;

    void Start()
    {
        // Get the CharacterController component on the XR Rig
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController is missing on the XR Rig!");
        }
    }

    void Update()
    {
        if (useGravity && characterController != null)
        {
            ApplyGravity();
        }
    }

    void ApplyGravity()
    {
        // Simulate gravity by applying downward force
        if (characterController.isGrounded)
        {
            fallSpeed = -gravityStrength * Time.deltaTime; // Reset fall speed when grounded
        }
        else
        {
            fallSpeed -= gravityStrength * Time.deltaTime; // Apply gravity when in the air
        }

        // Apply terminal velocity
        if (fallSpeed < terminalVelocity)
        {
            fallSpeed = terminalVelocity;
        }

        // Move the character using the gravity effect
        Vector3 moveDirection = new Vector3(0, fallSpeed, 0);
        characterController.Move(moveDirection);
    }

    public void ToggleGravity(bool enable)
    {
        useGravity = enable;
    }
}
