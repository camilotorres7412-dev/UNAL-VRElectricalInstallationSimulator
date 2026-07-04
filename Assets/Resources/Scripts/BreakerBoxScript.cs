using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Enables and disables breaker box depending on whether it has been installed, 
/// breaker switching, and relationship with other electric fixtures.
/// </summary>

public class BreakerBoxScript : MonoBehaviour
{   
    private void OnEnable()
    {
        AnchorMagnet.OnPlaced += UpdateBreakerBoxComponents;
    }

    private void OnDisable()
    {
        AnchorMagnet.OnPlaced -= UpdateBreakerBoxComponents;
    }

    // Method called as soon as anchoring process is finished
    public void UpdateBreakerBoxComponents()
    {
        // Disable Rigidbody physics, grab interactions and collider
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<XRGrabInteractable>().enabled = false;

        // Add a sphere collider to the breaker socket for anchoring behavior
        GameObject breakerSocket = transform.Find("BreakerPosition").gameObject;

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
        AnchorMagnet.OnPlaced -= UpdateBreakerBoxComponents;
    }
}
