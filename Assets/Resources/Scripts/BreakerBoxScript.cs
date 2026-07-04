using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Enables and disables breaker box depending on whether it has been installed, 
/// breaker switching, and relationship with other electric fixtures.
/// </summary>

public class BreakerBoxScript : MonoBehaviour
{   
    private bool doorOpen = false;

    public List<GameObject> breakerList;
    public GameObject doorLid;

    public GameObject stencilCutter;
    
    private float animationDuration = 0.3f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object has a specific tag
        if (other.CompareTag("Blueprint"))
        {
            AnchorMagnet.OnPlaced += UpdateComponents;
        }
    }

    // Method associated via inspector to Inner Doorlid's XR Simple Interactable - Activate
    public void OnXRActivated()
    {
        if (doorOpen == false)
        {
            doorOpen = true;

            // Rotation towards open
            Quaternion targetRotation = Quaternion.Euler(0f, -125f, 0f);

            StopAllCoroutines();

            StartCoroutine(DoorAnimation(targetRotation, true));
        }

        else
        {
            doorOpen = false;

            // Rotation towards "off" state
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);

            StopAllCoroutines();

            StartCoroutine(DoorAnimation(targetRotation, false));
        }
    }


    private IEnumerator DoorAnimation(Quaternion targetRotation, bool ghostEnable)
    {
        Quaternion startRotation = doorLid.transform.localRotation;

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            // Increment percentage over time
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);

            // Apply Slerp
            doorLid.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // Wait for the next frame
            yield return null;
        }

        // Ensure we hit the exact target rotation at the end
        doorLid.transform.localRotation = targetRotation;

        // Activate or deactivate breaker box ghosts
        if (ghostEnable)
        {
            // Enable Stencil Cutter
            stencilCutter.GetComponent<MeshRenderer>().enabled = true;

            // Enable breaker slots
            foreach (GameObject breaker in breakerList)
            {
                if (breaker != null)
                {
                breaker.GetComponent<BoxCollider>().enabled = true;
                breaker.GetComponent<AnchorMagnet>().enabled = true;
                }
            }
        }

        else
        {
            // Disable Stencil Cutter
            stencilCutter.GetComponent<MeshRenderer>().enabled = false;

            // Disable breaker slots
            foreach (GameObject breaker in breakerList)
            {
                if (breaker != null)
                {
                    breaker.GetComponent<BoxCollider>().enabled = false;
                    breaker.GetComponent<AnchorMagnet>().enabled = false;
                }
            }
        }
    }

    // Method called as soon as anchoring process is finished
    public void UpdateComponents()
    {
        // Disable Rigidbody physics, grab interactions and collider
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<XRGrabInteractable>().enabled = false;

        // Enable doorlid box collider and simple XR interactable for opening
        doorLid.GetComponent<BoxCollider>().enabled = true;
        doorLid.GetComponent<XRSimpleInteractable>().enabled = true;

        // Unsubscribe from future OnPlaced events
        AnchorMagnet.OnPlaced -= UpdateComponents;
    }
}
