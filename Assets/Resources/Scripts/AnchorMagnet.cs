using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Attach to an object to make it so that when an object with an equal identifier touches its collider,
/// said object will be transported into this object. 
/// </summary>

public class AnchorMagnet : MonoBehaviour
{
    public static event Action OnPlaced;
    private float animationDuration = 1f;
    private GameObject incomingFixture;

    // Collider filter defined in "HammerScript". Only reacts to "Fixture" layer objects
    private void OnTriggerEnter(Collider other)
    {
        incomingFixture = other.gameObject;

        MeshFilter incomingFilter = incomingFixture.GetComponent<MeshFilter>();

        // Only anchor matching mesh filters (In case a different object is picked up)
        if (GetComponent<MeshFilter>().sharedMesh == incomingFilter.sharedMesh)
        {
        // Disable blueprint clone rendering
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            foreach (Renderer r in GetComponentsInChildren<MeshRenderer>())
            {
                r.enabled = false;
            }

            // Disable blueprint clone collider
            gameObject.GetComponent<SphereCollider>().enabled = false;

            // Disable held fixture grab
            incomingFixture.GetComponent<XRGrabInteractable>().enabled = false;

            // Enable IsKinematic to prevent object from falling to the floor
            incomingFixture.GetComponent<Rigidbody>().isKinematic = true;

            // Prevent animation overlap
            StopAllCoroutines();

            // Commence animation with acquired target rotation
            StartCoroutine(SlerpRoutine(gameObject.transform.position, gameObject.transform.rotation));
        }
    }

    private IEnumerator SlerpRoutine(Vector3 targetPosition, Quaternion targetRotation)
    {
        Vector3 startPosition = incomingFixture.transform.position;
        Quaternion startRotation = incomingFixture.transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            // Increment percentage over time
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);

            // Apply Slerp
            incomingFixture.transform.position = Vector3.Slerp(startPosition, targetPosition, t);
            incomingFixture.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // Wait for the next frame
            yield return null;
        }

        // Ensure exact target rotation and position at the end
        incomingFixture.transform.position = targetPosition;
        incomingFixture.transform.rotation = targetRotation;

        // Invoke component update method on fixture object
        OnPlaced?.Invoke();

        // Destroy this blueprint
        Destroy(gameObject);
    }
}
