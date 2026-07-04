using UnityEngine;

public static class GlobalFunctions
{
    public static void RenderRaycast(GameObject caster, LineRenderer lineRenderer, Ray ray, RaycastHit hit)
    {
        // Perform the raycast
        if (Physics.Raycast(caster.transform.position, caster.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            // If hit, draw line from origin to hit point
            lineRenderer.SetPosition(0, caster.transform.position);
            lineRenderer.SetPosition(1, hit.point);
        }

        else
        {
            // If no hit, draw line from origin to max distance in ray direction
            lineRenderer.SetPosition(0, caster.transform.position);
            lineRenderer.SetPosition(1, caster.transform.position + ray.direction * 10);
        }
    }
}
