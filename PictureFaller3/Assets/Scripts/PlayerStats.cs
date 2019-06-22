using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

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
    private PlayerMovement playerMovement;


    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        health = maxHealth;
        hpBar.maxValue = maxHealth;
    }

    void Update()

    {
        flickerTimer -= Time.deltaTime;

        if (playerMovement.floating)
        {
            healthTimer -= Time.deltaTime;

            if(healthTimer <= 0)
            {
                healthTimer = healthLossDelay;
                damagePlayer(damageOnThinking, false);
            }
        }


        hpBar.value = health; //TODO: sometimes trigger doesnt work??

        if (health <= 0)
        {
            Time.timeScale = 0;
            GameObject.FindGameObjectWithTag("Managers").GetComponent<Slowmotion>().gameOver();
            GameOverCanvas.SetActive(true);




        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pause)
            {
                Time.timeScale = 0f;
                GameObject.FindGameObjectWithTag("Managers").GetComponent<Slowmotion>().gameOver();
                pause = true;
                PauseMenuCanvas.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
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

        if (flicker && flickerTimer < 0) //flickerTimer prevents alpha adding on many damage after another
        {
            flickerTimer = timeFlicker;
            var desCol = damageFlicker.color;
            desCol.a = alphaMaxFlicker;
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
