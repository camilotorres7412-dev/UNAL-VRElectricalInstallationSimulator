using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SwitchScript : MonoBehaviour
{
    private bool activated = false;
    private float animationDuration = 1f;

    [SerializeField] Transform poleTransform;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blueprint"))
        {
            AnchorMagnet.OnPlaced += UpdateComponents;
        }
    }

    // Method called as soon as anchoring process is finished
    public void UpdateComponents()
    {
        // Disable grab behavior, physics updates and enable switch behavior instead
        GetComponent<XRGrabInteractable>().enabled = false;
        GetComponent<XRSimpleInteractable>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = true;

        // Unsubscribe from event to prevent future calls upon object placement
        AnchorMagnet.OnPlaced -= UpdateComponents; 
    }

    // Method associated to XR Simple Interactable - On Select, initiates rotation animation
    public void OnSelect()
    {
        if (activated == false)
        {
            activated = true;

            // Rotation towards "on" state
            Quaternion targetRotation = Quaternion.Euler(4f, 0f, 0f);

            // Prevent animation overlap
            StopAllCoroutines();

            // Commence animation with acquired target rotation
            StartCoroutine(SlerpRoutine(targetRotation, true));
        }

        else
        {
            activated = false;

            // Rotation towards "off" state
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);

            // Prevent animation overlap
            StopAllCoroutines();

            // Commence animation with acquired target rotation
            StartCoroutine(SlerpRoutine(targetRotation, false));
        }
    }

    private IEnumerator SlerpRoutine(Quaternion targetRotation, bool isPowered)
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

        // Toggle device power
        GetComponent<ElectricalAttributes>().devicePower = isPowered;

        // Ensure we hit the exact target rotation at the end
        poleTransform.localRotation = targetRotation;
    }
}