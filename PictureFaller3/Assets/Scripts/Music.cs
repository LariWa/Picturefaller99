using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;

    static Music instance = null;
    public float slowMoPitchMin = 0.8f;

    private bool gameplay;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }

        audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (FindObjectOfType<PlayerStats>() != null)
            gameplay = true;


        if (gameplay)
        {
            if (audioSource.clip != gameplayMusic)
            {
                audioSource.clip = gameplayMusic;
                audioSource.Play();
            }

            var time = Time.timeScale;
            time = time.Remap(0,1, slowMoPitchMin ,1);
            audioSource.pitch = time;
        }
        else
        {
            if (audioSource.clip != menuMusic)
            {
                audioSource.clip = menuMusic;
                audioSource.Play();
            }
        }

        print(audioSource.clip.name);
    }

    // Update is called once per frame
    public void ToggleSound()
    {
        if (PlayerPrefs.GetInt("Muted", 0) == 0){
            PlayerPrefs.SetInt("Muted", 1);
            //AudioListener.volume = 1;
        }
        else
        {
            PlayerPrefs.SetInt("Muted", 0);
            //AudioListener.volume = 0;
        }
    }

    /*
    public void soundSlowMo()
    {
        GetComponent<AudioSource>().pitch = slowMoPitch; 
    }

    public void soundSlowMoDone()
    {
        GetComponent<AudioSource>().pitch = 1f;
    }*/
}
