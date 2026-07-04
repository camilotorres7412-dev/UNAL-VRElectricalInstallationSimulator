using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Handles the "Measuring" mode logic, providing height measurements and performance metrics
/// </summary>

public class TapeScript : MonoBehaviour
{
    public static event Action OnTapeActivated;

    // Raycast hit point and fixture only mask
    private RaycastHit hit;

    // Objects for selection indicator text
    private GameObject textObject;
    private TextMeshPro tapeObject;
    private LineRenderer lineRenderer;

    private LayerMask layerMask;

    // Logic variables
    private bool tapeSelected = false;

    // Store String with height value
    public string height = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        layerMask = LayerMask.GetMask("Wireable");

        // Add a LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Set the material
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        // Set the width
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;

        // Set the number of vertices
        lineRenderer.positionCount = 2;

        // Get the display text gameobject
        textObject = transform.GetChild(4).gameObject;

        // Get the tape's text mesh pro component
        tapeObject = textObject.GetComponent<TextMeshPro>();
    }

    // Method called upon pickup, generates a guiding raycast from the tape and names selected object
    public void TapeSelected()
    {
        tapeSelected = true;
        lineRenderer.enabled = true;
    }

    // Method called upon drop, disables guiding raycast
    public void TapeUnselected()
    {
        tapeSelected = false;
        lineRenderer.enabled = false;
    }

    public void OnActivated()
    {
        OnTapeActivated?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (tapeSelected)
        {
            // Draw the red guiding raycast with start and end positions
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + (transform.forward * 5f));

            // Create raycast for object selection text indicator and change guiding ray color
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10f, layerMask))
            {
                // Update the color of the guiding raycast while a valid object is hit
                lineRenderer.startColor = Color.green;
                lineRenderer.endColor = Color.green;

                // Calculate height of hit gameObject, approximate to 2 decimal units and add unit indicator
                height = "Altura " + hit.collider.gameObject.name + " :" + hit.collider.gameObject.transform.position.y.ToString("0.00") + "m";

                // Update content of the Text object
                tapeObject.text = height;
            }

            else
            {
                // Reset guiding raycast color and text when no valid object is found
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;
                height = "";
                tapeObject.text = height;
            }
        }
    }
}