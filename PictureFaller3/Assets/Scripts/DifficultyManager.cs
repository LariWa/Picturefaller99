using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve healthLoss;
    [SerializeField] private AnimationCurve fallSpeed;
    [SerializeField] private Vector2 controlSpeedMinMax;
    [Space]
    [SerializeField] private AnimationCurve objectSpawns; //first values for 2x2 pictures, last for 15x15
    [SerializeField] private AnimationCurve wallChunkDistance;
    [Space]
    [SerializeField] private AnimationCurve hpAmmount;
    [SerializeField] private AnimationCurve coinAmmount;
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float collectibleRange = 6;
    //[SerializeField] private int wallDistanceRandPlus = 5;
    [SerializeField] private int objRandMore = 5;
    [SerializeField] private int wallDistRandMore = 2;

    [Space]

    [SerializeField] private int startDim = 2;
    [SerializeField] private int maxDim = 15;
    [SerializeField] private List<int> leftoutDims;
    [SerializeField] private bool jumpBackInDim;
    private int jumpBackCounter;
    private int currDim;
    private PlayerMovement player;
    private List<GameObject> currCollectibles = new List<GameObject>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        updatePlayer();

        currDim = startDim;

        spawnHPandCoins();
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
        updatePlayer();

        currDim++;
        //skip some to get to speed/ relevant faster
        while (leftoutDims.Contains(currDim))
        {
            currDim++;
        }

        //if (jumpBackInDim && currDim > 7 && currDim % 2 == 0)



        if (currDim >= maxDim)
        {
            currDim = maxDim;
        }
        else
        {
            // repeat dimensions so can always try same sort again
            //2 4 6 7 6 7 8 9 8 9 10 11 10 11 12 13 12 13
            //(1 2 1 2 3 4 3 4 5 6 5 6 7 8)


            jumpBackCounter++;

            if (jumpBackCounter == 4)
            {
                jumpBackCounter = 0;
                currDim-=2;
            }
        }
    }


    private void updatePlayer()
    {
        // Calculate new controlls and set them

        float t = (float)currDim;
        t = t.Remap(startDim, maxDim, 0, 1);

        var fall = fallSpeed.Evaluate(t);
        var contr = t.Remap(startDim,maxDim, controlSpeedMinMax.x, controlSpeedMinMax.y);

        player.updateControlls(fall, contr);
    }

    public void spawnHPandCoins()
    {
        foreach(GameObject go in currCollectibles)
        {
            Destroy(go);
        }


        // Spawn hp and coins for how much is evaluated in curve between 0 and the picture wall


        //Get the position until wall where to spawn
        var wallZ = FindObjectOfType<ChunkManager>().getCurrPictureWall().transform.position.z;


        //Determine where at in float
        float t = (float)currDim;
        t = t.Remap(startDim, maxDim, 0, 1);

        //Get how many hp to spawn here
        var hp = hpAmmount.Evaluate(t);
        var am = Mathf.RoundToInt(hp);
        if (am <= 0) am = 0;

        for (int i = 0; i < am; i++)
        {
            var pos = Random.insideUnitSphere * collectibleRange;
            pos.z = Random.Range(10, wallZ);
            var h = Instantiate(healthPrefab, pos, healthPrefab.transform.rotation);
            currCollectibles.Add(h);
        }


        //Get how many coins to spawn here
        var coins = coinAmmount.Evaluate(t);
        am = Mathf.RoundToInt(coins);
        if (am <= 0) am = 0;

        for (int i = 0; i < am; i++)
        {
            var pos = Random.insideUnitSphere * collectibleRange;
            pos.z = Random.Range(10, wallZ);
            var c = Instantiate(coinPrefab, pos, coinPrefab.transform.rotation);
            currCollectibles.Add(c);
            //UnityEditor.PrefabUtility.UnpackPrefabInstance(c,UnityEditor.PrefabUnpackMode.Completely, UnityEditor.InteractionMode.AutomatedAction);
        }

    }


    public float getHealthLoss()
    {
        float t = (float)currDim;
        t = t.Remap(startDim, maxDim, 0, 1);

        return healthLoss.Evaluate(t);
    }
}


