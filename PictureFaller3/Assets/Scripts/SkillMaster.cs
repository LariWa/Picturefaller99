using System.Collections;
using System.Collections.Generic;
using UnityEngine;


abstract public class SkillMaster : MonoBehaviour
{
    public string name;
    public int averageDamage; //sometimes + or -?
    public float cost = 10f;
    public float cooldown = 0.5f;
    protected float timerCD;
    //private actualActionThatHappens
    //public whoToDamage

    private Skillset skillset;

    private void Start()
    {
        skillset = GetComponent<Skillset>();

    }

    private void Update()
    {
        timerCD -= Time.deltaTime;
    }

    public void updateUI() //insert default implementation to be inherited
    {

    }

    public virtual void showIndication(Vector2 mousePos) //insert default implementation to be inherited
    {

    }
    public virtual void removeIndication()
    {

    }


    public void tryToDoSkill(Vector3 shootDir)
    {
        if (timerCD <= 0) //if its priamry can always shoot, even if no HP
        {
            timerCD = cooldown;
            doSkill(shootDir);
        }
    }

    //Method that has no default, needs to be overriden
    public abstract void doSkill(Vector3 mousePos);

    public abstract int getInputSlot();


    public float getTimerCD()
    {
        return timerCD;
    }

}

