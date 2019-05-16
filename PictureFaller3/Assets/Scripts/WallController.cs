using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    private GameObject fireParent;
    [SerializeField] private GameObject ventilator;
    [SerializeField] private GameObject defaultMesh;
    [SerializeField] private GameObject shapeMeshTEST;
    [SerializeField] private GameObject colorMeshTEST;
    //[SerializeField] private GameObject crosshair;

    [SerializeField] private GameObject selectingSquare;
    [SerializeField] private float selectingSpeed = 0.1f;
    [SerializeField] private float selectingDelay = 0.2f;
    private Vector2 selectedPos;
    private Coroutine[] accelerationCoroutines = new Coroutine[4];

    private PlayerMovement player;
    private WallManager wallManager;

    private bool pictureMode; //If it has pictures or not;

    private Vector3 playerStartOffset = Vector3.zero;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        wallManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<WallManager>();
    }

    void Update()
    {
        //crosshair.transform.position = new Vector3(player.transform.position.x, crosshair.transform.position.y, player.transform.position.z);
        //var multiplier = wallManager.getGridGap();
        //selectingSquare.transform.position = new Vector3(Mathf.RoundToInt(player.transform.position.x / multiplier) * multiplier, selectingSquare.transform.position.y, Mathf.RoundToInt(player.transform.position.z / multiplier) * multiplier);


        //Move selection square like in tetris (Move this somewhere else?)
        if (player.floating)
        {
            var multiplier = wallManager.getGridGap();
            //Vector3 playerStartOffset = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.W))
            {
                selectedPos += new Vector2(0, 1);
                accelerationCoroutines[0] = StartCoroutine(moveAcceleration(new Vector2(0, 1)));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                selectedPos += new Vector2(-1, 0);
                accelerationCoroutines[1] = StartCoroutine(moveAcceleration(new Vector2(-1, 0)));
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                selectedPos += new Vector2(0, -1);
                accelerationCoroutines[2] = StartCoroutine(moveAcceleration(new Vector2(0, -1)));
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                selectedPos += new Vector2(1, 0);
                accelerationCoroutines[3] = StartCoroutine(moveAcceleration(new Vector2(1, 0)));
            }

            if (Input.GetKeyUp(KeyCode.W)) if (accelerationCoroutines[0] != null) StopCoroutine(accelerationCoroutines[0]);
            if (Input.GetKeyUp(KeyCode.A)) if (accelerationCoroutines[1] != null) StopCoroutine(accelerationCoroutines[1]);
            if (Input.GetKeyUp(KeyCode.S)) if (accelerationCoroutines[2] != null) StopCoroutine(accelerationCoroutines[2]);
            if (Input.GetKeyUp(KeyCode.D)) if (accelerationCoroutines[3] != null) StopCoroutine(accelerationCoroutines[3]);

            //TODO: add bounds check for selectedPos

            selectingSquare.transform.position = new Vector3(selectedPos.x * multiplier, selectingSquare.transform.position.y, selectedPos.y * multiplier);
        }


    }



    public void setEmptyFire(int index)
    {
        for (int i = 0; i < fireParent.transform.childCount; i++)
            fireParent.transform.GetChild(i).gameObject.SetActive(true);

        fireParent.transform.GetChild(index).gameObject.SetActive(false);
    }

    public void setFireParent(GameObject go)
    {
        fireParent = go;
    }

    public void setShape()
    {
        defaultMesh.SetActive(false);
        shapeMeshTEST.SetActive(true);
        colorMeshTEST.SetActive(false);
    }

    public void setColor()
    {
        defaultMesh.SetActive(false);
        shapeMeshTEST.SetActive(false);
        colorMeshTEST.SetActive(true);
        //get spriterenderer

        var cols = colorMeshTEST.GetComponentsInChildren<SpriteRenderer>();
        var r = Random.Range(0f, 1f);
        var g = Random.Range(0f, 1f);
        var b = Random.Range(0f, 1f);
        cols[0].color = new Color(r,g,b);
        cols[1].color = new Color(r + Random.Range(0f, 0.33f), g + Random.Range(0f, 0.33f), b + Random.Range(0f, 0.33f));
    }




    public void setPictureModeOn()
    {
        pictureMode = true;
        defaultMesh.SetActive(true);
        shapeMeshTEST.SetActive(false);
        colorMeshTEST.SetActive(false);

        ventilator.SetActive(true);
        fireParent.SetActive(true);

        //Show pictures
        var imgPar = transform.Find("Image Parent").gameObject;
        for (int i = 0; i < imgPar.transform.childCount; i++)
            imgPar.transform.GetChild(i).gameObject.SetActive(true);

        enableSelection();
    }
    public void setPictureModeOff()
    {
        pictureMode = false;
        ventilator.SetActive(false);
        fireParent.SetActive(false);

        //Hide pictures
        var imgPar = transform.Find("Image Parent").gameObject;
        for (int i = 0; i < imgPar.transform.childCount; i++)
            imgPar.transform.GetChild(i).gameObject.SetActive(false);

        disableSelection();
    }



    public bool getPictureMode()
    {
        return pictureMode;
    }



    public Vector3 getSelectSquarePos()
    {
        return selectingSquare.transform.position;
    }

    public void setSelectSquarePos(Vector3 playerPos)
    {
        var multiplier = wallManager.getGridGap();
        selectedPos = new Vector3(Mathf.RoundToInt(playerPos.x / multiplier), Mathf.RoundToInt(playerPos.z / multiplier));
    }


    public int getSelectedPicture()
    {
        //Go from world coordinates to array rotation (array has weird orientation)
        var squareDim = wallManager.getGridWidthAndHeight();
        var pos = selectedPos + new Vector2(squareDim / 2, -squareDim / 2);
        pos.y = -pos.y;
        if ((squareDim % 2) != 0) pos -= new Vector2(0.5f,0.5f); //Make up for uneven length eg 9 boxes instead of 8


        //Translate to index that position would have in the array
        return (int) (pos.y * squareDim + pos.x);
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






    private IEnumerator moveAcceleration(Vector2 dir)
    {
        yield return new WaitForSeconds(selectingDelay);

        while(true)
        {
            selectedPos += dir;
            yield return new WaitForSeconds(selectingSpeed);
        }
    }


}
