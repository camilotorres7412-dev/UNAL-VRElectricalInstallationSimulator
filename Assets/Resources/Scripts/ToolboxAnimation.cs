using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the opening animation of the toolbox' lid. Layer filters added via inspector
/// </summary>

public class ToolboxAnimation : MonoBehaviour
{
    [SerializeField] private Transform lidTransform;

    private readonly float animationDuration = 1f;

    void OnTriggerEnter() 
    {
        // Rotation towards "open" state
        Quaternion targetRotation = Quaternion.Euler(-200f, 0f, 0f);

        // Prevent animation overlap
        StopAllCoroutines();

        // Commence animation with acquired target rotation
        StartCoroutine(SlerpRoutine(targetRotation));
    }

    void OnTriggerExit()
    {
        // Rotation towards "off" state
        Quaternion targetRotation = Quaternion.Euler(-90f, 0f, 0f);

        // Prevent animation overlap
        StopAllCoroutines();

        // Commence animation with acquired target rotation
        StartCoroutine(SlerpRoutine(targetRotation));
    }

    public IEnumerator SlerpRoutine(Quaternion targetRotation)
    {
        Quaternion startRotation = lidTransform.localRotation;

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            // Increment percentage over time
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);

            // Apply Slerp
            lidTransform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // Wait for the next frame
            yield return null;
        }

        // Ensure we hit the exact target rotation at the end
        lidTransform.localRotation = targetRotation;
    }
}
