using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float xyDrag = 0; // between 0 and 1 to restrict floaty movement on xy axis (come to stop faster)
    [SerializeField] private float controlSpeed = 16f; // how fast to move on xy axis from input
    [SerializeField] private float gravity = 9.81f; //how fast fall accelerates
    [SerializeField] private float maxFallSpeed = 10f; //maximum fall speed
    [SerializeField] private float maxFallSpeedBOOST = 60f; //maximum fall speed when holding down space
    [SerializeField] private float boostImpulse = 20f;
    [SerializeField] private float jumpImpulse = 200f;

    [Space]

    [SerializeField] private float dashDelay = 0.2f; //how long to wait until second click registers as double click
    [SerializeField] private float dashImpulse = 50f;
    //private Coroutine doubleTapCoroutine = new Coroutine();
    private float dashTimer = -1;
    private Vector2 lastDir;

    [Space]

    //[SerializeField] private float slowmoCurve = 1;
    [SerializeField] private float slowmoDuration = 1; //how long until full stop
     //[SerializeField] private enum slowmoEase;
    [SerializeField] private float knockback = 2;
    [SerializeField] private float knockbackSideways = 2;

    [Space]

    [SerializeField] private float flyPicDur = 2f;
    [SerializeField] private bool useDebugAbilities = false;
    public bool mouseSelection;

    private Rigidbody rb;
    private ScoreManager scoreManager;
    private ChunkManager chunkManager;
    private ScienceTimer scienceTimer;
    private PictureManager pictureManager;
    private PlayerStats stats;

    private float inputHor;
    private float inputVert;
    private float startFloatZ;
    private float slowmoTimer;
    private KeyCode selectKey = KeyCode.Space;

    //private Tween activeTween;

    public bool divingDown { get; private set; }
    public bool floating { get; private set; } // rename to inSlowmo

    public bool maus;

    public Slider setting;


    Quaternion startRot;


    



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponentInChildren<PlayerStats>();

        pictureManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<PictureManager>();
        scienceTimer = GameObject.FindGameObjectWithTag("Managers").GetComponent<ScienceTimer>();
        chunkManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ChunkManager>();
        scoreManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ScoreManager>();

        if (mouseSelection) selectKey = KeyCode.Mouse0;

        StartCoroutine(DoubleTapInputListener());

        Physics.gravity = new Vector3(0, 0, gravity);
        startRot = rb.rotation;
      
    }


    void Update()
    {
        dashTimer -= Time.deltaTime;// unscaledDeltaTime; //ignore slow motion(?) TRY https://answers.unity.com/questions/1155266/timescale-not-affect-timer-c.html
        slowmoTimer -= Time.deltaTime;

        if (slowmoTimer <= 0)
        {
            rb.useGravity = true;
            slowmoTimer = 0;
        }
        if (floating)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            //gravity = slowmoTimer.Remap(0, slowmoDuration, 0, gravityNormal);

        }

        //if (MenuController.GameIsPaused)
        //{
        //    rb.velocity = Vector3.zero;
        //}
       

        //freeze player at -3 while camera fully zoomed in to hide him
        //if (gravity <= 0) transform.position = new Vector3(transform.position.x, -3, transform.position.z);


        inputHor = Input.GetAxisRaw("Horizontal"); //GetAxis
        inputVert = Input.GetAxisRaw("Vertical");

        if (floating && Input.GetKeyDown(selectKey))
        {
            if(!divingDown) //only allow one correct selection
            {
                var correct = pictureManager.selectedAPic();

                if (correct && stats.getHealth() != 0) //Dive down since into a picture
                {
                    divingDown = true;
                    floating = false;
                    rb.useGravity = true;

                    /*if(activeTween != null)
                    {
                        print("y");
                        //activeTween.Kill();
                        DOTween.Kill(activeTween.id);   }*/

                    Vector3 target = chunkManager.getSelectSquarePos();
                    target.z += 2f;
                    transform.DOMove(target, flyPicDur).SetEase(Ease.InFlash/*InCubic*/);
                }
            }
        }



        scoreManager.scoreIncreasing = true;

        if (divingDown)
        {
            inputHor = 0;
            inputVert = 0;
        }
        else if (floating)
        {
            scoreManager.scoreIncreasing = false;
        }


        maus = Convert.ToBoolean(Convert.ToInt16(setting.value));

    }



    void rotNormal()
    {
        rb.MoveRotation(startRot);      
    }

    void FixedUpdate()
    {

        Vector3 previousLoaction = transform.position;
        Vector3 moveVec = (Vector3.right * inputHor) + (Vector3.up * inputVert);
    


        moveVec.Normalize();

      
        if (inputHor > 0)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0,0,-3));
            Invoke("rotNormal", 0.3f);

        }
        if (inputHor < 0)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, 0, 3));
            Invoke("rotNormal", 0.3f);

        }
        if (inputVert< 0)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(2, 0, 0));
            Invoke("rotNormal", 0.3f);

        }
        if (inputVert > 0)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(-2, 0, 0));
            Invoke("rotNormal", 0.3f);

        }







        //rb.velocity = (moveVec * moveSpeed) + (Vector3.forward * gravity); //Bad...
        //rb.AddForce(moveVec * speed, ForceMode.Impulse); //ForceMode.VelocityChange?  ADD DRAG, but not on y... maybe just bigger rb mass https://answers.unity.com/questions/1130605/can-i-prevent-rigidbody-drag-from-affecting-the-z.html


        // Player controlls

        rb.AddForce(moveVec * controlSpeed);//, ForceMode.Impulse);



        if (maus)
        {
            Vector3 mousePosition = Input.mousePosition;
            // mousePosition.z = Camera.main.nearClipPlane;
            mousePosition.z = rb.transform.position.z;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 direction = (mousePosition - rb.transform.position).normalized;

            rb.AddForce(new Vector2(direction.x * 300, direction.y * 300));
        }
        else
            rb.AddForce(moveVec * controlSpeed);//, ForceMode.Impulse);

        // Check and do dash
        //dash();



        float maxDown = maxFallSpeed;
        if (useDebugAbilities)
        {
            // If pressing ... add an impulse boost and make falling limit speed bigger
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                rb.AddForce(Vector3.forward * boostImpulse, ForceMode.Impulse);
            }
            if (Input.GetKey(KeyCode.LeftShift)) maxDown = maxFallSpeedBOOST;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(-Vector3.forward * jumpImpulse, ForceMode.Impulse);
            }
        }
        


        // Drag for controlls
        Vector3 vel = rb.velocity;
        vel.x *= xyDrag;
        vel.y *= xyDrag;
        if (vel.z >= maxDown) vel.z = maxDown; // Max fall speed
        rb.velocity = vel;
    }

    public void knockBack(Vector3 objectPos)
    {
        rb.velocity = Vector3.zero;

        //Prevent being knocked into positive z
        var knockDirSideways = transform.position - objectPos;
        knockDirSideways.z = 0f;
        knockDirSideways.Normalize();
        knockDirSideways *= knockbackSideways;

        var knockVec = (-transform.forward * knockback) + knockDirSideways;

        rb.AddForce(knockVec, ForceMode.Impulse);
        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
    }

    public void moveBack()
    {
        //var back = (-transform.forward * 4);
        //rb.velocity = back;

        if(!divingDown)
            transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z - 4), 0.5f); //makes the camera float weirdly back when diving (?)
    }



    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "PictureSafeZone")
        //    pictureManager.rollPicToSearch();

        if (other.tag == "Wind")
        {
            //First time hitting wind trigger (slow down)
            if (!floating)
            {
                Camera.main.GetComponent<CameraManager>().setPictureCam();
                //chunkManager.setSelectSquarePos(new Vector3(transform.position.x, transform.position.y, chunkManager.getSelectSquarePos().z));
                slowmoTimer = slowmoDuration;
                scienceTimer.resetTimer();
                moveBack(); //so that the player is not in the way
            }
            floating = true;
        }
        else if (other.tag == "Wall") //Actually hit the pictures
        {
            pictureManager.hitPicWall();
            //divingDown = false;
            
        }
    }



    public void rerouteAndReset()
    {
        divingDown = false;
        floating = false;

        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        transform.position = new Vector3(0, -1, 0); // don't preserve X and Y because then will end up on the sides often
        rb.velocity = Vector3.zero;
    }




    // https://gamedev.stackexchange.com/questions/116455/how-to-properly-differentiate-single-clicks-and-double-click-in-unity3d-using-c
    private IEnumerator DoubleTapInputListener()
    {
        while (enabled)//Run as long as this is active
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                yield return ClickEvent(new Vector2(0, 1), KeyCode.W, KeyCode.UpArrow);

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                yield return ClickEvent(new Vector2(-1, 0), KeyCode.A, KeyCode.LeftArrow);

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                yield return ClickEvent(new Vector2(0, -1), KeyCode.S, KeyCode.DownArrow);

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                yield return ClickEvent(new Vector2(1, 0), KeyCode.D, KeyCode.RightArrow);

            yield return null;
        }
    }

    private IEnumerator ClickEvent(Vector2 dir, KeyCode a, KeyCode b)
    {
        //pause a frame so you don't pick up the same mouse down event
        yield return new WaitForEndOfFrame();

        float count = 0f;
        while (count < dashDelay)
        {
            if (Input.GetKeyDown(a) || Input.GetKeyDown(b))
            {
                rb.AddForce(dir * dashImpulse, ForceMode.Impulse); //Dash here
                yield break;
            }
            count += Time.deltaTime;// increment counter by change in time between frames
            yield return null; // wait for the next frame
        }
        //SingleClick();
    }



    /* private void dash() // or try https://forum.unity.com/threads/double-tapping-axis-input.8620/
     {
         if (floating) return;


         if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
         {
             if (dashTimer > 0)
             {
                 if (lastDir == new Vector2(0, 1))
                 {
                     rb.AddForce(lastDir * dashImpulse, ForceMode.Impulse);
                     dashTimer = -1;
                 }
             }
             else
             {
                 dashTimer = dashDelay;
                 lastDir = new Vector2(0, 1);
             }
         }

         if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
         {
             if (dashTimer > 0)
             {
                 if (lastDir == new Vector2(-1, 0))
                 {
                     rb.AddForce(lastDir * dashImpulse, ForceMode.Impulse);
                     dashTimer = -1;
                 }
             }
             else
             {
                 dashTimer = dashDelay;
                 lastDir = new Vector2(-1, 0);
             }
         }


         if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
         {
             if (dashTimer > 0)
             {
                 if (lastDir == new Vector2(0, -1))
                 {
                     rb.AddForce(lastDir * dashImpulse, ForceMode.Impulse);
                     dashTimer = -1;
                 }
             }
             else
             {
                 dashTimer = dashDelay;
                 lastDir = new Vector2(0, -1);
             }
         }


         if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
         {
             if (dashTimer > 0)
             {
                 if (lastDir == new Vector2(1, 0))
                 {
                     rb.AddForce(lastDir * dashImpulse, ForceMode.Impulse);
                     dashTimer = -1;
                 }
             }
             else
             {
                 dashTimer = dashDelay;
                 lastDir = new Vector2(1, 0);
             }
         }

    }*/



}



public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


}
