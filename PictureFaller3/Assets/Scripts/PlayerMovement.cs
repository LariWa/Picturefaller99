using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 16f;
    [SerializeField] private float gravityNormal = 32f;
    //[SerializeField] private float slowmoCurve = 1;
    [SerializeField] private float slowmoDuration = 1; //how long until full stop
     //[SerializeField] private enum slowmoEase;
    [SerializeField] private float knockback = 2;
    [SerializeField] private float knockbackSideways = 2;

    private Rigidbody rb;
    private ScoreManager scoreManager;
    private ChunkManager chunkManager;
    private ScienceTimer scienceTimer;
    private PictureManager pictureManager;
    private TransitionManager transitionManager;
    private float inputHor;
    private float inputVert;
    private float startFloatZ;
    private float gravity;
    private float slowmoTimer;

    public bool divingDown { get; private set; }
    public bool floating { get; private set; } // rename to inSlowmo


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transitionManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TransitionManager>();
        pictureManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<PictureManager>();
        scienceTimer = GameObject.FindGameObjectWithTag("Managers").GetComponent<ScienceTimer>();
        chunkManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ChunkManager>();
        scoreManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ScoreManager>(); 

        gravity = gravityNormal;
    }


    void Update()
    {
        slowmoTimer -= Time.deltaTime;

        if (slowmoTimer <= 0)
        {
            gravity = gravityNormal;
            slowmoTimer = 0;
        }
        if(floating) gravity = slowmoTimer.Remap(0, slowmoDuration, 0, gravityNormal);



        //freeze player at -3 while camera fully zoomed in to hide him
        //if (gravity <= 0) transform.position = new Vector3(transform.position.x, -3, transform.position.z);


        inputHor = Input.GetAxisRaw("Horizontal"); //GetAxis
        inputVert = Input.GetAxisRaw("Vertical");


        if (Input.GetKeyDown(KeyCode.Space) && floating) //KeypadEnter?
        {
            pictureManager.selectedPic();
            divingDown = true;
            floating = false;
            gravity = gravityNormal;

            // Vector3 target = transform.position;
            //target.y = -3;
            //transform.DOMove(target, 5f);
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


        rb.velocity = (moveVec * moveSpeed) + (Vector3.forward * gravity); //Bad...

        //rb.AddForce(moveVec * speed, ForceMode.Impulse); //ForceMode.VelocityChange?  ADD DRAG, but not on y... maybe just bigger rb mass https://answers.unity.com/questions/1130605/can-i-prevent-rigidbody-drag-from-affecting-the-z.html
    }

    public void knockBack()
    {
        rb.AddForce(-transform.forward * knockback, ForceMode.Impulse);
        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Wind")
        {
            //First time hitting wind
            if (!floating)
            {
                chunkManager.setSelectSquarePos(new Vector3(transform.position.x, transform.position.y, chunkManager.getSelectSquarePos().z));
                slowmoTimer = slowmoDuration;
                scienceTimer.resetTimer();
            }
            floating = true;
        }
        else if (other.tag == "Wall")
        {
            pictureManager.hitPicWall();
            transitionManager.hitPicWall();
            divingDown = false;
            floating = false;
        }
    }



    public void reroute()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        
    }
}



public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}