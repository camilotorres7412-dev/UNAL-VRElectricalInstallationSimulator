using UnityEngine;

public class BookScript : MonoBehaviour
{
    // Logic variables
    private bool bookSelected = false;

    // Rotation handling variables
    private Transform child;
    private Quaternion start;
    private Quaternion end;
    private float timeCount = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the book cover transform by child index (1)
        child = transform.GetChild(1);

        // Get current rotation (at some point of open at this point)
        start = child.transform.localRotation;

        // Return y axis rotation to zero
        end = Quaternion.Euler(new Vector3(0, 0, 90));
    }

    // Method called upon pickup, flips the book open
    public void BookSelected()
    {
        // Get current rotation (closed at this point)
        start = Quaternion.Euler(child.transform.localEulerAngles);

        // Target rotation on X axis, not Y, and negative for correct animation
        end = child.rotation * Quaternion.Euler(-179, 0, 0);

        bookSelected = true;
    }

    // Method called upon drop, closes the book
    public void BookUnselected()
    {
        bookSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bookSelected)
        {
            // Progressively flip book lid to y-axis 185 degrees
            child.rotation = Quaternion.Slerp(start, end, timeCount);
            timeCount += Time.deltaTime;
        }
        
        else
        {
            // Progressively flip book lid back to zero. This requires inverting the parameters
            child.localRotation = Quaternion.Slerp(start, end, timeCount);
            timeCount += Time.deltaTime;
        }
        
    }
}
