using UnityEngine;
using System.Collections.Generic;
using System;

public class ElectricalAttributes : MonoBehaviour
{
    public static event Action OnElectricalUpdate;
    public List<GameObject> connections;
    public int Amperage;
    public bool powered = false;

    public void Signal(bool isPowered)
    {
        powered = isPowered;
    }
}
