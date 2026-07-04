using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Attach to an object to make it so that when an object with an equal identifier touches its collider,
/// said object will be transported into this object. 
/// </summary>

public class AnchorMagnet : MonoBehaviour
{
    public static event Action OnPlaced;
    private bool enableUpdate = false;

    private GameObject incomingFixture;

    private float slerpX = 0f;

    public string anchorID;
    private string incomingAnchorID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Fixture"))
        {
            incomingFixture = other.gameObject;
            
            // Check if the object that touched has attributes
            if (incomingFixture.GetComponent<ObjectAttributes>() != null)
            {

                incomingAnchorID = incomingFixture.GetComponent<ObjectAttributes>().anchorID;

                // Only anchor matching IDs
                if (anchorID == incomingAnchorID)
                {
                    // Disable wireframe clone rendering
                    gameObject.GetComponent<MeshRenderer>().enabled = false;

                    foreach (Renderer r in GetComponentsInChildren<MeshRenderer>())
                    {
                        r.enabled = false;
                    }

                    // Disable wireframe clone collider
                    gameObject.GetComponent<Collider>().enabled = false;

                    // Disable held fixture grab
                    incomingFixture.GetComponent<XRGrabInteractable>().enabled = false;

                    // Bandaid fix for isKinematic
                    // Maybe because of GrabInteractor shenanigans?
                    incomingFixture.GetComponent<Rigidbody>().isKinematic = true;

                    // Enable position updates every frame
                    enableUpdate = true;
                }
            }
        }
    }

    // Handles Slerp into target position when enabled
    void Update()
    {
        if (enableUpdate == true)
        {
            // Smoothly move and rotate into anchor position
            incomingFixture.transform.position = Vector3.Slerp(incomingFixture.transform.position, gameObject.transform.position, Mathf.Clamp(slerpX, 0, 1));
            incomingFixture.transform.rotation = Quaternion.Slerp(incomingFixture.gameObject.transform.rotation, gameObject.transform.rotation, Mathf.Clamp(slerpX, 0, 1));

            slerpX += 0.01f;

            // Execute logic when the object arrives at its destination
            if (Mathf.Clamp(slerpX, 0, 1) == 1)
            {
                OnPlaced?.Invoke();

                // Destroy anchor object
                Destroy(gameObject);
            }
        }
    }
}
