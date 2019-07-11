using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private AnimationCurve coutndownStart;
    [SerializeField] private int restoreHPStart = 25;
    [SerializeField] private int maxRestoreHP = 50;
    [SerializeField] private Slider hpBar;
    [SerializeField] private int maxHealth = 100;
    //[SerializeField] private float healthLossDelay = 0.1f;
    [SerializeField] private int damageOnSelect = 10;
    [SerializeField] private int healOnSelect = 25;
    [SerializeField] private float alphaMaxFlicker = 0.5f;
    [SerializeField] private float timeFlicker = 0.5f;
    [SerializeField] private int flickerTimes = 8; //Even!
    [SerializeField] private Image damageFlicker;
    [SerializeField] private GameObject GameOverCanvas;
    [SerializeField] private GameObject PauseMenuCanvas;
    bool pause = false;
    private float health;
    //private float healthTimer;
    private float flickerTimer;
    private bool invincible;
    private bool alreadyDied;
    private PlayerMovement playerMovement;
    private UiManager uiManager;
    private DifficultyManager difficultyManager;
    private SoundEffects soundEffects;
    public bool isTutorial;

   public GameObject score;


    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        uiManager = FindObjectOfType<UiManager>();
        difficultyManager = FindObjectOfType<DifficultyManager>();
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
            //healthTimer -= Time.deltaTime;

            float hpLossPerSec = difficultyManager.getHealthLoss();
            health -= Time.deltaTime * hpLossPerSec;


            //Calculate how long until dead (and show on last seconds)
            float secondsLeft = health / hpLossPerSec;

            var coutndownThreshold = coutndownStart.Evaluate(difficultyManager.getDimNorm());
            coutndownThreshold = Mathf.RoundToInt(coutndownThreshold);

            if (secondsLeft < coutndownThreshold)           
                uiManager.setCountdown(secondsLeft);
            else
                uiManager.setCountdown(-99);
                
            //print(secondsLeft);
        }



        hpBar.value = health; //TODO: sometimes trigger doesnt work??

        if (health <= 0)
        {
            if (!isTutorial)
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
            }
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
        if (wasCorrect)
        {
            healPlayer(healOnSelect);
            if (isTutorial)
                SceneManager.LoadScene("World01big");
        }
        if (!wasCorrect)
        {
            // Also sound + shake
            FindObjectOfType<SoundEffects>().selectedWrong();
            FindObjectOfType<ScreenShakeTest>().wrongSelection();

            damagePlayer(damageOnSelect, true);
        }
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


    public float getHealth()
    {
        return health;
    }

    public void setInvincible(bool b)
    {
        invincible = b;
    }


    public void addHPifLow()
    {
        if (health <= restoreHPStart)
        {
            var restoreHP = health.Remap(0, restoreHPStart, maxRestoreHP, 0);
            healPlayer(Mathf.RoundToInt(restoreHP));
        }
    }
}
