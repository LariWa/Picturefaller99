using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EntityStatsMaster : MonoBehaviour, IDamageable
{
    public int healthMax;
    protected int health;
    protected HealthAndDamageVisuals healthVisuals;

    private void Awake()
    {
        if(transform.parent.GetComponentInChildren<HealthAndDamageVisuals>()) healthVisuals = transform.parent.GetComponentInChildren<HealthAndDamageVisuals>();
    }

    protected void Start()
    {
        
        health = healthMax;
        if(healthVisuals) healthVisuals.setMaxHP(healthMax);
        if(healthVisuals) healthVisuals.setHealth(health);
    }


    protected void Update()
    {
        //Movement shit etc?
    }

    public abstract void takeDamage(int damage);
}
