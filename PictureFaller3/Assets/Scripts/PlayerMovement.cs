using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 16f;
    [SerializeField] private float moveSpeedSlowMoMax = 64f;
    [SerializeField] private float fastGravity = 16f;
    [SerializeField] private float normalGravity = 32f;
    [SerializeField] private float slowGravity = 2f;
    private Rigidbody rb;
    private TimeManager timeManager;
    private ScoreManager scoreManager;
    private LevelMasterManager levelManager;
    private float inputHor;
    private float inputVert;
    private float gravity;


    [Space]
    [SerializeField] private float floatSpeed = 1;
    [SerializeField] private float floatAmplitude = 1;
    //[SerializeField] private GameObject meshGO;
    [SerializeField] private float smoothFollowSquare = 0.5f;
    private float startFloatY;
    private Vector3 m_refPos = Vector3.zero;
    public bool divingDown { get; private set; }
    public bool floating { get; private set; }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        levelManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<LevelMasterManager>();
        scoreManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ScoreManager>(); 
        timeManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TimeManager>();
    }

    void Update()
    {

        inputHor = Input.GetAxisRaw("Horizontal"); //GetAxis
        inputVert = Input.GetAxisRaw("Vertical");

        gravity = normalGravity;

        if (Input.GetKey(KeyCode.J)) //LeftShift
            gravity = fastGravity;

        if (Input.GetKey(KeyCode.K)) // OR RATHER MAKE A JUMP
            gravity = slowGravity;


        if (Input.GetKeyDown(KeyCode.Space) && floating) //KeypadEnter
        {
            divingDown = true;
            floating = false;
        }

        scoreManager.scoreIncreasing = true;

        if (divingDown)
        {
            inputHor = 0;
            inputVert = 0;
            gravity = normalGravity;
        }
        else if (floating)
        {
            gravity = 0;
            scoreManager.scoreIncreasing = false;

            //Animate floating? or use addforce and dont set velocity below???
            transform.position = new Vector3(transform.position.x, startFloatY + floatAmplitude * Mathf.Sin(floatSpeed * Time.time), transform.position.z); //Does it center itself again afterwards wit hcollider????

            // Move smoothly to selection square
            Vector3 target = levelManager.getSelectSquarePos();
            target.y = transform.position.y;
            //transform.position = Vector3.SmoothDamp(transform.position, target, ref m_refPos, smoothFollowSquare);
            transform.DOMove(target, smoothFollowSquare);
        } 
    }

    void FixedUpdate()
    {
        Vector3 moveVec = (Vector3.right * inputHor) + (Vector3.forward * inputVert);

        moveVec.Normalize();

        if(!floating) rb.velocity = (moveVec * moveSpeed) + (Vector3.down * gravity); //Bad...

        //rb.AddForce(moveVec * speed, ForceMode.Impulse); //ForceMode.VelocityChange?  ADD DRAG, but not on y... https://answers.unity.com/questions/1130605/can-i-prevent-rigidbody-drag-from-affecting-the-z.html
    }



    public void damage()
    {
        Time.timeScale = 0;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Wind")
        {
            //First time hitting wind
            if (!floating)
            {
                levelManager.setSelectSquarePos(new Vector3(transform.position.x, levelManager.getSelectSquarePos().y, transform.position.z));
                startFloatY = transform.position.y;
            }
            floating = true;
        }
        else if (other.tag == "Wall")
        {
            levelManager.hitWall();
            divingDown = false;
            floating = false;
        }

        //if its image container tell some manager which one(s) hit
    }
}


public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}