using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepAnimation : MonoBehaviour
{
    private float finalPositionRotation;
    private float finalPositionTranslation;

    private Vector3 goalPosition;
    private Vector3 goalRotation;
    // Start is called before the first frame update
    void Start()
    {
        goalPosition = new Vector3(0.064f, -0.717f, -0.649f);
        goalRotation = new Vector3(-73.11f, 86.805f, 3.058f);
    }

    // Update is called once per frame
    void Update()
    {
        //Animate the player: position and rotation
        Debug.Log(transform.position);
        transform.position = Vector3.MoveTowards(transform.position, goalPosition, 1f*Time.deltaTime);
        Vector3 NewDir = Vector3.RotateTowards(transform.position, goalRotation, 1f*Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(NewDir);
    }
}
