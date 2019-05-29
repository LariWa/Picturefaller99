using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float normalGravity = 32f;
    private float gravity;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        gravity = normalGravity;

       rb.velocity = Vector3.down * gravity; 
    }

}
