using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject laserPrefab;
    public float maxOnScreen = 20;
    public float randomNess = 10;
    public float laserSafeZoneTop = 20f;
    public float laserSafeZoneLow = 10f;

    private Transform laserParentA;
    private Transform laserParentB;
    private bool parent;

    private GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        laserParentA = new GameObject("Laser Parent A").transform;
        laserParentB = new GameObject("Laser Parent B").transform;
        for (int i = 0; i < maxOnScreen; i++)
            initializeLasers(laserParentA);
        for (int i = 0; i < maxOnScreen; i++)
            initializeLasers(laserParentB);
    }

    private void initializeLasers(Transform par)
    {
        var laser = Instantiate(laserPrefab, new Vector3(Random.Range(-20, 20), 10000, Random.Range(-20, 20)), Quaternion.identity);
        laser.transform.parent = par;
        laser.transform.eulerAngles = new Vector3(Random.Range(-20, 20), Random.Range(0,360),0);
    }

    public void randomizeAndMove(float lowestY, float highestY)
    {
        var par = laserParentA;
        if (parent)
        {
            par = laserParentB;
            parent = false;
        }
        else
        {
            par = laserParentA;
            parent = true;
        }

        lowestY += laserSafeZoneLow;
        highestY -= laserSafeZoneTop;

        for (int i = 0; i < par.transform.childCount; i++)
            par.GetChild(i).gameObject.SetActive(false);

        var am = Random.Range(0, randomNess);
        for (int i = 0; i < par.transform.childCount - am; i++)
        {
            par.GetChild(i).gameObject.SetActive(true);
            par.GetChild(i).transform.position = new Vector3(Random.Range(-20, 20), Random.Range(lowestY, highestY), Random.Range(-20, 20));
            par.GetChild(i).transform.eulerAngles = new Vector3(Random.Range(-20, 20), Random.Range(0, 360), 0);
        }
    }
}
