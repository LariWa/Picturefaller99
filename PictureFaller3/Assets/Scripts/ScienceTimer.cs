using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ScienceTimer : MonoBehaviour
{

    //https://www.tantzygames.com/blog/most-accurate-timer-in-unity/

    private float timer;
    private bool timerStarted = true;
    private float time;
    private SettingManager settingM;
    private DifficultyManager diffM;
    private byte[] jsonStringTrial;

    private int hashLength = 13;
    private string hash;

    void Start()
    {
        settingM = FindObjectOfType<SettingManager>();
        diffM = FindObjectOfType<DifficultyManager>();

        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        for (int i = 0; i < hashLength; i++)
            hash += chars[Random.Range(0, chars.Length)];
    }

    void Update()
    {
        //print(timer);
        if (timerStarted)
        {
            timer += Time.unscaledDeltaTime;// deltaTime;

        }

    }


    public string getSessionID()
    {
        var session = 0;
        if (PlayerPrefs.HasKey("sessionID"))
            session = PlayerPrefs.GetInt("sessionID");


        return hash + "" + session;

        /*
        if (PlayerPrefs.HasKey("sessionID"))
            return SystemInfo.deviceUniqueIdentifier + PlayerPrefs.GetInt("sessionID");
        else
            return SystemInfo.deviceUniqueIdentifier + 0;*/
    }

    public void printTimer()
    {

        print("It took the player: " + timer);
        //var h = getSessionID();
        //Debug.Log("Gunther :"+ h);
    }

    public float getTime()
    {

        var quality = settingM.getQuality();
        var dif = diffM.getDim();
        var sessionID = getSessionID();
        StartCoroutine(sendPostRequest(timer, quality, dif, sessionID)); //SPIEL FUNKT SONST NICHT
        //Debug.Log("Dimension ist :" + dif + " Qualität ist :" + quality + " It took the player: " + time);
        //Debug.Log("SessionID ist :" + sessionID);
        return timer;
    }


    public void resetTimer()
    {
        timer = 0;
    }
    /* AUSKOMMENTIERT SONST FUNKT DAS SPIEL NICHT 
    private string sendPostRequest(float userTime,int sortQuality, int dim, string gameID)
    {
        var httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("https://fathomless-spire-55232.herokuapp.com/addtimerihno");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        using (var streamWriter = new System.IO.StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string json = "{\"time\":" + userTime + "," +
                      "\"sortQuality\":" + sortQuality + "," +
                      "\"dim\":" + dim + "," +
                      "\"gameID\":" + '"' + gameID + '"' + "}";
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
    */
    /////////////////////////////////////
    /* AUSKOMMENTIERT SONST FUNKT DAS SPIEL NICHT */
    public IEnumerator sendPostRequest(float userTime, int sortQuality, int dim, string gameID)
    {

        string json = "{\"time\":" + userTime + "," +
                        "\"sortQuality\":" + sortQuality + "," +
                        "\"dim\":" + dim + "," +
                        "\"gameID\":" + '"' + gameID + '"' + "}";


        var request = new UnityWebRequest("http://localhost:3000/addtimerihno", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)

        {
            Debug.Log("ERROR: " + request.error);
        }
        else
        {
            string contents = request.downloadHandler.text;
            Debug.Log("SEND TIME RESPONSE: " + contents);

        }

    }


}