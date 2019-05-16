using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMasterManager : MonoBehaviour
{
    private ObstacleManager obstacleManager;
    private ScienceTimer scienceTimer;
    private ScoreManager scoreManager;
    private WallManager wallManager;
    


    void Start()
    {
        obstacleManager = GetComponent<ObstacleManager>();
        scienceTimer = GetComponent<ScienceTimer>();
        scoreManager = GetComponent<ScoreManager>();
        wallManager = GetComponent<WallManager>();
    }


    void Update()
    {

    }


    public void hitWall()
    {
        if (wallManager.currentIsPictureWall() && wallManager.hitCorrectPicture())
        {
            if (wallManager.getCurrentPictureWall().GetComponent<WallController>().getPictureMode())  //TODO: rather if player hits key
                scienceTimer.printTimer();

            scoreManager.addScorePictureHit(scienceTimer.getTime());
            print("The selection was correct!");

            scienceTimer.resetTimer();
        }

        wallManager.hitWall();
    }


    public Vector3 getSelectSquarePos()
    {
        return wallManager.getCurrentPictureWall().GetComponent<WallController>().getSelectSquarePos();
    }

    public void setSelectSquarePos(Vector3 pos)
    {
        wallManager.getCurrentPictureWall().GetComponent<WallController>().setSelectSquarePos(pos);
    }
}
