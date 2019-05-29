using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    private Rigidbody rb;
    

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();   
    }

    void FixedUpdate()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        var thing = other.transform.GetComponentInChildren<PlayerStats>();

        if (thing != null)
        {
            thing.damagePlayer(damage);
            other.transform.GetComponentInChildren<PlayerMovement>().knockBack(transform.position);
            Destroy(transform.parent.gameObject);
        }

    }
}
