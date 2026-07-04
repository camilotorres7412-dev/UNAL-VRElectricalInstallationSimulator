using UnityEngine;

public class HandleEnabler : MonoBehaviour
{
    // Get an instance of the playerHandler script to write held object
    public PlayerHandler playerHandler;

    // Indicates the player handler that this is the held object upon grab
    public void EnableHeld()
    {
        playerHandler.SetFixture(this.gameObject);
    }

    // Resets the held object upon release
    public void DisableHeld()
    {
        playerHandler.SetFixture(null);
    }
    
}
