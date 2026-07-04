using UnityEngine;

/// <summary>
/// Enables light if powered and switched on
/// </summary>

public class LightHandler : MonoBehaviour
{

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
