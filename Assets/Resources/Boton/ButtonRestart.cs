using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonRestart : MonoBehaviour
{
    // store activation status
    private bool isActivated = false;

    // store current value
    static float t = 0;

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
        // Maximum y movement visually determined in editor
        transform.position = new Vector3(0, Mathf.Lerp(0, -0.045f, t), 0);

        // Increase t based on time
        t += 0.5f * Time.deltaTime;

        // Trigger once the button has finished pressing
        if (t > 1.0f)
        {
            // Get build index and reload
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        }
    }
}