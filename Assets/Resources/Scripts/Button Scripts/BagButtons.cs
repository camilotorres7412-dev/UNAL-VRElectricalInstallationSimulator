using UnityEngine;

public class BagButtons : MonoBehaviour
{
    //  Declaration of fixture prefabs selected in-editor
    public GameObject breakerBox;
    public GameObject powerSocket;
    public GameObject breakerSwitch;
    public GameObject lightbulb;
    public GameObject powerSwitch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void BreakerBoxPress()
    {
        GameObject spawnedObject = Instantiate(breakerBox, new Vector3(0.6f,1.1f,2.3f), Quaternion.identity);

        spawnedObject.name = "Gabinete 1";
    }
    public void PowerSocketPress()
    {
        Instantiate(powerSocket, new Vector3(0.6f,1.1f,2.3f), Quaternion.identity);
    }
    public void BreakerSwitchPress()
    {
        Instantiate(breakerSwitch, new Vector3(0.6f,1.1f,2.3f), Quaternion.identity);
    }
    public void LightbulbPress()
    {
        Instantiate(lightbulb, new Vector3(0.6f,1.1f,2.3f), Quaternion.identity);
    }
        public void PowerSwitchPress()
    {
        Instantiate(powerSwitch, new Vector3(0.6f,1.1f,2.3f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
