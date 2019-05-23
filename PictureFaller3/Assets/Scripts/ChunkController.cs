using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [SerializeField] private GameObject[] allObstacles;
    [SerializeField] private GameObject obstacleParent;
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

    public void spawnObstacles()
    {
        //Deactivate some predefines obstacles randomly
        for(int i = 0; i < obstacleParent.transform.childCount; i++)
            if(Random.Range(0,2) == 0) obstacleParent.transform.GetChild(i).gameObject.SetActive(false);



        //TODO: spawn additional obstacles in the air (test for space with hitscan and do patterns)
        var o = Instantiate(allObstacles[0], transform.position + (Random.insideUnitSphere * 5f), Quaternion.identity);
        o.transform.rotation = Quaternion.Euler(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360));
        o.transform.parent = obstacleParent.transform;
    }
}
