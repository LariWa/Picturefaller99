using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceTimer : MonoBehaviour
{

    //https://www.tantzygames.com/blog/most-accurate-timer-in-unity/

    private float timer;
    private bool timerStarted = true;

    void Start()
    {
    }

    void Update()
    {
        if(timerStarted)
        {
            timer += Time.unscaledDeltaTime;// deltaTime;
        }

    }

    public void printTimer()
    {
        print("It took the player: " + timer);
    }

    public float getTime()
    {
        return timer;
    }

    public void resetTimer()
    {
        timer = 0;
    }
}
