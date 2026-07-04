using UnityEngine;

public class BreakerBoxDoorlidScript : MonoBehaviour
{
    // Door opening speed
    public float rotationSpeed = 10.0f;
    private bool opening = false;
    private bool closing = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private GameObject stencilCutter;

    private GameObject breakerSlot;

    void Start()
    {
        closedRotation = Quaternion.identity;

        // Rotate -125° on the Y Axis (visually determined in editor)
        openRotation = Quaternion.Euler(0f, -125f, 0f);

        stencilCutter = transform.parent.Find("StencilCutter").gameObject;

        breakerSlot = transform.parent.Find("BreakerPos").gameObject;
    }

    // Called when activated by the player, opens doorlid, enables stencil cutter plane and enables the first breaker slot collider
    public void OnSelect()
    {
        if (opening == false && closing == false) 
        {
            opening = true;
            stencilCutter.GetComponent<MeshRenderer>().enabled = true;
        }

        else if (opening == true && closing == false)
        {
            opening = false;
            closing = true;
        }
    }

    void Update()
    {
        if (opening)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, openRotation, rotationSpeed * Time.deltaTime);

            if (transform.localRotation == openRotation)
            {
                opening = false;

                breakerSlot.GetComponent<BoxCollider>().enabled = true;
                
                breakerSlot.GetComponent<AnchorMagnet>().enabled = true;
            }
        }

        else if (closing)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, closedRotation, rotationSpeed * Time.deltaTime);

            if (transform.localRotation == closedRotation)
            {
                closing = false;

                stencilCutter.GetComponent<MeshRenderer>().enabled = false;

                breakerSlot.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
