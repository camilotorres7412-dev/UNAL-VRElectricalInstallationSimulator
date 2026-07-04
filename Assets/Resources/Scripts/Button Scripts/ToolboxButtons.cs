using UnityEngine;

/// <summary>
/// Spawns tools or warps exsiting tools back to the toolbox
/// </summary>

public class ToolboxButtons : MonoBehaviour
{
    // Enables selecting the tool prefabs in-editor
    public GameObject cable;
    public GameObject hammer;
    public GameObject measuring_tape;
    public GameObject notepad;

    // Actual in-game gameobjects
    private GameObject cableInstance;
    private GameObject hammerInstance;
    private GameObject measuring_tapeInstance;
    private GameObject notepadInstance;

    // Various methods for spawning the different tools
    public void CablePress()
    {
        Instantiate(cable, new Vector3(4.329f,1.172f,0.525f), Quaternion.identity);
    }
    public void HammerPress()
    {
        if (hammerInstance is null)
        {
            hammerInstance = Instantiate(hammer, new Vector3(4.329f,1.172f,0.525f), Quaternion.identity);
        }

        else
        {
            hammerInstance.transform.position = new Vector3(4.329f,1.172f,0.525f);
        }
    }
    public void MeasuringTapePress()
    {
        Instantiate(measuring_tape, new Vector3(4.329f,1.172f,0.525f), Quaternion.identity);
    }
    public void NotepadPress()
    {
        Instantiate(notepad, new Vector3(4.329f,1.172f,0.525f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
