using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Enables light if powered and switched on
/// </summary>

public class LightBulbScript : MonoBehaviour
{
    private void OnEnable()
    {
        AnchorMagnet.OnPlaced += UpdateLightbulbComponents;
    }

    private void OnDisable()
    {
        AnchorMagnet.OnPlaced -= UpdateLightbulbComponents;
    }

    // Method called as soon as anchoring process is finished
    public void UpdateLightbulbComponents()
    {
        // Disable Rigidbody physics, grab interactions and collider
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<XRGrabInteractable>().enabled = false;

        // Unsubscribe from future OnPlaced events
        AnchorMagnet.OnPlaced -= UpdateLightbulbComponents;
    }

    void Update()
    {
        if (GetComponent<ElectricalAttributes>().powered == true)
        {
            GetComponent<Light>().enabled = true;

        }
        else
        {
            GetComponent<Light>().enabled = false;
        }
    }
}
