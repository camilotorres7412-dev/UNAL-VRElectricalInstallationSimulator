using UnityEngine;

public class ToolboxButtons : MonoBehaviour
{
    // Enables selecting the tool prefabs in-editor
    public GameObject cable;
    public GameObject hammer;
    public GameObject measuring_tape;
    public GameObject notepad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
        // Various methods for spawning the different tools
    public void CablePress()
    {
        Instantiate(cable, new Vector3(4.329f,1.172f,0.525f), Quaternion.identity);
    }
    public void HammerPress()
    {
        Instantiate(hammer, new Vector3(4.329f,1.172f,0.525f), Quaternion.identity);
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
