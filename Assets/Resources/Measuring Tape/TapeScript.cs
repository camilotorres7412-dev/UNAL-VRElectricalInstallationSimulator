using UnityEngine;
using TMPro;

/// <summary>
/// Handles the "Measuring" mode logic, providing height measurements and performance metrics
/// </summary>

public class TapeScript : MonoBehaviour
{

    // Raycast hit point and fixture only mask
    private RaycastHit hit;
    private LayerMask layerMask;

    // Objects for selection indicator text
    private GameObject textObject;
    private TextMeshPro tapeObject;
    private LineRenderer lineRenderer;

    // Logic variables
    private bool tapeSelected = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Filter selection of non-fixture objects
        layerMask = LayerMask.GetMask("Fixture");

        // Add a LineRenderer component
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Set the material
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color
        lineRenderer.endColor = Color.red;

        // Set the width
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;

        // Set the number of vertices
        lineRenderer.positionCount = 2;

        // Create TMP object which indicates the object being pointed at
        textObject = new GameObject("Tape Selector");

        // Text spawns with flipped alignment, so the minus fixes it
        // Uncomment or remove
        // textObject.transform.LookAt(-hit.normal);

        tapeObject = textObject.AddComponent<TextMeshPro>();

        tapeObject.text = "";
        tapeObject.fontSize = 2;
        tapeObject.color = Color.white;
        tapeObject.alignment = TextAlignmentOptions.Left;
    }

    // Method called upon pickup, generates a guiding raycast from the tape and names selected object
    public void TapeSelected()
    {
        tapeSelected = true;
    }

    // Method called upon drop, disables guiding raycast
    public void TapeUnselected()
    {
        tapeSelected = false;
    }

    // Method called continously during trigger pull, stores the object that is selected by the raycast
    public void TapeActivated()
    {
        // Create raycast for object selection and obtaining height, filter based on Fixture mask
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tapeSelected)
        {
        // Draw the red guiding raycast with start and end positions
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + (transform.forward * 5f));

        // Cleanup: Update the following logic to do the assignments only once?

        // Create raycast for object selection text indicator and change guiding ray color, filter based on Fixture mask
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                // Update the color of the guiding raycast while a valid object is hit
                lineRenderer.endColor = Color.green;

                // Update position of the Text object, inherited from the base object
                tapeObject.transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z + 0.1f);

                // Update content of the Text object
                tapeObject.text = hit.collider.gameObject.name;
            }

        // Reset guiding raycast color and text when no valid object is found
        lineRenderer.endColor = Color.red;
        tapeObject.text = "";
        }
    }
}