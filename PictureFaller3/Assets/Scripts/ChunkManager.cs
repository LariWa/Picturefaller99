using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    //[SerializeField] private int chunksUntilPictureMin = 4;
    //[SerializeField] private int chunksUntilPictureMax = 8;
    [SerializeField] private float chunkLength = 100f;
    [SerializeField] private int spawnAhead = 3;
    //[SerializeField] private GameObject[] allChunks;
    [SerializeField] private GameObject pictureWallPre;

    private ObstacleManager obstacleManager;

    private DifficultyManager difficultyManager;
    private PictureManager pictureManager;
    private SettingManager settingManager;
    private Transform player;
    private GameObject currentPictureWall;
    private GameObject chunkParent;
    private float zSpawnNext;

    void Start()
    {
        difficultyManager = GetComponent<DifficultyManager>();
        settingManager = GetComponent<SettingManager>();
        obstacleManager = GetComponent<ObstacleManager>();
        pictureManager = GetComponent<PictureManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;


        //Setup chunk parent + spawn first few chunks + picturewall
        chunkParent = new GameObject("Environment Chunks");

        setupChunksAndWall();
    }


    void Update()
    {
        if (player.transform.position.z > zSpawnNext - (spawnAhead * chunkLength))
            spawnChunk(true, true);
    }


    private void spawnChunk(bool spawnObstacles, bool deleteLastChunk)
    {
        var ch = Instantiate(settingManager.getRandomChunkCurrSetting(), Vector3.forward * zSpawnNext, Quaternion.identity);
        ch.transform.parent = chunkParent.transform;
        if(spawnObstacles) ch.GetComponent<ChunkController>().spawnObstacles(difficultyManager.getObstacDifficulty(), difficultyManager.getObstacRand());
        else ch.GetComponent<ChunkController>().disableAllObstacles();

        zSpawnNext += chunkLength;
        
        //if(currentPictureWall) currentPictureWall.GetComponent<WallController>().deleteNearObstacles();


        //TODO: instead of deleting use pooling (or maybe not?)
        if (deleteLastChunk) Destroy(chunkParent.transform.GetChild(0).gameObject);
    }


    private void spawnPicWall()
    {
        //var chunksUntilPicture = Random.Range(chunksUntilPictureMin, chunksUntilPictureMax);
        var chunksUntilPicture = difficultyManager.getWallChunkOffset();
        currentPictureWall = Instantiate(pictureWallPre, Vector3.forward * chunksUntilPicture * chunkLength, Quaternion.Euler(-90, 0, 0));  //TODO: probably not accurate pos !!!

        //currentPictureWall.GetComponent<WallController>().deleteNearObstacles();
    }

    public void spawnPicWallOffsetFromLast()
    {
        var oldPos = currentPictureWall.transform.position;

        Destroy(currentPictureWall);

        //var chunksUntilPicture = Random.Range(chunksUntilPictureMin, chunksUntilPictureMax);
        var chunksUntilPicture = difficultyManager.getWallChunkOffset();
        currentPictureWall = Instantiate(pictureWallPre, Vector3.forward * chunksUntilPicture * chunkLength + oldPos, Quaternion.Euler(-90, 0, 0));  //TODO: probably not accurate pos !!!

        //currentPictureWall.GetComponent<WallController>().deleteNearObstacles();
    }




    public void resetChunksAndWall()
    {
        Destroy(currentPictureWall);

        for (int i = 0; i < chunkParent.transform.childCount; i++)
            Destroy(chunkParent.transform.GetChild(i).gameObject);

        zSpawnNext = 0;

        setupChunksAndWall();

        
        //spawnPicWall();
    }



    private void setupChunksAndWall()
    {
        spawnChunk(false, false);

        for (int i = 0; i < spawnAhead; i++)
            spawnChunk(true, false);

        spawnPicWall();
    }




    public Vector3 getSelectSquarePos()
    {
        return currentPictureWall.GetComponent<WallController>().getSelectSquarePos();
    }

    public void setSelectSquarePos(Vector3 pos)
    {
        currentPictureWall.GetComponent<WallController>().setSelectSquarePos(pos);
    }

    public GameObject getCurrPictureWall()
    {
        return currentPictureWall;
    }
}
