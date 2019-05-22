using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private Slider hpBar;
    [SerializeField] private int maxHealth = 100;
    static public int health;

    void Start()
    {
        health = maxHealth;
        hpBar.maxValue = maxHealth;
    }
    
    void Update()
    {
        hpBar.value = health; //TODO: sometimes trigger doesnt work?? 

        if (health <= 0)
            //Player is dead, game over
            Time.timeScale = 0; //TODO: stop setting timescale in time manager
    }

    public void damagePlayer(int damage)
    {
        health -= damage;
    }

    public int getHealth()
    {
        return health;
    }
}
