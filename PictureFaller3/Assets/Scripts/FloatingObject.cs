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
    private Rigidbody rb;
    private float speed;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = Random.Range(floatSpeedMin, floatSpeedMax);
        if (doFloat) moveDir = Random.insideUnitSphere;
    }


    void FixedUpdate()
    {
        if (doFloat)
        {
            moveDir += Random.insideUnitSphere * floatDirChangeSpd;
            moveDir.Normalize();
            moveDir *= speed;

            //transform.position += moveDir * Time.deltaTime;
            //rb.AddForce(moveDir);
            rb.velocity = moveDir;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wind" || other.tag == "PictureSafeZone")
            Destroy(gameObject);   
    }
}
