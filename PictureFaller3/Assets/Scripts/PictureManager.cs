using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureManager : MonoBehaviour
{
    [SerializeField] private Image picSearched; //Move somewhere else? UI
    private int currPicSearched;

    [SerializeField] private Sprite blackPicture;
    

    private ChunkManager chunkManager;
    private ScienceTimer scienceTimer;
    private ScoreManager scoreManager;
    private TransitionManager transitionManager;


    void Start()
    {
        chunkManager = GetComponent<ChunkManager>();
        scienceTimer = GetComponent<ScienceTimer>();
        scoreManager = GetComponent<ScoreManager>();
        transitionManager = GetComponent<TransitionManager>();

        //rollPicToSearch();
        picSearched.sprite = null;
    }





    
    public bool hitCorrectPicture()
    {
        var selectedPictureIndex = chunkManager.getCurrPictureWall().GetComponent<WallController>().getSelectedPicture();

        if (selectedPictureIndex == currPicSearched) return true;

        return false;
    }



    public void selectedPic()
    {
        scienceTimer.printTimer();
    }


    public void hitPicWall()
    {
        if (hitCorrectPicture())
        {
            scoreManager.addScorePictureHit(scienceTimer.getTime());
            print("The selection was correct!");

            transitionManager.doSettingTransition(); //also resets chunks and picwall
        }
        else
        {
            chunkManager.spawnPicWallOffsetFromLast();
            Camera.main.GetComponent<CameraManager>().setNormalCam(false);
        }


        picSearched.sprite = null;


        //chunkManager.spawnPicWall();
    }

    public void rollPicToSearch()
    {
        var currentPics = GetComponent<SettingManager>().getAllCurrentPicturesInSort();
        currPicSearched = Random.Range(0, currentPics.Length);
        picSearched.sprite = currentPics[currPicSearched];
    }
}
