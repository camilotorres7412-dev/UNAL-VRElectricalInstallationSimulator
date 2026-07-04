using System;
using Unity.Mathematics;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities;

public class ButtonExit : MonoBehaviour
{
    // Store activation status
    private bool isActivated = false;

    // Store current value
    static float t = 0f;

    private float ypos;

    void Start()
    {
        ypos = transform.position.y;
    }

    // Called when activated by the player
    public void OnPress()
    {
        Debug.Log("Pressed!");
        // Initiate press animation
        isActivated = true;
    }

    void Update()
    {
        if(isActivated)
        {
        // Maximum y movement visually determined in editor
        transform.position = new Vector3(transform.position.x, ypos - (-Mathf.Abs(Time.deltaTime - 0.15f) + 0.15f), transform.position.z);

            // Trigger once the button has finished pressing
            if (t > 1.0f)
            {
                // Quits the application when in build
                Application.Quit();

                // Quits the application in editor for testing
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif

                Debug.Log("Closing Game");
            }
        }
    }
}
