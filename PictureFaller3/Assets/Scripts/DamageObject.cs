using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private bool doFall;
    [SerializeField] private float gravity = 4;
    private Rigidbody rb;
    

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();   
    }

    void FixedUpdate()
    {
        if(doFall) rb.velocity = Vector3.down * gravity;
    }

    private void OnTriggerEnter(Collider other)
    {
        var thing = other.transform.GetComponentInChildren<PlayerStats>();

        if (thing != null)
            thing.damagePlayer(damage);
    }
}
