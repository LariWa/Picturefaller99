using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [SerializeField] private Sprite blackPicture;
    private Sprite[] allPictures; // ammount needs to be squared so 4, 9, 16, 25 etc
    [SerializeField] private GameObject pictureBlockPrefab; //TODO: make frame instead
    [SerializeField] private float pictureBlockScale = 1.5f;
    [SerializeField] private float gridGap = 2f;
    [SerializeField] private float delteObstaclesRadius = 40f;

    [Space]

    [SerializeField] private GameObject selectingSquare;
    [SerializeField] private float selectingSpeed = 0.1f;
    [SerializeField] private float selectingDelay = 0.2f;

    private Vector2Int selectedPos; //intern array position of selection
    private Coroutine[] accelerationCoroutines = new Coroutine[8];

    private PlayerMovement player;
    private PictureManager wallManager;

    private Vector3 playerStartOffset = Vector3.zero;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        wallManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<PictureManager>();

        var settingManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<SettingManager>();
        settingManager.randomSortForSetting(settingManager.getNextSetting());
        allPictures = settingManager.getAllPicturesInSort(settingManager.getNextSetting());

        // ------ Init ---------
        GameObject imgParent = new GameObject("Image Parent");
        imgParent.transform.parent = transform;

        int gridCells = Mathf.RoundToInt(Mathf.Sqrt(allPictures.Length));

        float maxDistHalf = ((gridCells - 1) * gridGap) / 2; //Used to center images for even and uneven gridCells

        for (int y = gridCells - 1; y >= 0; y--)
            for (int x = 0; x < gridCells; x++)
            {
                var frame = Instantiate(pictureBlockPrefab, new Vector3(x * gridGap - maxDistHalf, y * gridGap - maxDistHalf, transform.position.z), Quaternion.Euler(-90,0,0));

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


            int squareWidthHalf = (int) Mathf.Sqrt(allPictures.Length) / 2;

            bool widthIsEven = ((Mathf.Sqrt(allPictures.Length) / 2) % 1) == 0;

            if(!widthIsEven) selectedPos.Clamp(new Vector2Int(-squareWidthHalf, -squareWidthHalf), (new Vector2Int(squareWidthHalf, squareWidthHalf)));
            if (widthIsEven) selectedPos.Clamp(new Vector2Int(-squareWidthHalf + 1, -squareWidthHalf + 1), (new Vector2Int(squareWidthHalf, squareWidthHalf)));


            selectingSquare.transform.position = new Vector3(selectedPos.x * gridGap, selectedPos.y * gridGap, selectingSquare.transform.position.z);

            // Visual offset for uneven picture width
            if (widthIsEven)
                selectingSquare.transform.position -= new Vector3(0.5f, 0.5f, 0f);
        }
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
