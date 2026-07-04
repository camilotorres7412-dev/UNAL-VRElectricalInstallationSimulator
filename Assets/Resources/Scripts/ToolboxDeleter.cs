using System.Collections;
using UnityEngine;

public class ToolboxDeleter : MonoBehaviour
{
    [SerializeField] private ToolboxAnimation toolboxAnimator;
    void OnTriggerEnter(Collider other) 
    {
        // Destroy gameobjects that enter the toolbox
        // Layer filters assigned via inspector
        Destroy(other.gameObject);

        Quaternion targetRotation = Quaternion.Euler(-90f, 0f, 0f);

        // Prevent animation overlap
        toolboxAnimator.StopAllCoroutines();

        // Commence animation with acquired target rotation
        toolboxAnimator.StartCoroutine(toolboxAnimator.SlerpRoutine(targetRotation));
    }
}
