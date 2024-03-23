using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RotateObjectScript : MonoBehaviour
{
    [SerializeField] private XRSimpleInteractable interactable;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;

    private XRBaseInteractor interactor;
    private Vector3 initialInteractorPosition;
    private Vector3 currentInteractorPosition;
    private bool isObjectSelected = false;

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
        }
    }

    private void RotateObject()
    {
        currentInteractorPosition = GetProjectedPoint(interactor.transform.position);

        // Calculate the angle between the initial and current position
        float angle = Vector3.SignedAngle(
            initialInteractorPosition - transform.position, 
            currentInteractorPosition - transform.position, 
            rotationAxis
        );

        // Apply the rotation
        transform.Rotate(rotationAxis, angle, Space.World);

        // Update the initial position for the next frame
        initialInteractorPosition = currentInteractorPosition;
    }

    private Vector3 GetProjectedPoint(Vector3 interactorPosition)
    {
        // Project the interactor position onto the rotation axis
        Vector3 direction = interactorPosition - transform.position;
        return Vector3.ProjectOnPlane(direction, rotationAxis) + transform.position;
    }
}
