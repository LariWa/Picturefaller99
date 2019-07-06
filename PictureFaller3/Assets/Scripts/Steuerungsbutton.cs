using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Steuerungsbutton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onPress;
    public UnityEvent onRelease;
    private bool mouse;
    public Sprite on;
    public Sprite off;
    public Image button;
    private PlayerMovement playerM;

    public void Start()
    {
        if (PlayerPrefs.HasKey("moveSettings"))
        {
            mouse = Convert.ToBoolean(PlayerPrefs.GetInt("moveSettings"));
            if (mouse == false) button.sprite = off;
            else button.sprite = on;
        }
        else
        {
            mouse = false;
            PlayerPrefs.SetInt("moveSettings", 0);
        }


        playerM = FindObjectOfType<PlayerMovement>();
        if (playerM != null)
            playerM.setMouse(mouse);
    }

    public void changeSettings()
    {
        print("hi");
        Debug.Log("change");
        if (mouse)
        {
            PlayerPrefs.SetInt("moveSettings", 0);
            button.sprite = off;
            mouse = !mouse;
            if (playerM != null)
                playerM.setMouse(mouse);
        }
        else
        {
            PlayerPrefs.SetInt("moveSettings", 1);
            mouse = !mouse;
            button.sprite = on;
            if (playerM != null)
                playerM.setMouse(mouse);

        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onPress != null)
            onPress.Invoke();


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onRelease != null)
            onRelease.Invoke();


    }

}