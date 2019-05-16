using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallManager : MonoBehaviour
{
    [SerializeField] private Image picSearched; //Move somewhere else? UI
    private int currPicSearched;

    [SerializeField] private Sprite blackPicture;
    [SerializeField] private Sprite[] allPictures; // ammount needs to be squared so 4, 9, 16, 25 etc
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject pictureBlockPrefab;
    [SerializeField] private GameObject firePartPrefab;

    [Space]

    [SerializeField] private float likelyhoodNoPictures = 0.5f;
    [SerializeField] private float fairnessMultiplier = 0.2f;
    [SerializeField] private float pictureWallVeticalOffset = 50f;

    [SerializeField] private float pictureBlockScale = 1.5f;
    [SerializeField] private float gridGap = 2f;

    [SerializeField] private float likelyHoodOfEmpty = 0.1f;
    [SerializeField] private float minDistanceBetweenWalls = 100;
    [SerializeField] private float maxDistanceBetweenWalls = 100;

    private ObstacleManager laserManager;

    private GameObject currentPictureWall; //As soon as player hits current, current becoems next and vice versa, so they just swap each time (and move + change images?)
    private GameObject nextPictureWall;
    //private GameObject emptyWall;
    //private GameObject shapeWall;
    //private GameObject colorWall;

    private SpriteRenderer[] currentWallSprites;
    private SpriteRenderer[] nextWallSprites;

    private bool lastRandomResult;
    private int sameResultCounter;
    private float fairnesDir;
    private float baseLikelyNoPics;

    void Start()
    {
        baseLikelyNoPics = likelyhoodNoPictures;

        laserManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ObstacleManager>();

        //Two walls to use (pooling)
        initializeWall(out currentPictureWall, out currentWallSprites, 0);
        initializeWall(out nextPictureWall, out nextWallSprites, 1);
    }


    private void initializeWall(out GameObject wall, out SpriteRenderer[] wallSpr, int n)
    {
        bool hasPictures;
        float yOffsetDown;
        if (Random.Range(0f, 1f) < likelyhoodNoPictures + fairnesDir * fairnessMultiplier * sameResultCounter)
        {
            hasPictures = false;
            if(n == 0) yOffsetDown = Random.Range(minDistanceBetweenWalls, maxDistanceBetweenWalls);
            else yOffsetDown = Mathf.Abs(currentPictureWall.transform.position.y) + Random.Range(minDistanceBetweenWalls, maxDistanceBetweenWalls);
        }
        else
        {
            hasPictures = true;
            if (n == 0) yOffsetDown = pictureWallVeticalOffset;
            else yOffsetDown = Mathf.Abs(currentPictureWall.transform.position.y) + pictureWallVeticalOffset;
        }

        wall = Instantiate(wallPrefab, new Vector3(0, -yOffsetDown, 0), Quaternion.identity);

        GameObject imgParent = new GameObject("Image Parent");
        GameObject fireParent = new GameObject("Fire Parent");
        imgParent.transform.parent = wall.transform;
        fireParent.transform.parent = wall.transform;

        wall.GetComponent<WallController>().setFireParent(fireParent);

        int gridCells = Mathf.RoundToInt(Mathf.Sqrt(allPictures.Length));

        float maxDistHalf = ((gridCells-1) * gridGap) / 2; //Used to center images for even and uneven gridCells

        for (int y = gridCells-1; y >= 0; y--)
            for (int x = 0; x < gridCells; x++)
            {
                var box = Instantiate(pictureBlockPrefab, new Vector3(x * gridGap - maxDistHalf, -yOffsetDown, y * gridGap - maxDistHalf), Quaternion.identity);

                box.transform.parent = imgParent.transform;
                box.transform.localScale = new Vector3(pictureBlockScale, pictureBlockScale, pictureBlockScale);

                var fire = Instantiate(firePartPrefab, new Vector3(x * gridGap - maxDistHalf, -yOffsetDown - 30, y * gridGap - maxDistHalf), Quaternion.Euler(new Vector3(-90, 0, 0)));

                fire.transform.parent = fireParent.transform;
            }

        wallSpr = imgParent.GetComponentsInChildren<SpriteRenderer>();

        for(int i = 0; i < wallSpr.Length; i++)
            wallSpr[i].sprite = allPictures[i/*Random.Range(0, pictures.Length)*/];


       
        if (hasPictures)
        {
            wall.GetComponent<WallController>().setPictureModeOn();
            if (n == 0) currPicSearched = Random.Range(0, allPictures.Length - 1);
        }
        else
        {
            if (n == 0) laserManager.randomizeAndMove(-yOffsetDown, 0);
            if (n != 0) laserManager.randomizeAndMove(-yOffsetDown, currentPictureWall.transform.position.y);
            wall.GetComponent<WallController>().setPictureModeOff();
            if (n == 0) currPicSearched = -1;
        }


        if(currPicSearched != -1) picSearched.sprite = allPictures[currPicSearched];
        else picSearched.sprite = blackPicture;


        if (lastRandomResult == hasPictures)
        {
            fairnesDir = (hasPictures) ? 1 : -1;
            sameResultCounter++;
        }
        else
        {
            fairnesDir = 0;
            sameResultCounter = 0;
        }
        lastRandomResult = hasPictures;
    }


    public void hitWall()
    {
        StartCoroutine(moveWallDownAndPrepare());   
    }


    private IEnumerator moveWallDownAndPrepare() //Wait some seconds before destroying wall you just went through?  TODO: instead wait till player has passed certain ammount
    {
        yield return new WaitForSeconds(1.5f);

        //TODO: rerouting after some point to origin

        //TODO: dont move immedietly, but first fade the wall and wait till camera through wall


        bool hasPictures;

        //Set wall you just went through to something and move it down behind other one
        if (Random.Range(0f, 1f) < likelyhoodNoPictures + fairnesDir * fairnessMultiplier * sameResultCounter)
        {
            currentPictureWall.GetComponent<WallController>().setPictureModeOff();

            currentPictureWall.transform.position = nextPictureWall.transform.position + Vector3.down * Random.Range(minDistanceBetweenWalls, maxDistanceBetweenWalls);

            laserManager.randomizeAndMove(currentPictureWall.transform.position.y, nextPictureWall.transform.position.y);

            hasPictures = false;
        }
        else
        {
            currentPictureWall.GetComponent<WallController>().setPictureModeOn();

            currentPictureWall.transform.position = nextPictureWall.transform.position + Vector3.down * pictureWallVeticalOffset;

            hasPictures = true;
        }

        //next is now current
        var temp = currentPictureWall;
        currentPictureWall = nextPictureWall;
        nextPictureWall = temp;

        //If next wall has pictures generate a random one to search
        if (currentPictureWall.GetComponent<WallController>().getPictureMode())
            currPicSearched = Random.Range(0, allPictures.Length - 1);
        else
            currPicSearched = -1;

        if (currPicSearched == -1)
            picSearched.sprite = blackPicture;
        else
        {
            picSearched.sprite = allPictures[currPicSearched];
            currentPictureWall.GetComponent<WallController>().setEmptyFire(currPicSearched);
        }


        if (lastRandomResult == hasPictures)
        {
            fairnesDir = (hasPictures) ? 1 : -1;
            sameResultCounter++;
        }
        else
        {
            fairnesDir = 0;
            sameResultCounter = 0;
        }
        lastRandomResult = hasPictures;
    }



    public Vector3 getNextWallPos()
    {
        return currentPictureWall.transform.position;
    }
    public bool currentIsPictureWall()
    {
        return currentPictureWall.GetComponent<WallController>().getPictureMode();
    }
    public bool hitCorrectPicture()
    {
        var selectedPictureIndex = currentPictureWall.GetComponent<WallController>().getSelectedPicture();

        if (selectedPictureIndex == currPicSearched) return true;

        return false;
    }

    public GameObject getCurrentPictureWall()
    {
        return currentPictureWall;
    }




    public float getGridGap()
    {
        return gridGap;
    }

    public float getGridWidthAndHeight() //Always quadratic
    {
        return (Mathf.Sqrt(allPictures.Length) / 2) * gridGap;
    }
}
