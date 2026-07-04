using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using UnityEditor.UI;

// Currently unused. Eventually add.

[System.Serializable]
public struct ConnectionEntry
{
    public GameObject cableTarget;
    public int cableGauge;
    public bool cablePower;

    public ConnectionEntry(GameObject target, int gauge, bool power)
    {
        cableTarget = target;
        cableGauge = gauge;
        cablePower = power;
    }
}

public class ElectricalAttributes : MonoBehaviour
{
    public bool devicePower = false;

    public bool electricPower = false;

    // public List<ConnectionEntry> connnectionTable;

    public List<GameObject> cableTargets;

    // 120 Colombia
    public int voltageLine;

    public int voltageCapacity;

    public int voltageLoad;

    public void PowerAllNeighbors(bool isPowered)
    {
        foreach (GameObject connection in cableTargets)
        {
            connection.GetComponent<ElectricalAttributes>().electricPower = isPowered;
        }
    }
    
    public void PowerBulbs(bool isPowered)
    {
        // Loop over connection list
        foreach (GameObject connection in GetComponent<ElectricalAttributes>().cableTargets)
        {
            // Check each individual connection, is it a light?
            if (connection.GetComponent<ObjectAttributes>().displayName == "Ojo de Buey")
            {
                // If it is, toggle its energy and light
                connection.GetComponent<Light>().enabled = isPowered;
                connection.GetComponent<ElectricalAttributes>().electricPower = isPowered;
            }
        }
    }
}