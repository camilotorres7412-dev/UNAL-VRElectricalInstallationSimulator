using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.UIElements;

/// <summary>
/// Allows the placing of fixtures against valid surfaces through the creation of
/// "Blueprint" clones
/// </summary>

public class HammerScript : MonoBehaviour
{
    private XRGrabInteractable thisHammer;

    private XRBaseInteractor otherHand;

    private RaycastHit hit;

    private LayerMask layerMask;
    private LayerMask blueprintColliderMask;

    private int selectionStatus = 0;

    private bool updatePosition;

    private MeshFilter meshFilter;

    private GameObject heldFixture;
    private GameObject blueprintClone;

    private LineRenderer guideLine;
    private LineRenderer heightLine;

    private TextMeshPro heightIndicator;

    void ChangeAllMaterials(GameObject targetObject)
    {
        Material newMaterial = Resources.Load<Material>("Experimental/Blueprint");

        // Find all Renderer components on this object and all of its children
        Renderer[] renderers = targetObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
        {
            // Create a new array matching the size of the original materials array
            Material[] newMaterialsArray = new Material[rend.sharedMaterials.Length];

            // Fill the array with the new target material
            for (int i = 0; i < newMaterialsArray.Length; i++)
            {
                newMaterialsArray[i] = newMaterial;
            }

            // Assign the updated array back to the renderer
            rend.materials = newMaterialsArray;
        }
    }

    // Called before the first Update call
    void Start()
    {
        // Get layer mask to limit interactable surfaces (Roof and floor count as walls)
        layerMask = LayerMask.GetMask("Wall");
        blueprintColliderMask = LayerMask.GetMask("Fixture");

        // Get hammer's Grab Interactable component to later identify which hand is holding it
        thisHammer = gameObject.GetComponent<XRGrabInteractable>();

        // Get hammer's own LineRenderer component to enable guiding raycast
        guideLine = gameObject.GetComponent<LineRenderer>();

        // Get child HeightIndicator object LineRenderer and TextMeshPro for indicator updates
        heightLine = transform.Find("HeightIndicator").GetComponent<LineRenderer>();
        heightIndicator = transform.Find("HeightIndicator").GetComponent<TextMeshPro>();

        // Subscribe to function call on object placement to update tool status
        AnchorMagnet.OnPlaced += UpdateStatus;

        meshFilter = gameObject.GetComponent<MeshFilter>();
    }

    // Called upon hammer pickup
    // Identifies hand opposite of the hammer, enables guiding raycast and re-enables position updates if dropped mid use
    public void OnSelect()
    {
        Transform selectingInteractor = thisHammer.interactorsSelecting[0].transform;

        if (selectingInteractor.CompareTag("LeftHandInteractor"))
        {
            otherHand = GameObject.FindWithTag("RightHandInteractor").GetComponent<NearFarInteractor>();
        }

        else
        {
            otherHand = GameObject.FindWithTag("LeftHandInteractor").GetComponent<NearFarInteractor>();
        }

        if (selectionStatus == 1) {updatePosition = true;}

        guideLine.enabled = true;
    }

    // Called upon hammer drop
    // Disables guiding raycast and pauses position updates
    public void OnUnselect()
    {
        updatePosition = false;
        guideLine.enabled = false;
    }

    // Called upon hammer destruction
    // Destroys orphaned blueprint clone, if one existed at the time of destruction
    public void OnDisable()
    {
        if(blueprintClone != null) {Destroy(blueprintClone);}
    }

    // Called upon activation trigger pull
    // First press creates the wireframe clone and enables continuous position updates of it
    // Second press disables continuous position updates and enables wall anchor behavior
    // Third press destroys the active blueprint to enable the creation of a new one
    public void OnActivate()
    {
        switch (selectionStatus)
        {
            // Initial button press
            // Create the blueprint from a raycast and enable position updates
            // Will not progress into next state until a valid blueprint is created
            case 0:
                // Check for validity of object meant to receive a blueprint
                if (otherHand.hasSelection && otherHand.interactablesSelected[0].transform.CompareTag("Fixture"))
                {
                    heldFixture = otherHand.interactablesSelected[0].transform.gameObject;

                    // Fire a raycast to determine blueprint creation position
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5f, layerMask))
                    {
                        // Retrieve the held fixture's parent prefab, instantiate a copy and change its material to blueprint
                        // The object has its parent prefab instance assigned via editor
                        blueprintClone = Instantiate(heldFixture.GetComponent<ObjectAttributes>().blueprintInstance, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up));

                        ChangeAllMaterials(blueprintClone);

                        blueprintClone.tag = "Blueprint";

                        // Add relevant functionality components
                        SphereCollider blueprintCollider = blueprintClone.AddComponent<SphereCollider>();
                        blueprintCollider.enabled = false;
                        blueprintCollider.isTrigger = true;
                        blueprintCollider.includeLayers = blueprintColliderMask;
                        blueprintCollider.excludeLayers = ~blueprintColliderMask;

                        AnchorMagnet anchorScript = blueprintClone.AddComponent<AnchorMagnet>();
                        anchorScript.enabled = false;

                        ObjectAttributes blueprintAttributes = blueprintClone.AddComponent<ObjectAttributes>();
                        blueprintAttributes.AnchorID = "Blueprint";

                        // Enable position guides
                        heightLine.enabled = true;
                        heightIndicator.enabled = true;

                        // Enable continuous calls for adjustment function
                        updatePosition = true;

                        selectionStatus = 1;
                    }
                }
                break;

            // Second button press
            // Disable blueprint position updates, disable guides and enable collider
            case 1:
                blueprintClone.GetComponent<SphereCollider>().enabled = true;
                blueprintClone.GetComponent<AnchorMagnet>().enabled = true;

                heightLine.enabled = false;
                heightIndicator.enabled = false;

                updatePosition = false;

                selectionStatus = 2;
                break;

            // Third button press
            // Destroy wireframe clone to enable the creation of a new one
            case 2:
                Destroy(blueprintClone); 

                selectionStatus = 0;
                break;
        }
    }

    // Called every frame while adjusting is active, updates blueprint clone position
    public void AdjustWireFrame()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5f, layerMask))
        {
            // Update the object's position and rotation to look at the normal
            blueprintClone.transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(hit.normal, Vector3.up));

            // Get wireframe clone's lowest point in y for height reference
            Vector3 measurePoint = meshFilter.sharedMesh.bounds.min; 

            // Set the positions of LineRenderer points, one at measure point and other at floor
            // Slight displacement along the normal to avoid wall occlusion
            heightLine.SetPosition(0, measurePoint + (hit.normal * 0.1f) );
            heightLine.SetPosition(1, new Vector3(measurePoint.x, 0, measurePoint.z) + (hit.normal * 0.1f));

            // Update content of the Text object
            heightIndicator.text = measurePoint.y.ToString("0.00") + "m";
        }
    }

    // Called when an anchored object finishes placing. Updates tool status to zero
    public void UpdateStatus()
    {
        selectionStatus = 0;
    }

    void Update()
    {
        // If the guiding line is enabled, update its position every frame
        if (guideLine.enabled == true)
        {
            guideLine.SetPosition(0, transform.position);
            guideLine.SetPosition(1, transform.position + (transform.forward * 5f));
        }

        if (updatePosition)
        {
            AdjustWireFrame();
        }
    }
}