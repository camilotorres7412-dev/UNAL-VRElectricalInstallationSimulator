using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Enables and disables breaker box depending on whether it has been installed, 
/// breaker switching, and relationship with other electric fixtures.
/// </summary>

public class BreakerHandler : MonoBehaviour, IFixturePlacer
{   
    // Method called as soon as anchoring process is finished
    public void OnPlaced()
    {
        // Disable Rigidbody physics, grab interactions and collider
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<XRGrabInteractable>().enabled = false;

        // Add a sphere collider to the breaker socket for anchoring behavior
        GameObject breakerSocket = GameObject.Find("BreakerPosition");

        SphereCollider breakerSocketCollider = breakerSocket.AddComponent<SphereCollider>();
        breakerSocketCollider.isTrigger = true;
        breakerSocketCollider.radius = 0.1f;

        // Find first breaker socket and add an AnchorMagnet
        breakerSocket.AddComponent<AnchorMagnet>();

        // 
    }
}
