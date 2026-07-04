using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BreakerScript : MonoBehaviour
{
    private bool activated = false;
    private float animationDuration = 1f;

    // Get and store switch pole (child) transform
    Transform poleTransform;

    private void OnEnable()
    {
        AnchorMagnet.OnPlaced += UpdateComponents;

        poleTransform = transform.Find("Breaker Switch");
    }

    // Method associated to XR Simple Interactable - On Select, initiates rotation animation
    public void OnSelect()
    {
        if (activated == false)
        {
            activated = true;

            // Rotation towards "on" state
            Quaternion targetRotation = Quaternion.Euler(90f, -135f, 0f);

            TriggerSlerpAnimation(targetRotation);
        }

        else
        {
            activated = false;

            // Rotation towards "off" state
            Quaternion targetRotation = Quaternion.Euler(90f, 0f, 0f);

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

        // Alternate power states
        if (activated) {GetComponent<ElectricalAttributes>().powered = true;}

        else {GetComponent<ElectricalAttributes>().powered = false;}

        // Ensure we hit the exact target rotation at the end
        poleTransform.localRotation = targetRotation;
    }

    // Method called once the breaker reaches its final position after magnetism
    private void UpdateComponents()
    {
        // Disable grab behavior, physics updates and enable switch behavior instead
        GetComponent<XRGrabInteractable>().enabled = false;
        GetComponent<XRSimpleInteractable>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = true;

        // Unsubscribe from event to prevent future calls upon object placement
        AnchorMagnet.OnPlaced -= UpdateComponents;        
    }
}