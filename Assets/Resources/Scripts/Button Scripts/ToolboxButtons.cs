using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Spawns tools or warps exsiting tools back to the toolbox
/// </summary>

public class ToolboxButtons : MonoBehaviour
{
    // Reference to spawn point selected in editor
    public Transform spawnPoint;

    public void SpawnObject(GameObject spawnTarget)
    {
        // Check if there is already an existing tool with the same tag
        // If there isn't, instantiate it
        if (GameObject.FindWithTag(spawnTarget.tag) == null)
        {
            Instantiate(spawnTarget, spawnPoint.position, Quaternion.identity);
        }

        // If there is, warp it back to spawn
        else
        {
            GameObject.FindWithTag(spawnTarget.tag).transform.position = spawnPoint.position;
        }
    }
}
