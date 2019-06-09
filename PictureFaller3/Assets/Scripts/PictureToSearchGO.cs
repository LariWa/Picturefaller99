using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureToSearchGO : MonoBehaviour
{
    private GameObject picSearchUI;
    [SerializeField] private GameObject fadeFrame;
    [SerializeField] private float magnetDistance = 100f;
    [SerializeField] private float magnetSpeed = 1f;
    [SerializeField] private float fadeSpd = 0.5f;
    [SerializeField] private float moveTopLeftSpd = 1f;

    private Transform player;
    private bool notCollected = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        picSearchUI = GameObject.FindGameObjectWithTag("Picture Search UI");
    }

    void Update()
    {
        //Magnet follow player on x/y

        float dist = Vector3.Distance(player.position, transform.position);

        if (dist < magnetDistance && notCollected)
        {
            float speed = magnetDistance - dist;
            speed = speed * Time.deltaTime * magnetSpeed;
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed);
        }

        //Vector3 target = chunkManager.getSelectSquarePos();
        //target.z = transform.position.z;
        //transform.DOMove(target, flyPicDur).SetEase(Ease.InFlash);
    }



    public void setPicture(Sprite spr)
    {
        GetComponentInChildren<SpriteRenderer>().sprite = spr;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            notCollected = false;

            GetComponent<MeshRenderer>().enabled = false;

            var spr = GetComponentInChildren<SpriteRenderer>().sprite;
            GetComponentInChildren<SpriteRenderer>().enabled = false;

            var f = Instantiate(fadeFrame, transform.position, Quaternion.Euler(-90,0,0));
            f.transform.localScale = transform.localScale;

            var material = f.GetComponent<MeshRenderer>().material;
            StartCoroutine(fadeObjOver(0, fadeSpd, material));

            picSearchUI.GetComponent<Image>().sprite = spr;

            RectTransform recUI = (RectTransform)picSearchUI.transform;
            var UIorigin = recUI.anchoredPosition;

            // Move pic search UI to world position of frame

            RectTransform CanvasRect = GameObject.FindGameObjectWithTag("GameplayCanvas").GetComponent<RectTransform>();
            //For center anchor:
            /*Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(f.transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
                ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
            recUI.anchoredPosition = WorldObject_ScreenPosition; */


            //For left top anchor:
            Vector3 character = f.transform.position;
            Vector2 viewport = Camera.main.WorldToViewportPoint(character);
            Vector2 WorldObject_ScreenPosition = new Vector2(viewport.x * CanvasRect.sizeDelta.x,viewport.y * CanvasRect.sizeDelta.y);
            WorldObject_ScreenPosition.y = -WorldObject_ScreenPosition.y;/**/

            recUI.anchoredPosition = WorldObject_ScreenPosition;


            //TODO: set scale and position properly  
            //https://answers.unity.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html
            //https://forum.unity.com/threads/world-size-to-screen-size.596425/

            GameObject x = new GameObject("f", typeof(RectTransform));
            var rec = x.GetComponent <RectTransform>();
            StartCoroutine(moveUItopLeftAndKillThis(recUI, recUI.anchoredPosition, UIorigin, moveTopLeftSpd,rec));
        }
    }



    private IEnumerator moveUItopLeftAndKillThis(RectTransform recUI, Vector3 start, Vector3 end, float time, RectTransform f)
    {
        for (float t = 0f; t <= 1f; t += Time.deltaTime / time)
        {
            recUI.anchoredPosition = Vector3.Lerp(start, end, t);
            f.anchoredPosition = Vector3.Lerp(start, end, t);
            yield return null;
        }
        recUI.anchoredPosition = end;
        f.anchoredPosition = end;


        Destroy(gameObject);
    }



    private IEnumerator fadeObjOver(float aValue, float aTime, Material material)
    {
        float alpha = material.color.a;

        for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
        {
            setFadeAlpha(Mathf.Lerp(alpha, aValue, t), material);
            yield return null;
        }
        setFadeAlpha(aValue, material);
    }
    private void setFadeAlpha(float am, Material material)
    {
        var col = material.color;
        col.a = am;
        material.color = col;
    }
}
