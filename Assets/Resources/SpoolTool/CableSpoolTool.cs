using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Enables connection and disconnection of electric terminals on fixtures
/// </summary>

public class CableSpoolTool : MonoBehaviour
{

    // Store a ray and RaycastHit for object detection
    private Ray ray;
    private RaycastHit hit;

    // Store layermask to avoid unwanted interactions
    LayerMask layerMask;

    // Store Line Renderer for guidance
    LineRenderer lineRenderer;

    // Store a boolean for update() call control
    private bool updateEnabled;

    // Store instance of Grab Point child
    GameObject attachPoint;
    Transform grabPoint;

    // Store source and target gameobjects
    GameObject sourceObject;
    GameObject targetObject;

    // Store selection process status
    private int selectionStatus = 0;

    void Start()
    {
        attachPoint = gameObject.transform.GetChild(0).gameObject;
        grabPoint = attachPoint.transform;
        layerMask = LayerMask.GetMask("Wireable");

        // Create a line renderer for guidance
        lineRenderer = attachPoint.AddComponent<LineRenderer>();

        // Set the material
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color
        lineRenderer.material.color = Color.red;

        // Set the width
        lineRenderer.startWidth = 0.001f;
        lineRenderer.endWidth = 0.001f;

        // Set the number of vertices
        lineRenderer.positionCount = 2;

        // Disable upon start
        lineRenderer.enabled = false;

        updateEnabled = false;
    }

    // Method called once upon picking up the spool. Creates a guiding LineRenderer and enables update
    public void SpoolHeld()
    {
        // Enable guiding ray
        lineRenderer.enabled = true;

        // Enable Update for continous raycasting
        updateEnabled = true;
    }

    // Method called once with every trigger pull, selects the highlighted object and enables connection logic
    public void SpoolActivated()
    {
        // Cast selection ray
        if (Physics.Raycast(grabPoint.position, grabPoint.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            switch (selectionStatus)
            {
                // Selection of source object
                case 0:
                    // Activate logic only if a gameobject is hit
                    if (hit.collider.gameObject != null)
                    {
                        sourceObject = hit.collider.gameObject;
                        lineRenderer.material.color = Color.yellow;
                        selectionStatus = 1;
                    }

                    break;

                // Selection of target object & apply logic
                case 1:
                    targetObject = hit.collider.gameObject;

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

                    lineRenderer.material.color = Color.red;
                    selectionStatus = 0;
                    break;
            }
        }
    }

    // Method called once when the trigger stops being held.
    public void SpoolDropped()
    {
        lineRenderer.enabled = false;
        updateEnabled = false;
    }

    // Called for as long as the tool is being held

    void Update()
    {
        if (updateEnabled == true)
        {
            if (Physics.Raycast(grabPoint.position, grabPoint.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                // If hit, draw line from origin to hit point
                lineRenderer.SetPosition(0, grabPoint.position);
                lineRenderer.SetPosition(1, hit.point);
            }
        }

        else
        {
            // If no hit, draw line from origin to max distance in ray direction
            lineRenderer.SetPosition(0, grabPoint.position);
            lineRenderer.SetPosition(1, grabPoint.position * 10);
        }
    }
}
