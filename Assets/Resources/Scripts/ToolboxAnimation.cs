using Unity.Mathematics;
using UnityEngine;

public class ToolboxAnimation : MonoBehaviour
{
    private bool play;

    private Quaternion rest;

    private Quaternion start;

    private float timeCount = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rest = transform.rotation;
    }

    void OnTriggerEnter(Collider other) 
    {
        Debug.Log("Entered animation zone: " + other.gameObject.name);
        start = transform.rotation;
        timeCount = 0.0f;
        play = true;
    }

    void OnTriggerExit(Collider other) 
    {
        Debug.Log("Entered animation zone: " + other.gameObject.name);
        start = transform.rotation;
        timeCount = 0.0f;
        play = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(play)
        {
            // Spherically interpolate between start and target rotation
            transform.rotation = Quaternion.Slerp(start, start * Quaternion.Euler(-100f,0f,0f), timeCount);
            timeCount = timeCount + Time.deltaTime;
        }

        else
        {
            transform.rotation = Quaternion.Slerp(start, rest, timeCount);
            timeCount = timeCount + Time.deltaTime;
        }
    }
}
