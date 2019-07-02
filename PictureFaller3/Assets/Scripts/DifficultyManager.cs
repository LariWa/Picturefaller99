using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve objectSpawns; //first values for 2x2 pictures, last for 15x15
    [SerializeField] private AnimationCurve wallChunkDistance;
    //[SerializeField] private int wallDistanceRandPlus = 5;
    [SerializeField] private int objRandMore = 5;
    [SerializeField] private int wallDistRandMore = 2;

    [Space]

    [SerializeField] private int startDim = 2;
    [SerializeField] private int maxDim = 15;
    private int currDim;

    void Start()
    {
        currDim = startDim;
    }


    void Update()
    {
        
    }



    // difficulty may be a bit off because only the ones that will spawn will have more, not the ones immedietly infront of you
    public int getObstacDifficulty()
    {
        //Determine where at in float
        float t = (float)currDim;
        t = t.Remap(startDim, maxDim, 0, 1);

        //Get how many obstacles to spawn here
        var objs = objectSpawns.Evaluate(t);

        var am = Mathf.RoundToInt(objs);

        if (am <= 0) am = 0;

        return am;
    }

    public int getWallChunkOffset()
    {
        //Determine where at in float
        float t = (float)currDim;
        t = t.Remap(startDim, maxDim, 0, 1);

        //Get how many obstacles to spawn here
        var offset = wallChunkDistance.Evaluate(t);

        //offset += Random.Range(0, offset/2f);

        return Mathf.RoundToInt(offset) + wallDistRandMore;
    }


    public int getObstacRand()
    {
        return objRandMore;
    }

    public int getDim()
    {
        return currDim;
    }


    public void hitWall()
    {
        currDim++;
        if (currDim >= maxDim) currDim = maxDim;
    }
}

