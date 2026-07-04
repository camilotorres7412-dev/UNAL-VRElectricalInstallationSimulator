using UnityEngine;

public class BagScript : MonoBehaviour
{
    // Used for storing the UI transform
    private Transform child;

    private bool activateUI = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the canvas element that drives the tool selection UI
        child = gameObject.transform.GetChild(3);
    }

    public void OnRaycastSelect()
    {
        // If deactivated, enable, else disable
        if (activateUI == false)
        {
            activateUI = true;
        }

        else
        {
            activateUI = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activateUI)
        {
            child.transform.localScale = Vector3.Lerp(child.transform.localScale, new Vector3(0.001f, 0.001f, 0.001f), 5.0f * Time.deltaTime);
        }

        else
        {
            child.transform.localScale = Vector3.Lerp(child.transform.localScale, new Vector3(0, 0, 0), 5.0f * Time.deltaTime);
        }
    }
}
