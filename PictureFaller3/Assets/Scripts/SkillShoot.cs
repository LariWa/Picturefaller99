using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillShoot : SkillMaster
{

    public GameObject projectilePrefab;
    public Transform firePoint;

    public int bulletsPerShot = 1;          //for testing make a single shot, a shotgun shot, and a 3 brust salve ON BUTTON DOWN
    public int maxDelayBetweenBullets;
    public float bulletLikelyDelay; //0.0 to 1.0
    public float accuracy;
    public float speed = 20;
    public float range = 4;
    public float fireOffsetFromPlayer = 0.5f;
    public bool doesPierce;
    public int bounceAm;
    public int splitAm;
    public Vector3 scale = new Vector3(1,1,1);


    [Space]

    //public WeaponSO weapon;
    //private ScreenShakeTest shake;



    private PlayerStats playerLogic;
    private Skillset skillSet;

    public int pooledAmount = 10;
    public bool willGrow = true;
    List<GameObject> bullets;
    // FIND A WAY to do precise skills too, so one that has not jsut a 0-10 accuracy but one that always fires two outter shots and one big in the middle, each go different speed to !!! somehow make a list of this class? 

    private void Start()
    {
        if (GetComponent<Skillset>()) skillSet = GetComponent<Skillset>();
        if (GetComponent<PlayerStats>()) playerLogic = GetComponent<PlayerStats>();
        //shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<ScreenShakeTest>();//shake = Camera.main.GetComponent<ScreenShakeTest>();

        //pool Objects
        bullets = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            obj.SetActive(false);
            bullets.Add(obj);
        }
    }


    public override void doSkill(Vector3 mousePos)
    {

        StartCoroutine(shoot(mousePos));

    }



    public override int getInputSlot()
    {
        if (skillSet == null) return 1; //enemy case

        if (skillSet.getPrimary() == this) return 1;
        if (skillSet.getSecondary() == this) return 2;
        if (skillSet.getMovement() == this) return 3;

        return -999999;
    }


    IEnumerator shoot(Vector3 shootDir)
    {

        for (int i = 0; i < bulletsPerShot; i++)
        {

            //Spawn
            //var bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            var bullet = getBullet();
            bool owner = false;
            //bullet.transform.position = firePoint.position;
            //bullet.transform.rotation = Quaternion.identity;
            //bullet.transform.localScale = scale;

            if (GetComponent<PlayerStats>() != null) owner = true;
            bullet.GetComponent<ProjectileLogic>().setInfo(firePoint.position, range, averageDamage + Random.Range(-averageDamage / 2, averageDamage / 2), owner);

          

            //Look at mouse
            Vector3 diff = shootDir - bullet.transform.position;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + Random.Range(-accuracy, accuracy));
            if(owner) bullet.transform.position += bullet.transform.right * fireOffsetFromPlayer;

            if (owner)
            {
                //if(skillCostPercent == 0) shake.addShake(-bullet.transform.right, 0.4f);
                //else shake.addShake(Random.insideUnitCircle, 0.3f);
            }

            //Shoot
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.right * speed, ForceMode.Impulse);

            //if (maxDelayBetweenBullets > 0) yield return new WaitForSeconds(Random.Range(0, maxDelayBetweenBullets)); //problem is, not accurate at all, even WaitForSeconds(0) makes a delay, so just gemerate rand number and sometimes wait for a frame

            float rand = Random.Range(0f, 1f);
            if(rand <= bulletLikelyDelay) yield return new WaitForSeconds(0);

            
        }

    }
    GameObject getBullet()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
                bullets[i].SetActive(true);
                return bullets[i];

        }
        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            bullets.Add(obj);
        }
        return null;
    }
}