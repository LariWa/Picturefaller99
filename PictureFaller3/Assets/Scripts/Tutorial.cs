using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject move;
    public GameObject space;
    public GameObject picture;
    public GameObject coins;
    public GameObject pause;

    public Image slowmoMeter;
    bool slowmo = true;

    // Start is called before the first frame update
    void Start()
    {
        showTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene("World01big");
        }
        //if (FindObjectWithName(PlayerStats).health)
       
    }

    public void showTutorial()
    {
        Invoke("showMove", 0);
        Invoke("showSlowmo", 5);
        Invoke("showCoins", 9);
        Invoke("showPause", 13);
        Invoke("setPicture", 17);
    }

    public void showMove()
    {
        move.SetActive(true);
    }

    public void showSlowmo()
    {
        move.SetActive(false);
        space.SetActive(slowmo);
        if(slowmo)
            slowmoMeter.color = Color.green;
    }

    public void showCoins()
    {
        space.SetActive(false);
        slowmoMeter.color = Color.black;
        coins.SetActive(true);
    }

    public void showPause()
    {
        coins.SetActive(false);
        pause.SetActive(true);
    }

    public void setPicture()
    {
        pause.SetActive(false);
        slowmo = false;
        Invoke("pictureActive", 2);        
    }

    public void pictureActive()
    {
        picture.SetActive(true);
    }

   
}
