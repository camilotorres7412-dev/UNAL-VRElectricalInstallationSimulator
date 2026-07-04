using UnityEngine;
using System;

public class DisableHoverActivate : MonoBehaviour
{
    public static event Action OnHoverEnter;
    public static event Action OnHoverExit;

    // Associated via inspector to XR Grab Interactable - On Hover
    public void OnHoverEntered()
    {
        OnHoverEnter?.Invoke();
    }

    // Associated via inspector to XR Grab Interactable - On Hover
    public void OnHoverExited()
    {
        OnHoverExit?.Invoke();
    }
}
