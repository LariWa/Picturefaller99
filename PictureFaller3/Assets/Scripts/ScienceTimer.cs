using System.Collections.Generic;
using UnityEngine;

public class ScienceTimer : MonoBehaviour
{

    //https://www.tantzygames.com/blog/most-accurate-timer-in-unity/

    private float timer;
    private bool timerStarted = true;
    private float time;
    private SettingManager settingM ;
    private DifficultyManager diffM;
    void Start()
    {
        settingM = FindObjectOfType<SettingManager>();
        diffM = FindObjectOfType<DifficultyManager>();
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
        time = timer;
        print("It took the player: " + time);
    }

    public float getTime()
    {

        var  quality = settingM.getQuality();
        var dif = diffM.getDim();
        var response = sendPostRequest(time,quality,dif); //SPIEL FUNKT SONST NICHT
        Debug.Log(response);
        Debug.Log("Dimension ist :" + dif);
        Debug.Log("Qualität ist :" + quality);
        return timer;
    }

    public string getSessionID()
    {
        if (PlayerPrefs.HasKey("sessionID"))
            return SystemInfo.deviceUniqueIdentifier + PlayerPrefs.GetInt("sessionID");
        else
            return SystemInfo.deviceUniqueIdentifier + 0;
    }

    public void resetTimer()
    {
        timer = 0;
    }
    /* AUSKOMMENTIERT SONST FUNKT DAS SPIEL NICHT */
    private string sendPostRequest(float userTime,int sortQuality, int dim)
    {

        var httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://localhost:3000/addtimerihno");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";

        using (var streamWriter = new System.IO.StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string json = "{\"time\":" + userTime + "," + 
                           "\"sortQuality\":" + sortQuality + "," + 
                           "\"dim\":"+ dim + "}";

            Debug.Log(json);
            streamWriter.Write(json);
        }

        var httpResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            return result;
        }

    }
}
