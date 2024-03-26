using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RotateObjectScript : MonoBehaviour
{
    [SerializeField] private XRSimpleInteractable interactable;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private Vector3 minLimit = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 maxLimit = new Vector3(360, 360, 360); // Example max limit
    [SerializeField] private GameObject otherObject;
    [SerializeField] private Vector3 otherObjectPosition;
    [SerializeField] private Vector3 otherObjectDestination;
    [SerializeField] private Vector3 otherObjectRotationAxis = Vector3.up;
    [SerializeField] private Vector3 otherObjectRotationLimit = new Vector3(0, 0, 0);
    [SerializeField] private bool isOtherObjectMoveable = false;
    [SerializeField] private bool isOtherObjectRotatable = false;
    private Vector3 lastGrabPosition;
    public float grabMoveThreshold = 0.1f; // Adjust this value based on testing
    private float controllerVerticalInput;
    private float controllerHorizontalInput;

    public bool upAndDown = false;
    public float rotationSpeed = 1f;


    private XRBaseInteractor interactor;
    private Vector3 initialInteractorPosition;
    private Vector3 currentInteractorPosition;
    private bool isObjectSelected = false;
    private Quaternion initialRotation;
    private Vector3 initialRotationEulers;

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(SelectEntered);
        interactable.selectExited.AddListener(SelectExited);
        interactable.hoverExited.AddListener(OnHoverExited);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(SelectEntered);
        interactable.selectExited.RemoveListener(SelectExited);
        interactable.hoverExited.RemoveListener(OnHoverExited);
    }

    private void Start()
    {
        // Store the initial rotation
        initialRotation = transform.rotation;
        initialRotationEulers = initialRotation.eulerAngles;
    }

    private void SelectEntered(SelectEnterEventArgs args)
    {
        interactor = args.interactor;
        initialInteractorPosition = GetProjectedPoint(interactor.transform.position);
        isObjectSelected = true;
        Debug.Log("Object grabbed");
        lastGrabPosition = interactor.transform.position; // Store the position at the moment of grabbing
        initialInteractorPosition = interactor.transform.position; // Store the initial position for comparison
        initialRotation = transform.rotation; // Store the initial rotation for comparison
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        Debug.Log("Hover exited");
        isObjectSelected = false;
        interactor = null;
     }

    private void SelectExited(SelectExitEventArgs args)
    {
        if (interactor = null) return;
        isObjectSelected = false;
        Debug.Log("Object released");
        // Optionally update lastGrabPosition on release if you want to reset movement calculations upon next grab
        // This depends on your intended behavior when re-grabbing after moving the controller without grabbing
        lastGrabPosition = interactor.transform.position;
        // You may also consider resetting other related variables here if it fits your interaction model
        interactor = null;
    }

    // Add this at the class level
    public float sensitivityThreshold = 0.1f; // Adjust this value as needed

    private void Update()
    {
        if (isObjectSelected && interactor != null)
        {
            if (IsControllerMovedSinceLastGrab())
            {
                float inputDelta;
                if (upAndDown)
                {
                    inputDelta = interactor.transform.position.y - initialInteractorPosition.y;
                    controllerVerticalInput = inputDelta;
                }
                else
                {
                    inputDelta = interactor.transform.position.x - initialInteractorPosition.x;
                    controllerHorizontalInput = inputDelta;
                }

                if (Mathf.Abs(inputDelta) > sensitivityThreshold)
                {
                    RotateObject(inputDelta);
                }

                MoveOtherObject(); // Call the method to move and rotate the other object
            }
        }
    }

    private bool IsControllerMovedSinceLastGrab()
    {
        // Calculate the current distance from the last grab position
        float distanceMoved = Vector3.Distance(interactor.transform.position, lastGrabPosition);

        // Check if the movement exceeds the threshold
        if (distanceMoved > grabMoveThreshold)
        {
            // Update the last grab position to the current position for future checks
            lastGrabPosition = interactor.transform.position;
            return true;
        }

        return false;
    }

    private void RotateObject(float inputDelta)
    {
        // Directly use inputDelta to calculate rotationAmount, which now correctly reflects direction
        float rotationAmount = inputDelta * rotationSpeed;

        // Choose the rotation axis based on the movement direction
        Vector3 rotationAxis = Vector3.up; // Assuming Y-axis for left/right and changing to Vector3.forward if up/down

        // Apply rotation direction based on inputDelta's sign
        Quaternion rotationNeeded = Quaternion.AngleAxis(rotationAmount, rotationAxis.normalized);
        transform.rotation = initialRotation * rotationNeeded;

        // Conditional update to initialRotation can be here if you want the rotation to accumulate
        initialRotation = transform.rotation;

        // Update initialInteractorPosition to reflect the current frame's position for next frame comparison
        initialInteractorPosition = interactor.transform.position;
    }


    private Vector3 GetProjectedPoint(Vector3 interactorPosition)
    {
        Vector3 direction = interactorPosition - transform.position;
        return Vector3.ProjectOnPlane(direction, rotationAxis) + transform.position;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        angle = NormalizeAngle(angle);
        min = NormalizeAngle(min);
        max = NormalizeAngle(max);
        if (min < max) return Mathf.Clamp(angle, min, max);
        return angle < max ? angle : Mathf.Clamp(angle, min, 360);
    }

    private float NormalizeAngle(float angle)
    {
        while (angle > 360) angle -= 360;
        while (angle < 0) angle += 360;
        return angle;
    }

    private void MoveOtherObject()
    {
        if (otherObject == null) return;

        // Calculate rotation progress for each axis
        Vector3 rotationProgress = new Vector3(
            CalculateRotationProgress(transform.localEulerAngles.x, minLimit.x, maxLimit.x),
            CalculateRotationProgress(transform.localEulerAngles.y, minLimit.y, maxLimit.y),
            CalculateRotationProgress(transform.localEulerAngles.z, minLimit.z, maxLimit.z)
        );

        // If the object is movable, adjust its position based on the rotation progress
        if (isOtherObjectMoveable)
        {
            Vector3 targetPosition = Vector3.Lerp(otherObjectPosition, otherObjectDestination, rotationProgress.magnitude / 3f); // Assuming uniform importance for all axes
            otherObject.transform.position = Vector3.Lerp(otherObject.transform.position, targetPosition, Time.deltaTime);
        }

        // If the object is rotatable, adjust its rotation based on the rotation progress
        if (isOtherObjectRotatable)
        {
            Vector3 targetRotationEulers = Vector3.Scale(otherObjectRotationLimit, rotationProgress); // Apply rotation limits proportionally
            Quaternion targetRotation = Quaternion.Euler(targetRotationEulers);
            otherObject.transform.rotation = Quaternion.Lerp(otherObject.transform.rotation, targetRotation, Time.deltaTime);
        }
    }

    // Helper method to calculate rotation progress
    private float CalculateRotationProgress(float currentAngle, float minAngle, float maxAngle)
    {
        currentAngle = NormalizeAngle(currentAngle);
        minAngle = NormalizeAngle(minAngle);
        maxAngle = NormalizeAngle(maxAngle);

        if (maxAngle < minAngle) maxAngle += 360; // Ensure the max is greater than min
        currentAngle = currentAngle < minAngle ? currentAngle + 360 : currentAngle; // Adjust current angle if needed

        return (currentAngle - minAngle) / (maxAngle - minAngle);
    }

}