using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : EntityStatsMaster
{
    public float range = 4f;

    private Transform player;
    private Skillset skills;
    public GameObject explosionParticle;
    public Animator anim;

    //public Transform target;



    new void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        skills = GetComponent<Skillset>();
    }


    private void FixedUpdate()
    {
/*        if(Vector3.Distance(transform.position, player.position) < range)
        {
            skills.getPrimary().tryToDoSkill(player.position);
            anim.SetBool("shooting", true);
        }
        else anim.SetBool("shooting", false);*/
    }


    public override void takeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {

            //Play effects etc
            
            Destroy(transform.parent.gameObject);
 
        }

        if(healthVisuals) healthVisuals.showDamageText(damage, transform.position);
        if (healthVisuals) healthVisuals.setHealth(health);
    }

    public void setHPMAX(int hp)
    {
        healthMax = hp;
        health = healthMax;
        if (healthVisuals) healthVisuals.setMaxHP(healthMax);
        if (healthVisuals) healthVisuals.setHealth(health);
    }
}
