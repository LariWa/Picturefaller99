using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProjectileLogic : MonoBehaviour
{
    public GameObject hitParticle;
    public GameObject explosionParticle;
    public GameObject lifeStealParticle;
    private bool playerOwned;
    private float destroyRange;
    private Vector2 startPos;
    private int damage;
    private Rigidbody rb;
    private GameObject player;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Vector3.Distance(startPos, transform.position) >= destroyRange)
            StartCoroutine(killBullet());
    }


    private IEnumerator killBullet()
    {
        yield return new WaitForSeconds(0);

        //Play effects etc

        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider collision) //OnCollisionEnter
    {
        if (collision.gameObject.GetComponent<ProjectileLogic>() == null)  //No bullets hitting each other
        {
            if (collision.gameObject.GetComponent<PlayerMovement>() == null && playerOwned // If not the player iteself
             || collision.gameObject.GetComponentInChildren<EnemyStats>() == null && !playerOwned) // Or enemy hit itself
            {
                bool hitSomthingDamageable = true;

                //Check if the thing that was hit can take damage (or the children, in entity case) and do so.... NOT EFFICIENT ATM
                if(collision.GetComponent<IDamageable>() != null) collision.GetComponent<IDamageable>().takeDamage(damage);
                else if (collision.GetComponentInChildren<IDamageable>() != null) collision.GetComponentInChildren<IDamageable>().takeDamage(damage);
                else hitSomthingDamageable = false;


                StartCoroutine(killBullet());
            }
        }
    }

    
    public void setInfo(Vector2 start, float range, int dmg, bool owner)
    {

        startPos = start;
        destroyRange = range;
        damage = dmg;
        playerOwned = owner;
    }
}
