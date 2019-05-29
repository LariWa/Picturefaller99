using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private Image screenFadeImg;
    [SerializeField] private float screenFadeSpdIn = 0.25f;
    [SerializeField] private float screenFadeSpdOut = 0.5f;
    [SerializeField] private float screenWhiteDur = 0.5f;

    // TODO: Move player slowmoTimer here !!!

    private ChunkManager chunkManager;
    private CameraManager cameraManager; 
    private SettingManager settingManager; 
    private bool hitWall;

    void Start()
    {
        chunkManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ChunkManager>();
        settingManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<SettingManager>();
        cameraManager = Camera.main.GetComponent<CameraManager>();

        setFadeAlpha(0);
    }


    void Update()
    {
        
    }


    public void doSettingTransition()
    {
        if(!hitWall)
        {
            hitWall = true;
            StartCoroutine(fadeScreenWhiteOver(1, screenFadeSpdIn));
        }
    }




    private void setFadeAlpha(float am)
    {
        var col = screenFadeImg.color;
        col.a = am;
        screenFadeImg.color = col;
    }


    private IEnumerator fadeScreenBackOver(float aValue, float aTime)
    {
        float alpha = screenFadeImg.color.a;

        for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
        {
            setFadeAlpha(Mathf.Lerp(alpha, aValue, t));
            yield return null;
        }
        setFadeAlpha(aValue);
    }


    private IEnumerator fadeScreenWhiteOver(float aValue, float aTime)
    {
        float alpha = screenFadeImg.color.a;

        for (float t = 0f; t <= 1f; t += Time.deltaTime / aTime)
        {
            setFadeAlpha(Mathf.Lerp(alpha, aValue, t));
            yield return null;
        }
        setFadeAlpha(aValue);


        yield return new WaitForSeconds(screenWhiteDur);


        // Screen is now completely white

        settingManager.changeSettingToDifferentOne();

        chunkManager.resetChunksAndWall();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().reroute();

        cameraManager.setNormalCam(true);

        hitWall = false;

        StartCoroutine(fadeScreenBackOver(0, screenFadeSpdOut));
    }

}
