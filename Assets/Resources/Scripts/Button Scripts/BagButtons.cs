using System.Collections.Generic;
using UnityEngine;

public class SpawnButtons : MonoBehaviour
{
    // Reference to spawn point selected in editor
    [SerializeField] private Transform spawnPoint;
    
    // Keep a list of instance names for incremental naming
    private List<string> nameList;

    // Called by item spawning buttons when they are pressed
    // The spawnTarget parameter is passed via Inspector in each button's function call
    public void SpawnObject(GameObject spawnTarget)
    {
        // Spawn GameObject from seleced prefab in a neutral position at selected spawn point
        GameObject spawnedObject = Instantiate(spawnTarget, spawnPoint.position, Quaternion.identity);

        spawnedObject.name = spawnTarget.GetComponent<ObjectAttributes>().displayName;
    }
}
