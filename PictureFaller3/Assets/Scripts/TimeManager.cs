using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private WallManager wallManager;
    private PlayerMovement player;
    private CameraManager camera;
    private Transform playerTrans;

    [SerializeField] private float distFromWallStartSlow = 30f;
    [SerializeField] private float distFromWallFullSlow = 10f;
    [SerializeField] private float minTime = 0.1f;
    //[SerializeField] private float slowDownDur = 1;
    [SerializeField] private float slowmoCurve = 1;


    void Start()
    {
        camera = Camera.main.GetComponent<CameraManager>();
        wallManager = GetComponent<WallManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerTrans = player.transform;
    }

    /*
    void Update()
    {
        var playerWallDist = Mathf.Abs(wallManager.getNextWallPos().y - playerTrans.position.y);


        
        if (playerWallDist < distFromWallStartSlow && playerWallDist > distFromWallFullSlow)
        {
            Time.timeScale = playerWallDist.Remap(distFromWallStartSlow, distFromWallFullSlow, 1, minTime);
            camera.setSlowMoProgress(playerWallDist.Remap(distFromWallStartSlow, distFromWallFullSlow, 1, 0));
            player.setSlowMoProgress(playerWallDist.Remap(distFromWallStartSlow, distFromWallFullSlow, 1, 0));
        }
        else if (playerWallDist < distFromWallFullSlow)
        {
            Time.timeScale = minTime;
            camera.setSlowMoProgress(0);
            player.setSlowMoProgress(0);
        }
        else
        {
            resetTime();
        }

        if (player.divingDown)
            resetTime();

        //print(Time.timeScale);
    }

    public void resetTime()
    {
        Time.timeScale = 1;
        camera.setSlowMoProgress(1);
        player.setSlowMoProgress(1);
    }*/


}


