using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [SerializeField] private GameObject[] allObstacles;
    [SerializeField] private GameObject obstacleParent;
    [SerializeField] private float playAreaSpawnObjWidth = 10;
    //[SerializeField] private int randObstacAmmount = 30;
    //[SerializeField] private int randomMoreOrLessObstac = 15;
    private enum obstaclePattern { circle, verticalLines, random };


    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void disableAllObstacles()
    {
        for (int i = 0; i < obstacleParent.transform.childCount; i++)
            obstacleParent.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void spawnObstacles(int flatObstacles, int randMore)
    {
        //Deactivate some predefines obstacles randomly
        for(int i = 0; i < obstacleParent.transform.childCount; i++)
            if(Random.Range(0,2) == 0) obstacleParent.transform.GetChild(i).gameObject.SetActive(false);



        // Spawn additional obstacles in the air 
        //var obstacleAm = /*randObstacAmmount*/ flatObstacles + Random.Range(-randMoreOrLess, randMoreOrLess);
        var obstacleAm = flatObstacles + Random.Range(0, randMore);

        for (int i = 0; i < obstacleAm; i++)
        {
            var o = Instantiate(allObstacles[Random.Range(0, allObstacles.Length)],
                transform.position + new Vector3(Random.Range(-playAreaSpawnObjWidth, 
                            playAreaSpawnObjWidth),
                            Random.Range(-playAreaSpawnObjWidth,
                            playAreaSpawnObjWidth),
                            Random.Range(-50, 50)),
                Quaternion.identity);
            o.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            o.transform.parent = obstacleParent.transform;
        }

        //TODO: not perfect, sometimes empty spaces? do patterns, check and prevent overlaps (maybe buildings new collider then)
    }
}
