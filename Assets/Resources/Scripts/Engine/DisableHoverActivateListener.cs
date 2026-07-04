using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Disables interactors' "Allow Activate on hover" to prevent unwanted behaviors on tools
/// Attach to tools which should not be activated from afar
/// </summary>

public class DisableHoverActivateListener : MonoBehaviour
{
    public NearFarInteractor leftHand;
    public NearFarInteractor rightHand;

    void OnEnable()
    {
        DisableHoverActivate.OnHoverEnter += DisableBehavior;
        DisableHoverActivate.OnHoverExit += EnableBehavior;
    }

    void OnDisable()
    {
        DisableHoverActivate.OnHoverEnter -= DisableBehavior;
        DisableHoverActivate.OnHoverExit += EnableBehavior;
    }

    void DisableBehavior()
    {
     leftHand.allowHoveredActivate = false;
     rightHand.allowHoveredActivate = false;   
    }

    void EnableBehavior()
    {
        leftHand.allowHoveredActivate = true;
        rightHand.allowHoveredActivate = true;   
    }
}
