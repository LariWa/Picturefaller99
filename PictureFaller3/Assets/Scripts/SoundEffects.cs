using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioSource clockTicking;
    public AudioSource clockTicking2;
    public AudioSource clockIncreasing;
    public Vector2 clockIncPitchMinMax;
    public AudioSource correctPictureSel;
    public AudioSource wrongPictureSel;
    [Space]
    public AudioSource coinPickup;
    public AudioSource hpPickup;
    public AudioSource hitDamageObj;
    public AudioSource tookDamage;
    public AudioSource hitImageCollected;
    public AudioSource hitImageLiquid;
    public AudioSource slowmoEffect;
    public AudioSource airResistance;
    public AudioSource objectCloseToCameraWhoosh; //probably do this with audio source on the object
    public AudioSource gameOver;


    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        
    }

    public void selectedCorrect()
    {
        correctPictureSel.Play();
    }
    public void selectedWrong()
    {
        wrongPictureSel.Play();
    }
    public void countdownTick(float nr)
    {
        nr = nr.Remap(10, 1, clockIncPitchMinMax.x, clockIncPitchMinMax.y);

        clockIncreasing.pitch = nr;
        clockIncreasing.Play();

        clockTicking.Play();
    }
    public void countdownTickOffbeat()
    {
        StartCoroutine(offbeatTick());
    }

    private IEnumerator offbeatTick()
    {
        yield return new WaitForSeconds(0.5f);
        clockTicking2.Play();
    }

    public void stopOffbeatTick()
    {
        StopAllCoroutines();
    }



    public void coin()
    {
        coinPickup.Play();
    }
    public void hp()
    {
        hpPickup.Play();
    }


    public void hitDmgObj()
    {
        hitDamageObj.Play();
    }

    public void collectedImg()
    {
        hitImageCollected.Play();
    }
}
