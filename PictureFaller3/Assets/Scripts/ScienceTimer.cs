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

      
    }

    public void printTimer()
    {

        print("It took the player: " + timer);
        
    }

    public float getTime()
    {

        var quality = settingM.getQuality();
        var dif = diffM.getDim();
        var sessionID = getSessionID();
        StartCoroutine(sendPostRequest(timer, quality, dif, sessionID)); 
        
        return timer;
    }


    public void resetTimer()
    {
        timer = 0;
    }
   
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