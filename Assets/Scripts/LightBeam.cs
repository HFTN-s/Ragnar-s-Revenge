using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HitEvent : UnityEvent<RaycastHit> { }

public class LightBeam : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int maxReflectionCount = 5;
    public float maxStepDistance = 200;
    public string targetObjectName;
    public UnityEvent OnHitByLaserCorrect;
    public HitEvent OnHitByLaserIncorrect; // Use custom event type

    void Start()
    {
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

        int layerMask = ~0;

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
                    currentStartPoint = hit.point + currentDirection * 0.01f;
                    currentDirection = Vector3.Reflect(currentDirection, hit.normal);
                    reflectionsRemaining--;
                }
                else if (hit.collider.CompareTag("Boundaries"))
                {
                    break;
                }
                else if (hit.collider.CompareTag("Gemstone"))
                {
                    Debug.Log("Hit: " + hit.collider.name);
                    if (hit.collider.name == targetObjectName)
                    {
                        OnHitByLaserCorrect.Invoke();
                    }
                    else
                    {
                        OnHitByLaserIncorrect.Invoke(hit);
                    }
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.GetPoint(maxStepDistance));
                break;
            }
        }
    }
}
