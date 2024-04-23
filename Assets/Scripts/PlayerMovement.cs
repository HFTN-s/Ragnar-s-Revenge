using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private InputDevice Head;
    private InputDevice leftHandController;
    private InputDevice rightHandController;
    [SerializeField] private Camera camera;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float rotationAmount = 2.0f;
    private Vector2 turnInput;
    public bool canMove = false;

    void Start()
    {
        SetupHead();
        SetupLeftHandController();
        SetupRightHandController();
        // if current scene is main menu scene, player is not allowed to move
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }

    void SetupHead()
    {
        Head = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        if (!Head.isValid)
        {
            // Debug.Log("Head device not found. Retrying...");
            Invoke("SetupHead", 0.7f); // Retry after a delay
        }
        else
        {
            Debug.Log("Head device found");
        }
    }

    void SetupLeftHandController()
    {
        leftHandController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (!leftHandController.isValid)
        {
            // Debug.Log("Left hand controller not found. Retrying...");
            Invoke("SetupLeftHandController", 0.7f); // Retry after a delay
        }
        else
        {
            Debug.Log("Left hand controller found");
        }
    }
    void SetupRightHandController()
    {
        rightHandController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (!rightHandController.isValid)
        {
            //  Debug.Log("Right hand controller not found. Retrying...");
            Invoke("SetupRightHandController", 0.7f); // Retry after a delay
        }
        else
        {
            Debug.Log("Right hand controller found");
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        Rigidbody rb = GetComponent<Rigidbody>(); // Get the Rigidbody component

        // if player presses left stick forward or backward, move player along the camera's forward vector
        if (leftHandController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue))
        {
            // Determine the direction and magnitude of the movement
            Vector3 moveDirection = camera.transform.forward * primary2DAxisValue.y * speed * Time.fixedDeltaTime;
            // Ensure we are not moving vertically if the Y axis is locked
            moveDirection.y = 0;

            // Apply the movement to the Rigidbody
            rb.MovePosition(rb.position + moveDirection);
        }

        // Rotate player
        RotatePlayer(rb);
    }

    void RotatePlayer(Rigidbody rb)
    {
        if (rightHandController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue))
        {
            // The turn value is the input value scaled by rotation amount.
            float turn = primary2DAxisValue.x * rotationAmount;

            // Calculate the new rotation by applying the turn to the current rotation around the Y axis.
            Quaternion newRotation = Quaternion.Euler(0f, turn, 0f) * rb.rotation;

            // Smoothly rotate the player by interpolating to the new rotation.
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, newRotation, Time.fixedDeltaTime * 10));
        }
    }

}
