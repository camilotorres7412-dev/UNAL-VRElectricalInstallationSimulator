using UnityEngine;
using System.Collections;

public class ToggleUIAnimator : MonoBehaviour
{
    // Referenced via Inspector in Editor
    public Transform childUITransform;

    // Adjust duration of UI transition
    private float animationDuration = 0.5f;

    private bool activateUI = false;

    void OnTriggerEnter(Collider other) 
    {
        Destroy(other.gameObject);
    }

    // Activation method associated via inspector
    public void OnXRActivate()
    {
        // Enable toggle-like behavior on each call
        if (activateUI == false) {activateUI = true;}

        else {activateUI = true;}
    }

    private IEnumerator ScaleUIRoutine(Vector3 targetScale)
    {
        Vector3 startScale = childUITransform.transform.localScale;

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            // Increment percentage over time
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);

            // Apply Slerp
            childUITransform.transform.localScale = Vector3.Slerp(startScale, targetScale, t);

            // Wait for the next frame
            yield return null;
        }

        // Ensure exact target rotation and position at the end
        childUITransform.transform.localScale = targetScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (activateUI)
        {
            Vector3 targetScale = new(0.001f, 0.001f, 0.001f);

            // Prevent animation overlap
            StopAllCoroutines();

            // Commence animation with acquired target scale
            StartCoroutine(ScaleUIRoutine(targetScale));
        }

        else
        {
            Vector3 targetScale = new(0f, 0f, 0f);

            // Prevent animation overlap
            StopAllCoroutines();

            // Commence animation with acquired target scale
            StartCoroutine(ScaleUIRoutine(targetScale));
        }
    }
}
