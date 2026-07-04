using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEditor.MemoryProfiler;

public class BreakerScript : MonoBehaviour
{
    private bool activated = false;
    private float animationDuration = 1f;
    [SerializeField] private Transform poleTransform;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blueprint"))
        {
            AnchorMagnet.OnPlaced += UpdateComponents;
        }
    }

    public void OnXRActivated()
    {
        if (activated == false)
        {
            activated = true;

            // Rotation towards "on" state
            Quaternion targetRotation = Quaternion.Euler(90f, -135f, 0f);

            StopAllCoroutines();

            // Commence animation with acquired target rotation
            StartCoroutine(SlerpRoutine(targetRotation, true));
        }

        else
        {
            activated = false;

            // Rotation towards "off" state
            Quaternion targetRotation = Quaternion.Euler(90f, 0f, 0f);

            StopAllCoroutines();

            // Commence animation with acquired target rotation
            StartCoroutine(SlerpRoutine(targetRotation, false));

            GetComponent<ElectricalAttributes>().powered = false;
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

        // Enable or disable power
        GetComponent<ElectricalAttributes>().powered = isPowered;

        // Enable power of immediately connected neighbors
        foreach (GameObject connection in GetComponent<ElectricalAttributes>().connections)
        {
            connection.GetComponent<ElectricalAttributes>().Signal(isPowered);
        }

        // Ensure we hit the exact target rotation at the end
        poleTransform.localRotation = targetRotation;
    }

    // Method called once the breaker reaches its final position after magnetism
    private void UpdateComponents()
    {
        // Disable grab behavior, physics updates and enable simple interactable
        GetComponent<XRGrabInteractable>().enabled = false;
        GetComponent<XRSimpleInteractable>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = true;

        // Change interaction layer to Wireable for Cable Spool compatibility
        gameObject.layer = LayerMask.NameToLayer("Wireable");

        // Unsubscribe from event to prevent future calls upon object placement
        AnchorMagnet.OnPlaced -= UpdateComponents;        
    }
}