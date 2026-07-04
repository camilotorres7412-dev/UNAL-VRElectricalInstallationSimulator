using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WallPlacement : MonoBehaviour
{
    [Header("Placement Settings")]
    public LayerMask wallLayerMask = -1; // Which layers count as walls
    public float placementDistance = 0.1f; // Distance from wall surface
    public float snapThreshold = 0.3f; // How close to wall before snapping
    public bool visualizeRaycast = false; // Debug visualization
    
    [Header("Audio/Visual Feedback")]
    public AudioClip snapSound;
    public GameObject placementIndicator; // Optional visual indicator
    
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private AudioSource audioSource;
    private bool isPlacedOnWall = false;
    private Vector3 originalScale;
    private Transform currentWall;
    
    // Placement state
    private Vector3 wallNormal;
    private Vector3 placementPoint;
    private bool canPlace = false;
    
    void Start()
    {
        // Get required components
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null && snapSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        originalScale = transform.localScale;
        
        // Subscribe to grab events
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
    }
    
    void Update()
    {
        // Only check for wall placement when object is being held
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            CheckWallPlacement();
        }
        
        // Update placement indicator
        UpdatePlacementIndicator();
    }
    
    void CheckWallPlacement()
    {
        canPlace = false;
        
        // Cast rays in multiple directions to find nearby walls
        Vector3[] directions = {
            transform.forward,
            -transform.forward,
            transform.right,
            -transform.right,
            transform.up,
            -transform.up
        };
        
        float closestDistance = float.MaxValue;
        RaycastHit closestHit = new RaycastHit();
        
        foreach (Vector3 direction in directions)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, snapThreshold, wallLayerMask))
            {
                // Check if this is closer than previous hits
                if (hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    closestHit = hit;
                    canPlace = true;
                }
                
                if (visualizeRaycast)
                {
                    Debug.DrawRay(transform.position, direction * hit.distance, Color.green);
                }
            }
            else if (visualizeRaycast)
            {
                Debug.DrawRay(transform.position, direction * snapThreshold, Color.red);
            }
        }
        
        if (canPlace)
        {
            wallNormal = closestHit.normal;
            placementPoint = closestHit.point + wallNormal * placementDistance;
            currentWall = closestHit.transform;
        }
    }
    
    void UpdatePlacementIndicator()
    {
        if (placementIndicator != null)
        {
            if (canPlace && grabInteractable != null && grabInteractable.isSelected)
            {
                placementIndicator.SetActive(true);
                placementIndicator.transform.position = placementPoint;
                placementIndicator.transform.rotation = Quaternion.LookRotation(-wallNormal);
            }
            else
            {
                placementIndicator.SetActive(false);
            }
        }
    }
    
    void OnGrabbed(SelectEnterEventArgs args)
    {
        // Remove from wall when grabbed
        if (isPlacedOnWall)
        {
            RemoveFromWall();
        }
    }
    
    void OnReleased(SelectExitEventArgs args)
    {
        // Try to place on wall when released
        if (canPlace)
        {
            PlaceOnWall();
        }
    }
    
    void PlaceOnWall()
    {
        if (!canPlace) return;
        
        // Position and orient the object
        transform.position = placementPoint;
        transform.rotation = Quaternion.LookRotation(-wallNormal);
        
        // Make object kinematic to prevent physics interference
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        
        // Parent to wall for stability (optional)
        if (currentWall != null)
        {
            transform.SetParent(currentWall);
        }
        
        isPlacedOnWall = true;
        
        // Play sound feedback
        if (audioSource != null && snapSound != null)
        {
            audioSource.PlayOneShot(snapSound);
        }
        
        // Disable grab interaction while placed
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
            // Re-enable after a short delay to allow immediate re-grabbing
            Invoke(nameof(ReEnableGrabbing), 0.1f);
        }
        
        Debug.Log($"Object placed on wall: {currentWall?.name}");
    }
    
    void ReEnableGrabbing()
    {
        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
        }
    }
    
    void RemoveFromWall()
    {
        if (!isPlacedOnWall) return;
        
        // Restore physics
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        
        // Unparent from wall
        transform.SetParent(null);
        
        // Restore original scale
        transform.localScale = originalScale;
        
        isPlacedOnWall = false;
        currentWall = null;
        
        Debug.Log("Object removed from wall");
    }
    
    // Public method to force remove from wall (useful for external scripts)
    public void ForceRemoveFromWall()
    {
        RemoveFromWall();
    }
    
    // Check if object is currently placed on a wall
    public bool IsPlacedOnWall()
    {
        return isPlacedOnWall;
    }
    
    // Get the current wall transform
    public Transform GetCurrentWall()
    {
        return currentWall;
    }
    
    void OnDestroy()
    {
        // Clean up event subscriptions
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }
    
    // Draw gizmos for debugging
    void OnDrawGizmosSelected()
    {
        if (canPlace)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(placementPoint, 0.05f);
            Gizmos.DrawRay(placementPoint, wallNormal * 0.2f);
        }
        
        if (isPlacedOnWall)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}