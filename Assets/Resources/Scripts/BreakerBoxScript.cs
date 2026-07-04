using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Enables and disables breaker box depending on whether it has been installed, 
/// breaker switching, and relationship with other electric fixtures.
/// </summary>

public class BreakerBoxScript : MonoBehaviour
{   
    // Subscribes to the OnPlaced method when a valid collider is hit
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object has a specific tag
        if (other.CompareTag("Blueprint"))
        {
            AnchorMagnet.OnPlaced += UpdateComponents;
        }
    }

    // Method called as soon as anchoring process is finished
    public void UpdateComponents()
    {
        // Disable Rigidbody physics, grab interactions and collider
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<XRGrabInteractable>().enabled = false;

        // Add a sphere collider to the breaker socket for anchoring behavior
        GameObject breakerSocket = transform.Find("BreakerPos").gameObject;

        SphereCollider breakerSocketCollider = breakerSocket.AddComponent<SphereCollider>();
        breakerSocketCollider.isTrigger = true;
        breakerSocketCollider.radius = 0.1f;

        // Find first breaker socket and add an AnchorMagnet
        breakerSocket.AddComponent<AnchorMagnet>();

        // Enable doorlid box collider and simple XR interactable for opening
        GameObject doorlid = transform.Find("Inner Lid").gameObject;

        doorlid.GetComponent<BoxCollider>().enabled = true;
        doorlid.GetComponent<XRSimpleInteractable>().enabled = true;

        // Unsubscribe from future OnPlaced events
        AnchorMagnet.OnPlaced -= UpdateComponents;
    }
}
