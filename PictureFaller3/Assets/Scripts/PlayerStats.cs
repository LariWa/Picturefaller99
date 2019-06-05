using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private Slider hpBar;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float healthLossDelay = 0.1f;
    [SerializeField] private int damageOnSelect = 10;
    [SerializeField] private int healOnSelect = 25;
    private int health;
    private float healthTimer;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        health = maxHealth;
        hpBar.maxValue = maxHealth;
    }

    void Update()
    {
        if (playerMovement.floating)
        {
            healthTimer -= Time.deltaTime;

            if(healthTimer <= 0)
            {
                healthTimer = healthLossDelay;
                damagePlayer(1);
            }
        }


        hpBar.value = health; //TODO: sometimes trigger doesnt work??

        if (health <= 0)
            //Player is dead, game over
            Time.timeScale = 0; //TODO: stop setting timescale in time manager
    }



    public void selectedPicHealOrDmg(bool wasCorrect)
    {
        if (wasCorrect) healPlayer(healOnSelect);
        if (!wasCorrect) damagePlayer(damageOnSelect);
    }



    public void damagePlayer(int damage)
    {
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
}
