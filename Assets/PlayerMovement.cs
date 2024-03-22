using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    private InputDevice rightHandController;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float rotationAmount = 15.0f;
    private Vector2 turnInput;

    void Start()
    {
        SetupRightHandController();
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
