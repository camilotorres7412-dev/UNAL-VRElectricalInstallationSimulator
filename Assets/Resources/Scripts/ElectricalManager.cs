using UnityEngine;
using System.Collections.Generic;

/// <summary>
///  Keeps track and manages connection state, symbolically imitating tracking of "cables" set up
/// </summary>
/// 

public class ElectricalManager : MonoBehaviour
{
    public static ElectricalManager Instance { get; private set; }

    private void Awake()
    {
        // Enforce the singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
    }


    // Struct used to store connection information
    [System.Serializable]
    public struct ConnectionEntry
    {
        public GameObject cableSource;
        public GameObject cableTarget;
        public int cableGauge;
        public bool cablePower;

        public ConnectionEntry(GameObject source, GameObject target, int gauge, bool power)
        {
                cableSource = source;
                cableTarget = target;
                cableGauge = gauge;
                cablePower = power;
        }
    }

    // Create a table of active connections
    public List<ConnectionEntry> connectionTable;

    // Add a connection entry to the connection table
    public void AddConnection(GameObject source, GameObject target, int gauge)
    {
        connectionTable.Add(new ConnectionEntry(source, target, gauge, false));
    }

    public void ToggleConnectionPower(GameObject source, bool power)
    {
        
    }
}
