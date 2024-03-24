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
    [SerializeField] private bool continuousInteractionUpdate = false; // Or true, based on your desired default behavior

    private XRBaseInteractor interactor;
    private Vector3 initialInteractorPosition;
    private Vector3 currentInteractorPosition;
    private bool isObjectSelected = false;
    private Quaternion initialRotation;
    private Vector3 initialRotationEulerAngles;

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(SelectEntered);
        interactable.selectExited.AddListener(SelectExited);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(SelectEntered);
        interactable.selectExited.RemoveListener(SelectExited);
    }

    private void Start()
    {
        // Store the initial rotation
        initialRotation = transform.rotation;
        initialRotationEulerAngles = initialRotation.eulerAngles;
    }

    private void SelectEntered(SelectEnterEventArgs args)
    {
        interactor = args.interactor;
        initialInteractorPosition = GetProjectedPoint(interactor.transform.position);
        isObjectSelected = true;
    }

    private void SelectExited(SelectExitEventArgs args)
    {
        isObjectSelected = false;
        interactor = null;
    }

    private void Update()
    {
        if (isObjectSelected && interactor != null)
        {
            RotateObject();
            MoveOtherObject(); // Call the method to move and rotate the other object
        }
    }

    private void RotateObject()
    {
        currentInteractorPosition = GetProjectedPoint(interactor.transform.position);

        // Calculate the relative position of the interactor before and after movement
        Vector3 initialRelativePosition = initialInteractorPosition - transform.position;
        Vector3 currentRelativePosition = currentInteractorPosition - transform.position;

        // Calculate the rotation needed to align the initial relative position with the current one, around the rotation axis
        Quaternion rotationNeeded = Quaternion.FromToRotation(initialRelativePosition, currentRelativePosition);

        // Apply this rotation around the global rotation axis to the object
        transform.rotation = initialRotation * Quaternion.AngleAxis(rotationNeeded.eulerAngles.magnitude, rotationAxis.normalized);

        // Update the initial rotation if continuous interaction is enabled
        if (continuousInteractionUpdate)
        {
            initialRotation = transform.rotation;
        }

        // Update the initial interactor position for the next frame
        initialInteractorPosition = currentInteractorPosition;
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
            Vector3 targetRotationEulerAngles = Vector3.Scale(otherObjectRotationLimit, rotationProgress); // Apply rotation limits proportionally
            Quaternion targetRotation = Quaternion.Euler(targetRotationEulerAngles);
            otherObject.transform.rotation = Quaternion.Lerp(otherObject.transform.rotation, targetRotation, Time.deltaTime);
        }
    }

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
