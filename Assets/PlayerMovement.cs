using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private InputDevice rightHandController;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float rotationAmount = 15.0f;
    private Vector2 turnInput;
    [SerializeField] private bool canMove = true;

    void Start()
    {
        SetupLeftHandController();
        SetupRightHandController();
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }

    void SetupLeftHandController()
    {
        rightHandController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (!rightHandController.isValid)
        {
            Debug.Log("Left hand controller not found. Retrying...");
            Invoke("SetupLeftHandController", 0.5f); // Retry after a delay
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
            Debug.Log("Right hand controller not found. Retrying...");
            Invoke("SetupRightHandController", 0.5f); // Retry after a delay
        }
        else
        {
            Debug.Log("Right hand controller found");
        }
    }

    void FixedUpdate()
    {
        if (!rightHandController.isValid) return; // Exit if controller is not valid
        if (!canMove) return; // Exit if player is not allowed to move
            if (rightHandController.TryGetFeatureValue(CommonUsages.primaryButton, out bool isAPressed) && isAPressed)
            {
                Debug.Log("Moving Forward");
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }

            if (rightHandController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystickValue))
            {
                RotatePlayer(joystickValue);
            }
    }   

    void RotatePlayer(Vector2 joystickValue)
    {
        if (joystickValue.x > 0.5f)
        {
            transform.Rotate(Vector3.up, rotationAmount);
        }
        else if (joystickValue.x < -0.5f)
        {
            transform.Rotate(Vector3.up, -rotationAmount);
        }
    }
}
