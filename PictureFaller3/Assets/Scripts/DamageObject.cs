using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var thing = other.transform.GetComponentInChildren<PlayerStats>();

        if (thing != null)
            thing.damagePlayer(damage);
    }
}
