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
        //print(timer);
        if (timerStarted)
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
        // var response = sendPostRequest(timer); SPIEL FUNKT SONST NICHT
        //Debug.Log(timer);
        //Debug.Log(response);
        return timer;
    }

    public float getSessionID()
    {
        if (PlayerPrefs.HasKey("sessionID"))
            return PlayerPrefs.GetInt("sessionID");
        else
            return 0;
    }

    public void resetTimer()
    {
        timer = 0;
    }
    /* AUSKOMMENTIERT SONST FUNKT DAS SPIEL NICHT 
    private string sendPostRequest(float userTime)
    {
        var httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://localhost:3000/addtime3");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";

        using (var streamWriter = new System.IO.StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string json = "{\"time\":" + userTime + "}";

            Debug.Log(json);
            streamWriter.Write(json);
        }

        var httpResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            return result;
        }

    }*/
}
