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
        var playerStats = other.transform.GetComponentInChildren<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.damagePlayer(damage);
            other.transform.GetComponentInChildren<PlayerMovement>().knockBack(transform.position);
            
            //Destroy(transform.parent.gameObject);
            GetComponent<Collider>().enabled = false;
        }

    }
}
