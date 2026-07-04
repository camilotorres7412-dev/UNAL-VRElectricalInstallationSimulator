using System;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities;

public class ButtonExit : MonoBehaviour
{
    // Store activation status
    private bool isActivated = false;

    // Store fade to black renderer
    private Material matFade;

    void Start()
    {
        // Find and store black quad material for fadeout
        GameObject fadePlane = GameObject.Find("FadePlane");

        matFade = fadePlane.GetComponent<Renderer>().material;
    }

    // Called when activated by the player, initiates fade and closes app
    public void OnPress()
    {
        isActivated = true;
    }

    void Update()
    {
        if(isActivated)
        {
            // Get the current color and add to the color's alpha until fully opaque (Alpha = 1.0f)
            Color color = matFade.color;

            color.a += 0.05f; 

            matFade.color = color;

            // Quit the application after full fadeout
            if (color.a > 1f)
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