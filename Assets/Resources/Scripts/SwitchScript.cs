using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SwitchScript : MonoBehaviour
{
    private bool activated = false;
    private float animationDuration = 1f;

    // Get and store switch pole (child) transform
    Transform poleTransform;

    // Subscribe to event on enable
    private void OnEnable()
    {
        AnchorMagnet.OnPlaced += UpdateSwitchComponents;

        poleTransform = transform.Find("SwitchPole");
    }

    // Unsubscribe to event on disable or destruction
    private void OnDisable()
    {
        AnchorMagnet.OnPlaced -= UpdateSwitchComponents;
    }

    // Method called as soon as anchoring process is finished
    public void UpdateSwitchComponents()
    {
        // Disable grab behavior, physics updates and enable switch behavior instead
        GetComponent<XRGrabInteractable>().enabled = false;
        GetComponent<XRSimpleInteractable>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = true;

        // Unsubscribe from event to prevent future calls upon object placement
        AnchorMagnet.OnPlaced -= UpdateSwitchComponents; 
    }

    // Method associated to XR Simple Interactable - On Select, initiates rotation animation
    public void OnSelect()
    {
        if (activated == false)
        {
            activated = true;

            // Rotation towards "on" state
            Quaternion targetRotation = Quaternion.Euler(4f, 0f, 0f);

            TriggerSlerpAnimation(targetRotation);
        }

        else
        {
            activated = false;

            // Rotation towards "off" state
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);

            TriggerSlerpAnimation(targetRotation);

            GetComponent<ElectricalAttributes>().powered = false;
        }
    }

    public void TriggerSlerpAnimation(Quaternion targetRotation)
    {
        // Prevent animation overlap
        StopAllCoroutines();

        // Commence animation with acquired target rotation
        StartCoroutine(SlerpRoutine(targetRotation));
    }

    private IEnumerator SlerpRoutine(Quaternion targetRotation)
    {
        Quaternion startRotation = poleTransform.localRotation;

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            // Increment percentage over time
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);

            // Apply Slerp
            poleTransform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // Wait for the next frame
            yield return null;
        }

        // Check if input wire is powered then power, otherwise keep unpowered
        if (activated)
        {
            GameObject inputFixture = GetComponent<ElectricalAttributes>().wireIn;

            if (inputFixture.GetComponent<ElectricalAttributes>().powered == true)
            {
                GetComponent<ElectricalAttributes>().powered = true;
            }
        }

        else 
        {
            GetComponent<ElectricalAttributes>().powered = false;
        }

        // Ensure we hit the exact target rotation at the end
        poleTransform.localRotation = targetRotation;
    }
}