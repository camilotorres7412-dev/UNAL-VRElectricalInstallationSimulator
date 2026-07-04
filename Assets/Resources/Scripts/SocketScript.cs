using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SocketScript : MonoBehaviour
{
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
        GetComponent<Rigidbody>().isKinematic = true;

        // Change interaction layer to Wireable for Cable Spool compatibility
        gameObject.layer = LayerMask.NameToLayer("Wireable");

        // Unsubscribe from event to prevent future calls upon object placement
        AnchorMagnet.OnPlaced -= UpdateComponents; 
    }
}