using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// This script handles the creation of "Wall Anchors" depending on the held fixture
/// while the hammer is held on the other hand.
/// </summary>

public class HammerScript : MonoBehaviour

{
    // Reference to own interactable component to later get which hand is holding it
    private XRGrabInteractable thisHammer;

    // Get an instance of hand opposite to the one holding the hammer
    private XRBaseInteractor otherHand;

    // Raycast hit point
    RaycastHit hit;

    // Later assigned to "Wall" mask to limit interactions
    LayerMask layerMask;

    // Stores the fixture held in the other hand
    private GameObject heldFixture;

    // Determine whether the player has activated the tool
    private bool hammerActivated;

    // Store created wireframe clone and wall anchor
    private GameObject wireframeClone;

    // Visual guide LineRenderer
    private LineRenderer guideLine;

    // Object height LineRenderer
    private LineRenderer heightLine;

    // Object rotation with respect to raycast hit normal
    private Quaternion objRotation;

    // Get object's custom attributes
    private ObjectAttributes fixtureAttributes;

    // Get object's "bottom" for measuring
    private Transform measurePoint;

    // Store held fixture's text component for real-time value viewing
    private TextMeshPro heightIndicator;

    void Start()
    {
        // Get layer mask to limit interactable surfaces (Roof counts as a wall)
        layerMask = LayerMask.GetMask("Wall");

        // Get hammer's Grab Interactable component to later identify which hand is holding it
        thisHammer = gameObject.GetComponent<XRGrabInteractable>();

        // Get hammer's own LineRenderer component to enable guiding raycast
        guideLine = gameObject.GetComponent<LineRenderer>();
    }

    // Method called upon hammer pickup, identifies holding hand and enables guide
    public void HammerSelected()
    {
        // Get the transform component & tag of the holding hand
        Transform selectingInteractor = thisHammer.interactorsSelecting[0].transform;

        // If held by left hand, other hand is right, and vice versa
        if (selectingInteractor.CompareTag("LeftHandInteractor"))
        {
            otherHand = GameObject.FindWithTag("RightHandInteractor").GetComponent<NearFarInteractor>();
        }

        else
        {
            otherHand = GameObject.FindWithTag("LeftHandInteractor").GetComponent<NearFarInteractor>();
        }

        // Enable guiding raycast component and continuous update
        guideLine.enabled = true;
    }

    // Method called upon hammer drop, disables guide and destroys active wireframe clone
    public void HammerUnselected()
    {
        // Disable guiding raycast component and continuous update
        guideLine.enabled = false;

        // If there is an active blueprint, destroy it upon drop and disable position updates
        if (wireframeClone != null)
        {
            Destroy(wireframeClone); 
            hammerActivated = false;
        }
    }

    // Method called upon trigger pull, has two functions:
    // First press creates the wireframe clone and enables continuous position updates of it
    // Second press disables continuous position updates and enables wall anchor behavior
    // Third press re-enables wireframe clone position updating, and so on.
    public void HammerActivated()
    {
        // Check if the hammer had been successfully activated, skip otherwise
        if (hammerActivated == true)
        {
            hammerActivated = false;

            // Enable the wireframe clone's sphere collider
            wireframeClone.GetComponent<SphereCollider>().enabled = true;

            // Enable the wireframe clone's anchor magnet script
            wireframeClone.GetComponent<AnchorMagnet>().enabled = true;

            // Interrupt execution to prevent creating another instace
            return;
        }

        // If there is already an anchored wireframe clone, re-enable position updates
        // Also covers for the case where the object is dropped and another is picked up
        if (wireframeClone != null)
        {

            // Disable the wireframe clone's sphere collider
            wireframeClone.GetComponent<SphereCollider>().enabled = false;

            // Disable the wireframe clone's anchor magnet script
            wireframeClone.GetComponent<AnchorMagnet>().enabled = false;

            // Re-enable position updates every frame
            hammerActivated = true;

            // Interrupt execution to prevent creating another instance
            return;
        }

        // Check if the free hand is holding an object and that object is a fixture
        if (otherHand.hasSelection && otherHand.interactablesSelected[0].transform.CompareTag("Fixture"))
        {
            // Assign object as held fixture
            heldFixture = otherHand.interactablesSelected[0].transform.gameObject;

            // Get held fixture's custom attributes to determine measuring direction
            fixtureAttributes = heldFixture.GetComponent<ObjectAttributes>();

            // Shoot a raycast from the hammer's origin that only detects walls
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5f, layerMask))
            {
                // Get normal rotation of hit point for instantiation of wireframe clone rotated against surface
                objRotation = Quaternion.LookRotation(hit.normal, Vector3.up);

                // Get object's "identifier" field to create wireframe clone mesh. Mind exact name matching!
                GameObject objPrefab = Resources.Load<GameObject>("Blueprints/" + fixtureAttributes.identifier);

                // Instantiate new wireframe clone from identified mesh with special visuals.
                wireframeClone = Instantiate(objPrefab, hit.point, objRotation);

                // Get wireframe clone's measure point to align initial measurement
                measurePoint = wireframeClone.transform.Find("MeasurePoint");

                // Get wireframe clone's LineRenderer component to update the visual measurement line
                heightLine = wireframeClone.GetComponent<LineRenderer>();

                // Get wireframe clone's TextMeshPro component to update the visual measure value
                heightIndicator = wireframeClone.transform.Find("HeightIndicator").GetComponent<TextMeshPro>();

                // Enable continuous calls for adjustment function
                hammerActivated = true;
            }

        }
    }

    // Method called every frame while the trigger is held, updates wireframe clone position
    public void AdjustWireFrame()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            // Update the object's position to the hit point
            wireframeClone.transform.position = hit.point;

            // Update the object's rotation to look at the normal
            objRotation = Quaternion.LookRotation(hit.normal, Vector3.up);
            wireframeClone.transform.rotation = objRotation;

            // Check object's measuring orientation and adjust guiding lines accordingly
            if (fixtureAttributes.measureV == true)
            {
                // Set the positions of LineRenderer points, one at measure point and other at floor
                heightLine.SetPosition(0, measurePoint.position);
                heightLine.SetPosition(1, new Vector3(measurePoint.position.x, 0, measurePoint.position.z));

                // Update content of the Text object
                heightIndicator.text = measurePoint.position.y.ToString("0.00") + "m";
                Debug.Log("Measurepoint Height: " + measurePoint.position.y.ToString("0.00") + "m");
            }
        }
    }

    void Update()
    {
        // If the guiding line is enabled, update its position every frame
        if (guideLine.enabled == true)
        {
            guideLine.SetPosition(0, transform.position);
            guideLine.SetPosition(1, transform.position + (transform.forward * 5f));
        }

        if (hammerActivated)
        {
            AdjustWireFrame();
        }
    }
}