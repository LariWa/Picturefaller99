using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private bool doFloat;
    [SerializeField] private float floatSpeedMax = 0.5f;
    [SerializeField] private float floatSpeedMin = 1.5f;
    [SerializeField] private float floatDirChangeSpd = 0.1f; // 0 means dont change initial float dir

    private Vector3 moveDir; //offset from my origin to move to
    
    void Start()
    {
        if (doFloat) moveDir = Random.insideUnitSphere * Random.Range(floatSpeedMin, floatSpeedMax);
    }


    void FixedUpdate()
    {
        if (doFloat)
        {
            moveDir += Random.insideUnitSphere * floatDirChangeSpd;
            moveDir.Normalize();

            transform.position += moveDir * Time.deltaTime;
        }
    }
}
