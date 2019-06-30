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
    public Image slowmoMeter;
    bool slowmo = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        showTutorial();

        if (Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene("World01big");
        }

    }

    public void showTutorial()
    {
        Invoke("setSlowmo", 5);    
    }   

    public void setSlowmo()
    {
        
        move.SetActive(false);
        Invoke("spaceActive",2);
        Invoke("setPicture", 4);
        
    }

    public void spaceActive()
    {
        space.SetActive(slowmo);
        if(slowmo)
            slowmoMeter.color = Color.green;
        
    }

    public void setPicture()
    {
        slowmo = false;
        space.SetActive(false);
        slowmoMeter.color = Color.black;
        Invoke("pictureActive", 2);        
    }

    public void pictureActive()
    {
        picture.SetActive(true);
    }

   
}
