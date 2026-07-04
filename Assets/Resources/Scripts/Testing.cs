using UnityEngine;


public class Testing : MonoBehaviour
{
    // Store transform upon function call
    Transform pointTransform;

    // Store original and target positions
    Vector3 originalPosition;
    Vector3 targetPosition;

    // Store calculated original and target gesture vectors
    Vector3 playerGestureVector;
    Vector3 targetGestureVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Fixed gesture distance which the controller must travel to complete the gesture
        pointTransform = GameObject.Find("TestBall").transform;

        originalPosition = pointTransform.position;

        pointTransform.Translate(new Vector3(-0.1f, 0f, 0.1f));

        Debug.Log("Original position:" + originalPosition);

        targetPosition = pointTransform.position;

        Debug.Log("Displacement position:" + targetPosition);

        targetGestureVector = targetPosition - originalPosition;

        Debug.Log("Gesture vector is:" + targetGestureVector);

    }


    // Update is called once per frame
    void Update()
    {
        // Track and calculate current player gesture vector
        playerGestureVector = GameObject.Find("TestBall").transform.position - originalPosition;

        // Find difference between target and player gesture vector X value
        float diffx = playerGestureVector.x - targetGestureVector.x;

        if (diffx < 0)
        {
            // Run trigger code
        }
    }
}
