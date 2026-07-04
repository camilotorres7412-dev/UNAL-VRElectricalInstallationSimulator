using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonRestart : MonoBehaviour
{
    // store activation status
    private bool isActivated = false;

    // Store fade to black renderer
    private Material matFade;

    void Start()
    {
        // Find and store black quad color for fadeout
        GameObject fadePlane = GameObject.Find("FadePlane");

        matFade = fadePlane.GetComponent<Renderer>().material;
    }

    // Called when activated by the player
    public void OnPress()
    {
        // Initiate press animation
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
                // Get build index and reload
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}