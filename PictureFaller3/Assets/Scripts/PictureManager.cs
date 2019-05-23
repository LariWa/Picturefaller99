using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureManager : MonoBehaviour
{
    [SerializeField] private Image picSearched; //Move somewhere else? UI
    private int currPicSearched;

    [SerializeField] private Sprite blackPicture;
    [SerializeField] private Sprite[] allPictures; // CURRENTLY THESE ARE HARDWIRED IN WALL PREFAB(?)

    private ChunkManager chunkManager;
    private ScienceTimer scienceTimer;
    private ScoreManager scoreManager;

    void Start()
    {
        chunkManager = GetComponent<ChunkManager>();
        scienceTimer = GetComponent<ScienceTimer>();
        scoreManager = GetComponent<ScoreManager>();

        currPicSearched = Random.Range(0,allPictures.Length);
        picSearched.sprite = allPictures[currPicSearched];
        //else picSearched.sprite = blackPicture;
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
        }

        currPicSearched = Random.Range(0, allPictures.Length);
        picSearched.sprite = allPictures[currPicSearched];
        //else picSearched.sprite = blackPicture;


        //chunkManager.spawnPicWall();
    }

}
