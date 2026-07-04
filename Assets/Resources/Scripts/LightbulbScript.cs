using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Enables light if powered and switched on
/// </summary>

public class LightBulbScript : MonoBehaviour
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
        // Disable Rigidbody physics, grab interactions
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<XRGrabInteractable>().enabled = false;

        // Change interaction layer to Wireable for Cable Spool compatibility
        gameObject.layer = LayerMask.NameToLayer("Wireable");

        // Unsubscribe from future OnPlaced events
        AnchorMagnet.OnPlaced -= UpdateComponents;
    }

    void Update()
    {
        if (GetComponent<ElectricalAttributes>().devicePower == true)
        {
            GetComponent<Light>().enabled = true;

        }

        else
        {
            GetComponent<Light>().enabled = false;
        }
    }
}
