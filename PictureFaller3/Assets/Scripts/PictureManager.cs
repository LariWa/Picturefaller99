using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureManager : MonoBehaviour
{

    [SerializeField] private Image picSearched; //Move somewhere else? UI
    private int currPicSearched;
    private bool justSelectedCorrect;

    [SerializeField] private Sprite blackPicture;
    

    private ChunkManager chunkManager;
    private ScienceTimer scienceTimer;
    private ScoreManager scoreManager;
    private DifficultyManager difficultyManager;
    private TransitionManager transitionManager;
    private PlayerStats playerStats;


    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerStats>();
        chunkManager = GetComponent<ChunkManager>();
        scienceTimer = GetComponent<ScienceTimer>();
        scoreManager = GetComponent<ScoreManager>();
        difficultyManager = GetComponent<DifficultyManager>();
        transitionManager = GetComponent<TransitionManager>();

        //rollPicToSearch();
        //hideMovingSearchedUI();
    }








    public bool hitCorrectPicture()
    {

        var selectedPictureIndex = chunkManager.getCurrPictureWall().GetComponent<WallController>().getSelectedPicture();

        if (selectedPictureIndex == currPicSearched)
            return true;
        
        return false;
    }



    public bool selectedAPic()
    {
        justSelectedCorrect = hitCorrectPicture();

        if (playerStats.getHealth() != 0 && justSelectedCorrect)
        {
            scienceTimer.printTimer();
            transitionManager.doDiveCamera();
            chunkManager.getCurrPictureWall().GetComponent<WallController>().changeCursorToDefault();
        }

        chunkManager.getCurrPictureWall().GetComponent<WallController>().selectionSquashOrShake(justSelectedCorrect);

        if (playerStats.getHealth() != 0 && chunkManager.getCurrPictureWall().GetComponent<WallController>().selectionNotOffscreen())
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerStats>().selectedPicHealOrDmg(justSelectedCorrect);

        return justSelectedCorrect;
    }


    public void hitPicWall()
    {
        if (justSelectedCorrect)
        {
            scoreManager.addScorePictureHit(scienceTimer.getTime());
            print("The selection was correct!");

            transitionManager.doSettingTransition(); //also resets chunks and picwall

            chunkManager.getCurrPictureWall().GetComponent<WallController>().correctPicWobble(currPicSearched);
        }
        else
        {
            chunkManager.spawnPicWallOffsetFromLast();
            Camera.main.GetComponent<CameraManager>().setNormalCam(false);
        }

        difficultyManager.hitWall();




        //chunkManager.spawnPicWall();
    }

    public void rollPicToSearch()
    {
        var currentPics = GetComponent<SettingManager>().getAllPicturesInSort(GetComponent<SettingManager>().getNextSetting());
        currPicSearched = Random.Range(0, currentPics.Length);

        
        //picSearched.sprite = currentPics[currPicSearched];

    }

    public Sprite getCurrentSearchPic()
    {
        var currentPics = GetComponent<SettingManager>().getAllPicturesInSort(GetComponent<SettingManager>().getNextSetting());
        return currentPics[currPicSearched];
    }



    public void setSearchedUIvisible()
    {
        picSearched.transform.parent.gameObject.SetActive(true);

        //var par = picSearched.transform.parent;
        //var col = par.GetChild(par.childCount - 1).GetComponent<Image>().color;
        //col.a = 1;
        //par.GetChild(par.childCount - 1).GetComponent<Image>().color = col;
    }



    public void hideMovingSearchedUI()
    {
        //picSearched.sprite = null;
        picSearched.transform.parent.gameObject.SetActive(false);

        // Fade instead 
        //var par = picSearched.transform.parent;
        //StartCoroutine(fadeToOver(0, 2f, par.GetChild(par.childCount - 1).GetComponent<Image>()));
    }




    private IEnumerator fadeToOver(float aValue, float aTime, Image img)
    {
        float alpha = img.color.a;

        for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
        {
            setFadeAlpha(Mathf.Lerp(alpha, aValue, t), img);
            yield return null;
        }
        setFadeAlpha(aValue, img);
    }
    private void setFadeAlpha(float am, Image img)
    {
        var col = img.color;
        col.a = am;
        img.color = col;
    }
}
