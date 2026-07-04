using UnityEngine;
using System.Collections;

public class DoorObject : MonoBehaviour
{
    // “other” refers to the collider on the GameObject inside this trigger
    void OnTriggerEnter (Collider other)
    {
        Debug.Log ("A collider has entered the DoorObject trigger");
    }
}