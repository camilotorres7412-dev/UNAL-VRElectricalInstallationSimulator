using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GeneralSwitch : MonoBehaviour, IFixturePlacer
{
    private bool enableUpdate = false;

    // Store the rotation of the swtich pole child
    private Quaternion switchRotation;

    // Logic flag for on/off position (not power)
    private bool isOn = false;

    // Slerp constant
    private float slerp = 0f;

    // Called when turned into a static object
    public void OnPlaced()
    {
        // Disable rigidbody physics and grab interactions
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<XRGrabInteractable>().enabled = false;

        // Enable button behavior
        //GameObject switchPole = gameObject.transform.GetChild(0).gameObject;
        //GetComponent<XRSimpleInteractable>().enabled = true;
    }

    // Method called when activated as a static object, flips the switch and enables or disables power
    public void ToggleOnOff()
    {
        // Disable button to prevent spamming
        GetComponent<XRSimpleInteractable>().enabled = false;

        // Activate update function
        enableUpdate = true;
    }

    void Update()
    {
        if (enableUpdate == true)
        {
            if (isOn == false)
            {
                switchRotation = Quaternion.Lerp(switchRotation, switchRotation * Quaternion.Euler(3, 0, 0), slerp);

                // Rotation location increment
                slerp += 0.01f;

                if (Mathf.Clamp(slerp, 0f, 1f) == 1)
                {
                    // Re-enable button pressing
                    GetComponent<XRSimpleInteractable>().enabled = true;

                    // Set power to true
                    GetComponent<ElectricalAttributes>().powered = true;

                    // Set position to true
                    isOn = true;
                }
            }

            else
            {
                switchRotation = Quaternion.Slerp(switchRotation, switchRotation * Quaternion.Euler(-3, 0, 0), slerp);

                if (Mathf.Clamp(slerp, 0f, 1f) == 1)
                {
                    // Re-enable button pressing
                    GetComponent<XRSimpleInteractable>().enabled = true;

                    // Set power to false
                    GetComponent<ElectricalAttributes>().powered = true;

                    // Set position to false
                    isOn = false;
                }
            }
        }
    }
}