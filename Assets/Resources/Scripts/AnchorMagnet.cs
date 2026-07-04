using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Enables smooth motion and insertion of fixtures into wall anchors
/// </summary>

public class AnchorMagnet : MonoBehaviour
{
    private bool enableUpdate = false;

    // Store incoming fixture which touched the sphere collider
    private GameObject incomingFixture;

    // Store slerp scalar
    private float slerpX = 0f;

    // Store meshes for comparison to avoid unwanted objects from anchoring
    private Mesh anchorMesh;
    private Mesh localMesh;


    // Method called by the wall anchor when a fixture enters its collider
    private void OnTriggerEnter(Collider other)
    {
        // Get the game object of the incoming fixture and its mesh filter for comparison
        incomingFixture = other.gameObject;

        if (incomingFixture.GetComponent<MeshFilter>() != null)
        {
            anchorMesh = incomingFixture.GetComponent<MeshFilter>().sharedMesh;
            localMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
        }

        // Only anchor fixtures with equal mesh filters
        if (anchorMesh == localMesh)
        {
            // Disable wireframe clone rendering
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            foreach (Renderer r in GetComponentsInChildren<MeshRenderer>())
            {
                r.enabled = false;
            }

            // Disable wireframe clone collider
            gameObject.GetComponent<SphereCollider>().enabled = false;

            // Disable held fixture grab
            incomingFixture.GetComponent<XRGrabInteractable>().enabled = false;

            // Enable position updates every frame
            enableUpdate = true;
        }
    }

    // Handles Slerp into target position when enabled
    void Update()
    {
        if (enableUpdate == true)
        {
            // Bandaid fix for isKinematic
            // Maybe because of GrabInteractor shenanigans?
            incomingFixture.GetComponent<Rigidbody>().isKinematic = true;

            // Smoothly move and rotate into anchor position
            incomingFixture.transform.position = Vector3.Slerp(incomingFixture.transform.position, gameObject.transform.position, Mathf.Clamp(slerpX, 0, 1));
            incomingFixture.transform.rotation = Quaternion.Slerp(incomingFixture.gameObject.transform.rotation, gameObject.transform.rotation, Mathf.Clamp(slerpX, 0, 1));

            slerpX += 0.01f;

            // Execute logic when the object arrives at its destination
            if (Mathf.Clamp(slerpX, 0, 1) == 1)
            {
                IFixturePlacer triggerable = incomingFixture.GetComponent<IFixturePlacer>();
                if (triggerable != null)
                {
                    triggerable.OnPlaced();
                }

                // Destroy anchor object
                Destroy(gameObject);
            }
        }
    }
}
