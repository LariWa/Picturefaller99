using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slowmotion : MonoBehaviour
{
    [SerializeField] private float minTimeScale = 0.2f;
    [SerializeField] private float slowSpeed = 0.01f;
    [SerializeField] private AnimationCurve interpolateIn; //separate curve for ease out? for now just go back curve
    [SerializeField] private Slider slider;

    [Space]

    [SerializeField] private float loseRate = 0.1f;
    [SerializeField] private float refillRate = 0.05f;
    private float fuel = 1f;

    private PlayerMovement player;
    private float timer; //If 1 then full slow, if 0 then no slow
    private int timerDir = -1;
    bool alive = true;
    private CameraManager cam;

    private Music music;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        cam = Camera.main.GetComponent<CameraManager>();
        music = FindObjectOfType<Music>();
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && fuel != 0 && !player.floating && !player.divingDown)
        { 
            timerDir = 1;
            cam.setSlowMoCam(true);
        }
        else
        { 
            timerDir = -1;
            cam.setSlowMoCam(false);
        }

        timer += timerDir * slowSpeed;
        
        if (timer <= 0) timer = 0;
        if (timer >= 1) timer = 1;
        if (alive) 
        Time.timeScale = timer.Remap(0, 1, 1, minTimeScale);
        //print(Time.timeScale);




        /*
        if (Time.timeScale == 1)
            fuel += refillRate;
        else
            fuel -= loseRate;*/

        if (!player.floating && !player.divingDown)
            if (Input.GetKey(KeyCode.Space))
                fuel -= loseRate;
            else
                fuel += refillRate;



        if (fuel <= 0) fuel = 0;
        if (fuel >= 1) fuel = 1;

        slider.value = fuel;
    }

    public void gameOver()
    {
        alive = false;
    }
}
