using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    private float dashTimer;
    private Vector2 lastDir;

    [Space]

    //[SerializeField] private float slowmoCurve = 1;
    [SerializeField] private float slowmoDuration = 1; //how long until full stop
     //[SerializeField] private enum slowmoEase;
    [SerializeField] private float knockback = 2;
    [SerializeField] private float knockbackSideways = 2;

    [Space]

    [SerializeField] private float flyPicDur = 2f;

    private Rigidbody rb;
    private ScoreManager scoreManager;
    private ChunkManager chunkManager;
    private ScienceTimer scienceTimer;
    private PictureManager pictureManager;

    private float inputHor;
    private float inputVert;
    private float startFloatZ;
    private float slowmoTimer;

    public bool divingDown { get; private set; }
    public bool floating { get; private set; } // rename to inSlowmo




    void Start()
    {
        rb = GetComponent<Rigidbody>();

        pictureManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<PictureManager>();
        scienceTimer = GameObject.FindGameObjectWithTag("Managers").GetComponent<ScienceTimer>();
        chunkManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ChunkManager>();
        scoreManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ScoreManager>();

        Physics.gravity = new Vector3(0, 0, gravity);
    }


    void Update()
    {
        dashTimer -= Time.deltaTime;
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


        //freeze player at -3 while camera fully zoomed in to hide him
        //if (gravity <= 0) transform.position = new Vector3(transform.position.x, -3, transform.position.z);


        inputHor = Input.GetAxisRaw("Horizontal"); //GetAxis
        inputVert = Input.GetAxisRaw("Vertical");


        if (Input.GetKeyDown(KeyCode.Space) && floating) //KeypadEnter?
        {
            pictureManager.selectedAPic();

            if(pictureManager.hitCorrectPicture())
            {
                divingDown = true;
                floating = false;
                rb.useGravity = true;

                Vector3 target = chunkManager.getSelectSquarePos();
                //target.z = transform.position.z;
                transform.DOMove(target, flyPicDur).SetEase(Ease.InFlash);
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
    }

    void FixedUpdate()
    {
        Vector3 moveVec = (Vector3.right * inputHor) + (Vector3.up * inputVert);

        moveVec.Normalize();


        //rb.velocity = (moveVec * moveSpeed) + (Vector3.forward * gravity); //Bad...
        //rb.AddForce(moveVec * speed, ForceMode.Impulse); //ForceMode.VelocityChange?  ADD DRAG, but not on y... maybe just bigger rb mass https://answers.unity.com/questions/1130605/can-i-prevent-rigidbody-drag-from-affecting-the-z.html


        // Player controlls
        rb.AddForce(moveVec * controlSpeed);//, ForceMode.Impulse);

        // Check and do dash
        dash();



        // If pressing ... add an impulse boost and make falling limit speed bigger
        float maxDown = maxFallSpeed;
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



    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Wind")
        {
            //First time hitting wind
            if (!floating)
            {
                pictureManager.rollPicToSearch();
                Camera.main.GetComponent<CameraManager>().setPictureCam();
                chunkManager.setSelectSquarePos(new Vector3(transform.position.x, transform.position.y, chunkManager.getSelectSquarePos().z));
                slowmoTimer = slowmoDuration;
                scienceTimer.resetTimer();
            }
            floating = true;
        }
        else if (other.tag == "Wall")
        {
            pictureManager.hitPicWall();
            divingDown = false;
            floating = false;
        }
    }



    public void reroute()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        
    }





    private void dash()
    {
        if (floating) return;
        

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (dashTimer >= 0 && lastDir == new Vector2(0, 1))
            {
                rb.AddForce(lastDir * dashImpulse, ForceMode.Impulse);
                dashTimer = -1;
            }
            else
            {
                dashTimer = dashDelay;
                lastDir = new Vector2(0, 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (dashTimer >= 0 && lastDir == new Vector2(-1, 0))
            {
                rb.AddForce(lastDir * dashImpulse, ForceMode.Impulse);
                dashTimer = -1;
            }
            else
            {
                dashTimer = dashDelay;
                lastDir = new Vector2(-1, 0);
            }
        }


        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (dashTimer >= 0 && lastDir == new Vector2(0, -1))
            {
                rb.AddForce(lastDir * dashImpulse, ForceMode.Impulse);
                dashTimer = -1;
            }
            else
            {
                dashTimer = dashDelay;
                lastDir = new Vector2(0, -1);
            }
        }


        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (dashTimer >= 0 && lastDir == new Vector2(1, 0))
            {
                rb.AddForce(lastDir * dashImpulse, ForceMode.Impulse);
                dashTimer = -1;
            }
            else
            {
                dashTimer = dashDelay;
                lastDir = new Vector2(1, 0);
            }
        }

    }

    

}



public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}