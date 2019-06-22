using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCharMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private float range;
    private Rigidbody rb;
    private Vector3 direction;
    Vector3 originalPos;
    Vector3 constraintPos;
    bool oben;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        range = 1f;
        //originalPos = rb.Get;
        constraintPos = new Vector3(0, range, range * 1 / 3);
        oben = false;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * 1f, 1), transform.position.z);
    }

    void ChangePosition()
    {
        transform.position = direction;
        //Compute position for next time
        direction = new Vector3(0, Random.Range(-1, 1), 0);
    }
}
