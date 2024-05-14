using UnityEngine;
using UnityEngine.Events;

public class LightBeam : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int maxReflectionCount = 5;
    public float maxStepDistance = 200;
    public string targetObjectName;
    public UnityEvent OnHitByLaserCorrect;

    void Start()
    {
        // Ensure the LineRenderer uses world space to align correctly in the scene
        lineRenderer.useWorldSpace = true;
    }

    void Update()
    {
        DrawLaser(transform.position, transform.forward, maxReflectionCount);
    }

    void DrawLaser(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, position);

        Vector3 currentStartPoint = position;
        Vector3 currentDirection = direction;

        int layerMask = ~0;  // Include all layers the ray should interact with

        while (reflectionsRemaining > 0)
        {
            Ray ray = new Ray(currentStartPoint, currentDirection);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxStepDistance, layerMask))
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                if (hit.collider.CompareTag("Reflective"))
                {
                    // Reflect the laser off the surface
                    currentStartPoint = hit.point + currentDirection * 0.01f; // Small offset to prevent re-collision
                    currentDirection = Vector3.Reflect(currentDirection, hit.normal);
                    reflectionsRemaining--;
                }
                else
                {
                    // Stop the laser if it hits a non-reflective surface
                    //Get other objects name
                    Debug.Log(hit.collider.name);
                    //if other object is equal to a variable name then send message to parent that correct object has been hit
                    if (hit.collider.name == targetObjectName)
                    {
                        Debug.Log("Correct object hit");
                        hit.collider.transform.parent.SendMessage("HitByLaserCorrect");
                        OnHitByLaserCorrect.Invoke();
                    }
                    //Send message to parent of other object that it has been hit
                    hit.collider.transform.parent.SendMessage("HitByLaser", hit.collider.name);
                    Debug.Log("Hit: " + hit.collider.name);
                    break;
                }
            }
            else
            {
                // Extend the laser to the maximum distance if no hit occurs
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.GetPoint(maxStepDistance));
                break;
            }
        }
    }
}
