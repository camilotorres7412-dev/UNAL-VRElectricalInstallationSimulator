using UnityEngine;
using TMPro;

/// <summary>
/// Enables connection and disconnection of electric terminals on fixtures
/// </summary>

public class CableSpoolTool : MonoBehaviour
{

    // Raycast hit point and fixture only mask
    private RaycastHit hit;

    // Objects for selection indicator text
    private TextMeshPro textIndicator;
    private LineRenderer lineRenderer;

    // Logic variable for update call control
    private bool spoolSelected = false;

    // Store instance of Grab Point child for raycasting
    Transform grabPoint;

    // Store source and target gameobjects
    GameObject sourceObject;
    GameObject targetObject;

    // Store selection process status
    private int selectionStatus = 0;

    private int layerMask;

    public void CreateLineRenderer()
    {
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
    }

    void Start()
    {
        CreateLineRenderer();

        // Get Grab Point for Raycast casting
        grabPoint = gameObject.transform.GetChild(1);

        // Get the display text component
        textIndicator = transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();

        layerMask = LayerMask.GetMask("Wireable");
    }

    public void OnSelect()
    {
        spoolSelected = true;
        lineRenderer.enabled = true;
    }

    public void OnUnselect()
    {
        spoolSelected = false;
        lineRenderer.enabled = false;
    }

    // Method called once with every trigger pull, selects the highlighted object and enables connection logic
    public void OnActivated()
    {
        // Cast selection ray
        if (Physics.Raycast(grabPoint.position, grabPoint.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            switch (selectionStatus)
            {
                // Selection of source object
                case 0:
                    // Activate logic only if a fixture is hit
                    if (hit.collider.transform.CompareTag("Fixture"))
                    {
                        sourceObject = hit.collider.gameObject;
                        lineRenderer.startColor = Color.yellow;
                        lineRenderer.endColor = Color.yellow;
                        selectionStatus = 1;
                    }

                    break;

                // Selection of target object & apply logic
                case 1:
                    if (hit.collider.transform.CompareTag("Fixture"))
                    {
                        targetObject = hit.collider.gameObject;
                    }
                    
                    else {break;}

                    // 
                    sourceObject.GetComponent<ElectricalAttributes>().wireOut = targetObject;
                    targetObject.GetComponent<ElectricalAttributes>().wireIn = sourceObject;

                    // Get the electrical attributes of both objects
                    ElectricalAttributes sourceAttributes = sourceObject.GetComponent<ElectricalAttributes>();
                    ElectricalAttributes targetAttributes = targetObject.GetComponent<ElectricalAttributes>();

                    // Check if the source of the connection is powered
                    if (sourceAttributes.powered)
                    {
                        // Power this object if true
                        targetAttributes.powered = true;
                    }

                    else
                    {   
                        // Cut power if false
                        targetAttributes.powered = false;
                    }
                    selectionStatus = 0;
                    break;
            }
        }
    }

    void Update()
    {
        if (spoolSelected)
        {
        // Draw the red guiding raycast with start and end positions
        lineRenderer.SetPosition(0, grabPoint.transform.position);
        lineRenderer.SetPosition(1, grabPoint.transform.position + (grabPoint.transform.forward * 5f));

        // Create raycast for object selection text indicator and change guiding ray color
        if (Physics.Raycast(grabPoint.transform.position, grabPoint.transform.TransformDirection(Vector3.forward), out hit, 10f, layerMask))
        {
            // Check if the hit object is a fixture
            if (hit.transform.CompareTag("Fixture"))
            {
                // Update the color of the guiding raycast while a valid object is hit
                lineRenderer.startColor = Color.green;
                lineRenderer.endColor = Color.green;

                // Update content of the Text object
                textIndicator.text = "Seleccionando: " + hit.collider.gameObject.name;
            }
        }
    
        else
        {
            // Reset guiding raycast color and text when no valid object is found
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            textIndicator.text = "Sin selección";
        }
        }
    }
}
