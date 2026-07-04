using UnityEngine;

public class SpawnButtons : MonoBehaviour
{
    // Reference to spawn point selected in editor
    public Transform spawnPoint;

    // Called by item spawning buttons when they are pressed
    // The spawnTarget parameter is passed via Inspector in each button's function call
    public void SpawnObject(GameObject spawnTarget)
    {
        GameObject spawnedObject = Instantiate(spawnTarget, spawnPoint.position, Quaternion.identity);

        // If no object with this name can be found on the scene, name it as the "first" 
        if (GameObject.Find(spawnedObject.GetComponent<ObjectAttributes>().displayName) == null)
        {
            spawnedObject.name = spawnedObject.GetComponent<ObjectAttributes>().displayName + " 1";
        }

        else
        {
            int nameCounter = 2;

            // Increment name counter for every valid object with the same name found on the scene
            while (GameObject.Find(spawnedObject.GetComponent<ObjectAttributes>().displayName + nameCounter) != null)
            {
                nameCounter += 1;
            }

            spawnedObject.name = spawnedObject.GetComponent<ObjectAttributes>().displayName + nameCounter;
        }
    }
}
