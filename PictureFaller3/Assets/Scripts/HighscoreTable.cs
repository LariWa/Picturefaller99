﻿//Tutorial: https://www.youtube.com/watch?v=iAbaqGYdnyI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;
using UnityEngine.Networking;
using System.Text;

public class HighscoreTable : MonoBehaviour
{

    public Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;
    public GameObject Leaderboard;

  
 
    private void Awake()
    {
        Debug.Log("Awake");
        StartCoroutine(GetText());

        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        string jsonString = TempHighscoreEntry();
        Debug.Log(jsonString);
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);



        highscoreEntryTransformList = new List<Transform>();
        Debug.Log(highscoreEntryTransformList);
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }
    public void updateTable()
    {
        foreach (Transform child in entryContainer.gameObject.transform)
        {
            if(!(child.name == "highscoreEntryTemplate"))
            GameObject.Destroy(child.gameObject);
        }
        highscoreEntryTransformList = new List<Transform>();
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }

    }
    public void disable()
    {
        Leaderboard.SetActive(false);
    }
    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 20f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }
        if (highscoreEntry.name == "YOU" && rank == 11)
            rankString = "...";
        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        int score = highscoreEntry.score;

        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;


        // Highlight First
        if (rank == 1)
        {
            entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
        }

        if (name == "YOU")
        {
            entryTransform.Find("posText").GetComponent<Text>().color = Color.red;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.red;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.red;
        }

        transformList.Add(entryTransform);
    }



  
       
        IEnumerator GetText()
        {
            UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/selecthighscore");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);

                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;
            Debug.Log("Results are : "+ results);
            }
        
    }



    public int getHighestScore()
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        return highscores.highscoreEntryList[0].score;
    }

    

    public bool AddHighscoreEntry(int score, string name)
    {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Debug.Log(jsonString);
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            // There's no stored table, initialize
            highscores = new Highscores()
            {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }

        // Add new entry to Highscores

        foreach (HighscoreEntry entry in highscores.highscoreEntryList) //check name
        {
            if (entry.name == name || name == "YOU")
            {
                Debug.Log(entry.name);
                return false;
            }
        }


        highscores.highscoreEntryList.Add(highscoreEntry);
        highscores.highscoreEntryList = highscores.highscoreEntryList.OrderByDescending(o => o.score).ToList();
        if (highscores.highscoreEntryList.Count > 10)
            highscores.highscoreEntryList = highscores.highscoreEntryList.GetRange(0, 10);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
        //sendScoreRequest(name, score); FÜR DATENBANK
        updateTable();
        StartCoroutine(sendScoreRequest(name, score)); //FÜR DATENBANK
        return true;
    }
    public string TempHighscoreEntry()
    {
        int score = PlayerPrefs.GetInt("score");
        Debug.Log("score" + score);
         string name = "YOU";
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Debug.Log(jsonString);
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            // There's no stored table, initialize
            highscores = new Highscores()
            {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }

        // Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);
        highscores.highscoreEntryList = highscores.highscoreEntryList.OrderByDescending(o => o.score).ToList();
        if (highscores.highscoreEntryList.Count > 10)
            if(highscores.highscoreEntryList[10].name != "YOU")
            highscores.highscoreEntryList = highscores.highscoreEntryList.GetRange(0, 10);

        string json = JsonUtility.ToJson(highscores);
        return json;
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    /*
     * Represents a single High score entry
     * */
    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }



    public IEnumerator sendScoreRequest(string name, int score)
    {

        string json = "{\"name\":" + '"' + name + '"' + "," + "\"score\":" + score + "}";



        var request = new UnityWebRequest("http://localhost:3000/addscore", "POST");
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
            Debug.Log("SEND SCORE RESPONSE: " + contents);

        }




    }
}





