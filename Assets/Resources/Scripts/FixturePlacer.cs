using UnityEditor;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using Unity.XR.CoreUtils;
using JetBrains.Annotations;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.UIElements;

/// <summary>
/// This script handles the creation of "Wall Anchors" depending on the held fixture
/// while the hammer is held on the other hand.
/// </summary>
public class FixturePlacer : MonoBehaviour

{
    // Get an instance of hands to check held object
    public XRBaseInteractor lHand;

    // Objects for height indicator text
    private GameObject textObject;
    private TextMeshPro heightIndicator;
    private LineRenderer lineRenderer;

    // Raycast hit point and wall only mask
    RaycastHit hit;

    LayerMask layerMask;

    // Stores the fixture held in the other hand
    private GameObject heldFixture;

    // Wireframe material for anchor preview and chalk for height markings
    public Material mtWireframe;
    public Material mtChalk;

    // Determine whether the player has activated the tool
    private bool hammerActivated;

    // Store created wireframe clone and wall anchor
    private GameObject wireframeClone;

    // Store attributes of held fixture
    private ObjectAttributes heldAttributes;

    void Start()
    {
        layerMask = LayerMask.GetMask("Wall");
    }

    // Method called upon trigger pull, creates the wireframe clone and height indicators
    public void HammerActivated()
    {

        // Check if the left hand is holding something
        if (lHand.interactablesSelected.Count > 0)
        {
            // Assign as held fixture
            heldFixture = lHand.interactablesSelected[0].transform.gameObject;
            heldAttributes = heldFixture.GetComponent<ObjectAttributes>();

            // Interrupt the function if it's not a fixture
            if (heldAttributes == null || heldAttributes.fixture == false)
            {
                return;
            }
        }
        
        // Shoot a raycast and activate this condition on hit
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            // Get normal rotation for instantiation of mesh rotated against surface
            Quaternion objRotation = Quaternion.LookRotation(hit.normal, Vector3.up);

            GameObject objPrefab = Resources.Load(heldFixture.name) as GameObject;
            wireframeClone = Instantiate(objPrefab, hit.point, objRotation);

            // Get object renderer and replace with blueprint
            Renderer rend = wireframeClone.GetComponent<MeshRenderer>();
            Renderer[] childRend = wireframeClone.GetComponentsInChildren<Renderer>();

            Material[] newMaterialsArray = new Material[rend.materials.Length];
            for (int i = 0; i < newMaterialsArray.Length; i++)
            {
                newMaterialsArray[i] = mtWireframe;
            }

            rend.materials = newMaterialsArray;

            rend.material = mtWireframe;

            foreach (Renderer r in childRend)
            {
                r.material = mtWireframe;
            }

            // Check if object height should be measured
            if (heldAttributes.measure == true)
            {
                textObject = new GameObject("HeightIndicator");

                // Text spawns with flipped alignment for some reason, so the minus fixes it
                textObject.transform.LookAt(-hit.normal);

                heightIndicator = textObject.AddComponent<TextMeshPro>();

                heightIndicator.text = "0m";
                heightIndicator.fontSize = 2;
                heightIndicator.color = Color.white;
                heightIndicator.alignment = TextAlignmentOptions.Center;

                // Create a line renderer for the height indicator
                lineRenderer = wireframeClone.AddComponent<LineRenderer>();

                // Set the material
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

                // Set the color
                lineRenderer.startColor = Color.white;
                lineRenderer.endColor = Color.gray;

                // Set the width
                lineRenderer.startWidth = 0.05f;
                lineRenderer.endWidth = 0.05f;

                // Set the number of vertices
                lineRenderer.positionCount = 2;

                // Force the line renderer to look at its local Z
                lineRenderer.alignment = LineAlignment.TransformZ;
        }

                hammerActivated = true;
        }
    }

    // Method called upon trigger release, interrupts adjusting behavior and creates a collider for anchoring
    public void HammerDeactivated()
    {
        // Check if the hammer had been successfully activated, skip otherwise
        if (hammerActivated == true)
        {
            hammerActivated = false;

            // Add a sphere collider to the wireframe clone for anchoring behavior
            wireframeClone.AddComponent<SphereCollider>();
            SphereCollider anchorCollider = wireframeClone.GetComponent<SphereCollider>();
            anchorCollider.isTrigger = true;
            anchorCollider.radius = 0.3f;

            // Add anchor handler script
            wireframeClone.AddComponent<AnchorMagnet>();
        }
    }

    // Method continously called while trigger is held, allows for adjusting the wireframe clone's height
    public void SetWireframe()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            wireframeClone.transform.position = new Vector3(wireframeClone.transform.position.x, hit.point.y, wireframeClone.transform.position.z);

            // Check if object height should be measured
            if (heldAttributes.measure == true)
            {
                // Set the positions of the vertices
                lineRenderer.SetPosition(0, new Vector3(wireframeClone.transform.position.x, wireframeClone.transform.position.y, wireframeClone.transform.position.z) + hit.normal * 0.001f);
                lineRenderer.SetPosition(1, new Vector3(wireframeClone.transform.position.x, 0, wireframeClone.transform.position.z) + hit.normal * 0.001f);

                // Update position of the Text object
                heightIndicator.transform.position = new Vector3(wireframeClone.transform.position.x, wireframeClone.transform.position.y / 2, wireframeClone.transform.position.z) + hit.normal * 0.001f;

                // Update content of the Text object
                heightIndicator.text = wireframeClone.transform.position.y.ToString("0.00") + "m";
            }
        }
    }

    void Update()
    {
        if (hammerActivated)
        {
            SetWireframe();
        }
    }
}