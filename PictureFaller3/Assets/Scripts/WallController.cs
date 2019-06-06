using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [SerializeField] private Sprite blackPicture;
    private Sprite[] allPictures; // ammount needs to be squared so 4, 9, 16, 25 etc
    [SerializeField] private GameObject pictureBlockPrefab;
    [SerializeField] private GameObject pictureBlockPrefabBigWall;

    [SerializeField] private float bigFrameScaleMulti = 1.25f;
    [SerializeField] private float totalPicDimMin = 5f; //Whole space pics will take up at start (dim 2x2)
    [SerializeField] private float totalPicDimMax = 10f; //Whole space pics will take up at end (dim 15x15)
    [SerializeField] private float picGapsMin = 0.5f;
    [SerializeField] private float picGapsMax = 0.05f;
    [SerializeField] private int oneBigFrameFromDim = 5; //put all pictures tight together to calculate data and put frame around
    //[SerializeField] private float pictureBlockScale = 1.2f;
    //[SerializeField] private float gridGap = 1.25f;
    private float gridGap;
    private float pictureBlockScale;
    [SerializeField] private float delteObstaclesRadius = 40f;

    [Space]

    [SerializeField] private GameObject selectingSquare;
    [SerializeField] private float selectionScaleMult = 0.7f;
    [SerializeField] private float selectionZoffsetMult = 0.5f;
    [SerializeField] private float selectingSpeed = 0.1f;
    [SerializeField] private float selectingDelay = 0.2f;

    private Vector2Int selectedPos; //intern array position of selection
    private Coroutine[] accelerationCoroutines = new Coroutine[8];

    private PlayerMovement player;
    private PictureManager wallManager;

    private Vector3 playerStartOffset = Vector3.zero;

    private bool mouseSelection;

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        wallManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<PictureManager>();

        var settingManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<SettingManager>();
        settingManager.randomSortForSetting(settingManager.getNextSetting());
        allPictures = settingManager.getAllPicturesInSort(settingManager.getNextSetting());

        mouseSelection = player.mouseSelection;

        if (mouseSelection)
        {
            //Hide mouse or put mouse sprite (crosshair)
        }


        // ------ Init ---------
        GameObject imgParent = new GameObject("Image Parent");
        imgParent.transform.parent = transform;

        int gridWidth = Mathf.RoundToInt(Mathf.Sqrt(allPictures.Length));
        float floatWidth = (float)gridWidth;


        var totalPicDim = floatWidth.Remap(2, 15, totalPicDimMin, totalPicDimMax);
        gridGap = totalPicDim / gridWidth;
        var picGaps = floatWidth.Remap(2,15, picGapsMin, picGapsMax);

        if (gridWidth >= oneBigFrameFromDim) picGaps = 0;

         pictureBlockScale = gridGap - picGaps;

        float maxDistHalf = ((gridWidth - 1) * gridGap) / 2; //Used to center images for even and uneven gridCells

        for (int y = gridWidth - 1; y >= 0; y--)
            for (int x = 0; x < gridWidth; x++)
            {
                GameObject frame;

                if (gridWidth >= oneBigFrameFromDim) // One big pic
                {
                    frame = Instantiate(pictureBlockPrefabBigWall, new Vector3(x * gridGap - maxDistHalf, y * gridGap - maxDistHalf, transform.position.z), Quaternion.Euler(-90, 0, 0));

                    if(x == 0 && y == 0)
                    {
                        var bigframe = Instantiate(pictureBlockPrefab, new Vector3(x * gridGap - maxDistHalf, y * gridGap - maxDistHalf, transform.position.z), Quaternion.Euler(-90, 0, 0));
                        Destroy(bigframe.transform.GetChild(0).gameObject);
                        bigframe.transform.localScale = new Vector3(totalPicDim * bigFrameScaleMulti, totalPicDim/2, totalPicDim * bigFrameScaleMulti);
                        bigframe.transform.position = new Vector3(0,0, bigframe.transform.position.z);
                        bigframe.transform.parent = imgParent.transform.parent;
                    }
                }
                else // Each pic has frame
                    frame = Instantiate(pictureBlockPrefab, new Vector3(x * gridGap - maxDistHalf, y * gridGap - maxDistHalf, transform.position.z), Quaternion.Euler(-90, 0, 0));
                

                frame.transform.parent = imgParent.transform;
                frame.transform.localScale = new Vector3(pictureBlockScale, pictureBlockScale, pictureBlockScale);

            }

        var wallSpr = imgParent.GetComponentsInChildren<SpriteRenderer>();

        //Setup images
        for (int i = 0; i < wallSpr.Length; i++)
            wallSpr[i].sprite = allPictures[i];


        deleteNearObstacles();
    }



    void Update()
    {

        //Move selection square like in tetris (Move this somewhere else?)
        if (player.floating)
        {
            if (mouseSelection)
                mouseControlls();
            else
                buttonControlls();

            
            int squareWidthHalf = (int) Mathf.Sqrt(allPictures.Length) / 2;

            bool widthIsEven = ((Mathf.Sqrt(allPictures.Length) / 2) % 1) == 0;

            if(!widthIsEven) selectedPos.Clamp(new Vector2Int(-squareWidthHalf, -squareWidthHalf), (new Vector2Int(squareWidthHalf, squareWidthHalf)));
            if (widthIsEven) selectedPos.Clamp(new Vector2Int(-squareWidthHalf + 1, -squareWidthHalf + 1), (new Vector2Int(squareWidthHalf, squareWidthHalf)));


            selectingSquare.transform.position = new Vector3(selectedPos.x * gridGap, selectedPos.y * gridGap, transform.position.z - pictureBlockScale * selectionZoffsetMult); //Push out depending on pic scale
            selectingSquare.transform.localScale = new Vector3(pictureBlockScale * selectionScaleMult, pictureBlockScale * selectionScaleMult, pictureBlockScale * selectionScaleMult);

            // Visual offset for uneven picture width
            if (widthIsEven)
                selectingSquare.transform.position -= new Vector3(0.5f * gridGap, 0.5f * gridGap, 0f);
        }
    }




    private void mouseControlls()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = selectingSquare.transform.position.z;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        print("old " + mousePos);

        //Convert from 3d Space into array (-2,-1,0,1,2)
        mousePos = new Vector3(mousePos.x/gridGap, mousePos.y / gridGap, mousePos.z);
        print("new " + mousePos);
        //selectedPos = new Vector2Int(Mathf.RoundToInt(playerPos.x / gridGap), Mathf.RoundToInt(playerPos.y / gridGap));
        setSelectSquarePos(mousePos);
    }


    private void buttonControlls()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            selectedPos += new Vector2Int(0, 1);
            accelerationCoroutines[0] = StartCoroutine(moveAcceleration(new Vector2Int(0, 1)));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedPos += new Vector2Int(0, 1);
            accelerationCoroutines[4] = StartCoroutine(moveAcceleration(new Vector2Int(0, 1)));
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            selectedPos += new Vector2Int(-1, 0);
            accelerationCoroutines[1] = StartCoroutine(moveAcceleration(new Vector2Int(-1, 0)));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedPos += new Vector2Int(-1, 0);
            accelerationCoroutines[5] = StartCoroutine(moveAcceleration(new Vector2Int(-1, 0)));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            selectedPos += new Vector2Int(0, -1);
            accelerationCoroutines[2] = StartCoroutine(moveAcceleration(new Vector2Int(0, -1)));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedPos += new Vector2Int(0, -1);
            accelerationCoroutines[6] = StartCoroutine(moveAcceleration(new Vector2Int(0, -1)));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            selectedPos += new Vector2Int(1, 0);
            accelerationCoroutines[3] = StartCoroutine(moveAcceleration(new Vector2Int(1, 0)));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedPos += new Vector2Int(1, 0);
            accelerationCoroutines[7] = StartCoroutine(moveAcceleration(new Vector2Int(1, 0)));
        }


        if (Input.GetKeyUp(KeyCode.W)) if (accelerationCoroutines[0] != null) StopCoroutine(accelerationCoroutines[0]);
        if (Input.GetKeyUp(KeyCode.A)) if (accelerationCoroutines[1] != null) StopCoroutine(accelerationCoroutines[1]);
        if (Input.GetKeyUp(KeyCode.S)) if (accelerationCoroutines[2] != null) StopCoroutine(accelerationCoroutines[2]);
        if (Input.GetKeyUp(KeyCode.D)) if (accelerationCoroutines[3] != null) StopCoroutine(accelerationCoroutines[3]);

        if (Input.GetKeyUp(KeyCode.UpArrow)) if (accelerationCoroutines[4] != null) StopCoroutine(accelerationCoroutines[4]);
        if (Input.GetKeyUp(KeyCode.LeftArrow)) if (accelerationCoroutines[5] != null) StopCoroutine(accelerationCoroutines[5]);
        if (Input.GetKeyUp(KeyCode.DownArrow)) if (accelerationCoroutines[6] != null) StopCoroutine(accelerationCoroutines[6]);
        if (Input.GetKeyUp(KeyCode.RightArrow)) if (accelerationCoroutines[7] != null) StopCoroutine(accelerationCoroutines[7]);
    }



    public void deleteNearObstacles()
    {
        var allObstacles = FindObjectsOfType<DamageObject>();
        foreach (DamageObject d in allObstacles)
        {
            if (Vector3.Distance(d.transform.parent.position, transform.position) <= delteObstaclesRadius)
                Destroy(d.transform.parent.gameObject);
        }
    }

    public Vector3 getSelectSquarePos()
    {
        return selectingSquare.transform.position;
    }

    public void setSelectSquarePos(Vector3 playerPos)
    {
        selectedPos = new Vector2Int(Mathf.RoundToInt(playerPos.x / gridGap), Mathf.RoundToInt(playerPos.y / gridGap));
    }


    public int getSelectedPicture()
    {
        /*
        //Go from world coordinates to array rotation (array has weird orientation)
        float squareDim = (Mathf.Sqrt(allPictures.Length) / 2) * gridGap;
        Vector2 pos = selectedPos + new Vector2(squareDim / 2, -squareDim / 2);
        pos.y = -pos.y;
        if ((squareDim % 2) != 0) pos -= new Vector2(0.5f,0.5f); //Make up for uneven length eg 9 boxes instead of 8


        //Translate to index that position would have in the array
        return Mathf.RoundToInt(pos.y * squareDim + pos.x);*/


        // DONT USE VISUALS (gridGap)
        float squareWidth = Mathf.Sqrt(allPictures.Length);

        var selectPosToArr = selectedPos;

        selectPosToArr.y = -selectPosToArr.y;
        selectPosToArr = selectPosToArr + new Vector2Int((int)(squareWidth/2), (int)(squareWidth/2));
        if (squareWidth % 2 == 0) selectPosToArr = selectPosToArr - new Vector2Int(1, 0);

        return Mathf.RoundToInt(selectPosToArr.y * (int)squareWidth + selectPosToArr.x);

    }

    private void disableSelection()
    {
        //crosshair.SetActive(false);
        selectingSquare.SetActive(false);
    }
    private void enableSelection()
    {
        //crosshair.SetActive(true);
        selectingSquare.SetActive(true);
    }






    private IEnumerator moveAcceleration(Vector2Int dir)
    {
        yield return new WaitForSeconds(selectingDelay);

        while (true)
        {
            selectedPos += dir;
            yield return new WaitForSeconds(selectingSpeed);
        }
    }


}


