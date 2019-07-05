using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PictureToSearchGO : MonoBehaviour
{
    private GameObject picSearchUI;
    [SerializeField] private GameObject fadeFrame;
    [SerializeField] private bool magnet3D = true;
    [SerializeField] private float magnetDistanceZ = 15f;
    [SerializeField] private float magnetSpeed = 1f;
    [SerializeField] private float animSpeed = 1f;

    [SerializeField] private float floatSpeed = 1.5f;
    [SerializeField] private float floatDirChangeSpd = 0.1f; // 0 means dont change initial float dir

    [SerializeField] private float randStart = 5f;
    [SerializeField] private float moveTopLeftSpd = 1f;

    [Space]
    [SerializeField] private float shrinkTime = 0.25f;
    [SerializeField] private float shrinkScale = 0.5f;

    private Transform player;
    private bool notCollected = true;
    private Vector3 moveDir;

    void Start()
    {
        FindObjectOfType<PictureManager>().setSearchedUIvisible();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        picSearchUI = GameObject.FindGameObjectWithTag("Picture Search UI");

        moveDir = Random.insideUnitSphere;

        transform.position += new Vector3(Random.Range(-randStart, randStart), Random.Range(-randStart, randStart), 0);

        FindObjectOfType<PictureManager>().hideMovingSearchedUI();
    }

    void Update()
    {

        //float dist3D = Vector3.Distance(player.position, transform.position);
        float distXY = Vector2.Distance(player.position, transform.position);
        float distZ = Mathf.Abs(player.position.z - transform.position.z);

        if (notCollected)
        {
            if (distZ < magnetDistanceZ) //Magnet
            {
                /*
                float speed = magnetDistanceZ - distZ;
                speed = speed * Time.deltaTime * magnetSpeed;

                if (magnet3D) transform.position = Vector3.MoveTowards(transform.position, player.position, speed);
                else transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, player.position.y, transform.position.z), speed);

                if (distXY <= 1) hitPlayer();
                */

                Vector3 target = player.position;
                target.z = transform.position.z;
                transform.DOMove(target, animSpeed).SetEase(Ease.OutExpo); // OTHER EASE NOT WORKING CUZ EVERY FRAME???

                if (player.position.z > transform.position.z) hitPlayer();
            }
            else //Wander
            {
                moveDir += Random.insideUnitSphere * floatDirChangeSpd;
                moveDir.Normalize();
                moveDir *= floatSpeed;

                transform.position += moveDir * Time.deltaTime;
            }
        }



        //Vector3 target = chunkManager.getSelectSquarePos();
        //target.z = transform.position.z;
        //transform.DOMove(target, flyPicDur).SetEase(Ease.InFlash);
    }

    private void hitPlayer()
    {
        FindObjectOfType<PictureManager>().setSearchedUIvisible();

        notCollected = false;

        GetComponent<MeshRenderer>().enabled = false;

        var spr = GetComponentInChildren<SpriteRenderer>().sprite;
        GetComponentInChildren<SpriteRenderer>().enabled = false;

        //var f = Instantiate(fadeFrame, transform.position, Quaternion.Euler(-90,0,0));
        //f.transform.localScale = transform.localScale;


        picSearchUI.transform.GetChild(picSearchUI.transform.childCount - 1).GetComponent<Image>().sprite = spr;

        RectTransform recUI = (RectTransform)picSearchUI.transform;
        var UIorigin = recUI.anchoredPosition;

        // Move pic search UI to world position of frame
        recUI.position = worldToUISpace(GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>(), transform.position); //recUI.anchoredPosition


        //TODO: set scale properly  
        //https://answers.unity.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html

        StartCoroutine(moveUItopLeftAndKillThis(recUI, UIorigin));
    }





    public void setPicture(Sprite spr)
    {
        GetComponentInChildren<SpriteRenderer>().sprite = spr;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //hitPlayer();
        }
    }



    //https://forum.unity.com/threads/world-size-to-screen-size.596425/
    //https://stackoverflow.com/questions/45046256/move-ui-recttransform-to-world-position
    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {

        //RectTransform CanvasRect = GameObject.FindGameObjectWithTag("GameplayCanvas").GetComponent<RectTransform>();
        //For center anchor:
        /*Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(f.transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
        recUI.anchoredPosition = WorldObject_ScreenPosition; */


        //For left top anchor:
        /*Vector3 character = f.transform.position;
        Vector2 viewport = Camera.main.WorldToViewportPoint(character);
        Vector2 WorldObject_ScreenPosition = new Vector2(viewport.x * CanvasRect.sizeDelta.x,viewport.y * CanvasRect.sizeDelta.y);
        WorldObject_ScreenPosition.y = -WorldObject_ScreenPosition.y;

        recUI.anchoredPosition = WorldObject_ScreenPosition;*/



        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);

        return parentCanvas.transform.TransformPoint(movePos);
    }



    private IEnumerator moveUItopLeftAndKillThis(RectTransform recUI, Vector3 end)
    {
        var orgScale = recUI.localScale;

        recUI.DOScale(recUI.localScale * shrinkScale, shrinkTime).SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(shrinkTime);

        recUI.DOScale(orgScale, moveTopLeftSpd).SetEase(Ease.InOutQuad);

        recUI.DOAnchorPos(end, moveTopLeftSpd).SetEase(Ease.InCubic);

        Destroy(gameObject, moveTopLeftSpd);
    }
}



