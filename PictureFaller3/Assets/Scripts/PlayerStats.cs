﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private Slider hpBar;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float healthLossDelay = 0.1f;
    [SerializeField] private int damageOnThinking = 1;
    [SerializeField] private int damageOnSelect = 10;
    [SerializeField] private int healOnSelect = 25;
    [SerializeField] private float alphaMaxFlicker = 0.5f;
    [SerializeField] private float timeFlicker = 0.5f;
    [SerializeField] private int flickerTimes = 8; //Even!
    [SerializeField] private Image damageFlicker;
    [SerializeField] private GameObject GameOverCanvas;
    [SerializeField] private GameObject PauseMenuCanvas;
    bool pause = false;
    private int health;
    private float healthTimer;
    private float flickerTimer;
    private bool invincible;
    private bool alreadyDied;
    private PlayerMovement playerMovement;
    private UiManager uiManager;
    private SoundEffects soundEffects;

    public GameObject score;


    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        uiManager = FindObjectOfType<UiManager>();
        soundEffects = FindObjectOfType<SoundEffects>();
        health = maxHealth;
        hpBar.maxValue = maxHealth;
    }

    void Update()
    {
        flickerTimer -= Time.deltaTime;

        // Subtract health over time on pic wall
        if (playerMovement.floating && !playerMovement.divingDown)
        {
            healthTimer -= Time.deltaTime;

            if(healthTimer <= 0)
            {
                healthTimer = healthLossDelay;
                damagePlayer(damageOnThinking, false);
            }

            //Calculate how long until dead (and show on last seconds)
            var hpLossPerSec = 1 / healthLossDelay;
            var secondsLeft = health / hpLossPerSec;

            if (secondsLeft < 10)           
                uiManager.setCountdown(secondsLeft);
            else
                uiManager.setCountdown(-99);

            //print(secondsLeft);
        }



        hpBar.value = health; //TODO: sometimes trigger doesnt work??

        if (health <= 0)
        {
            Time.timeScale = 0;
            GameObject.FindGameObjectWithTag("Managers").GetComponent<Slowmotion>().setPlayerDead();
            //GameOverCanvas.SetActive(true);
            char[] charSeparator = new char[] { ' ' };
            string scoreNumber = score.transform.GetComponent<TextMeshProUGUI>().text;
            scoreNumber = scoreNumber.Split(charSeparator, StringSplitOptions.None)[0];
            int scoreInt = int.Parse(scoreNumber);

          
            PlayerPrefs.SetInt("score", scoreInt);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Outro");


            if(!alreadyDied)
            {
                alreadyDied = true;
                Time.timeScale = 1;
                GameObject.FindGameObjectWithTag("Managers").GetComponent<Slowmotion>().setPlayerDead();
                //GameOverCanvas.SetActive(true);
                //SceneManager.LoadScene("Outro");

                int id = 0;
                if (PlayerPrefs.HasKey("sessionID"))
                {
                    id = PlayerPrefs.GetInt("sessionID");
                    id++;
                    PlayerPrefs.SetInt("sessionID", id);
                }
                else
                    PlayerPrefs.SetInt("sessionID", 1);

            }

        }
        if (Input.GetKeyDown(KeyCode.Escape) && !playerMovement.divingDown && !playerMovement.floating)
        {
            //If pause hasn't been activated
            if (!pause)
            {

                Time.timeScale = 0f;
                GameObject.FindGameObjectWithTag("Managers").GetComponent<Slowmotion>().setPlayerDead();
                pause = true;
                PauseMenuCanvas.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                GameObject.FindGameObjectWithTag("Managers").GetComponent<Slowmotion>().setPlayerAlive();
                PauseMenuCanvas.SetActive(false);
                pause = false;
            }
        }
            //TODO: stop setting timescale in time manager
        }



    public void selectedPicHealOrDmg(bool wasCorrect)
    {
        if (wasCorrect) healPlayer(healOnSelect);
        if (!wasCorrect) damagePlayer(damageOnSelect, true);
    }



    public void damagePlayer(int damage, bool flicker)
    {
        if (invincible) return;

        if (flicker) 
        {
            flickerTimer = timeFlicker;
            var desCol = damageFlicker.color;
            desCol.a = alphaMaxFlicker;

            if (flickerTimer > 0) // prevent alpha adding on many damage after another
            {
                var def = desCol;
                def.a = 0;
                damageFlicker.color = def;
                damageFlicker.DOKill();
            }
            damageFlicker.DOColor(desCol, timeFlicker).SetEase(Ease.InFlash, flickerTimes, 1);
        }


        health -= damage;

        if (health <= 0)
            Time.timeScale = 0;
    }
    public void healPlayer(int am)
    {
        health += am;

        if (health >= maxHealth)
            health = maxHealth;
    }


    public int getHealth()
    {
        return health;
    }

    public void setInvincible(bool b)
    {
        invincible = b;
    }
}
