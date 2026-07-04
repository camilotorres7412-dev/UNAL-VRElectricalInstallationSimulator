using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class MS_InnerLid : MonoBehaviour, IFixturePlacer
{
    // Store Slerp position
    private float slerpX;

    // Method called as soon as anchoring process is finished
    public void OnPlaced()
    {
        // Enable box collider and simple interactable
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<XRSimpleInteractable>().enabled = true;
    }

    // Method called on activation, enables gesture tracking for opening the door
    private void DoorRotation()
    {
        // Fixed gesture distance which the controller must travel to complete the gesture
        Vector3 currentPosition = GetComponent<XRSimpleInteractable>().interactorsSelecting[0].transform.position;

        Vector3 TargetPosition = currentPosition;

        // Tracking = initial controller position vs move to the right for open, to the left for close

    }

    private void SlerpRotate()
    {
        // Smoothly move and rotate into anchor position
        transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * Quaternion.Euler(0, 0, 0), slerpX);

        slerpX += 0.01f;

        // Execute logic when the object arrives at its destination
        if (Mathf.Clamp(slerpX, 0, 1) == 1)
        {
            IFixturePlacer triggerable = GetComponent<IFixturePlacer>();
            if (triggerable != null)
            {
                triggerable.OnPlaced();
            }
        }
    }

    void Update()
    {
        if (true)
        {
            SlerpRotate();
        }
    }
    
}
