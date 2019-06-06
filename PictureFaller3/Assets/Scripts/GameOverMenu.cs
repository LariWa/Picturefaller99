﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameOverMenu : MonoBehaviour
{
    public GameObject scoreText;
    public Text name;
    public GameObject GameOverCanvas;
    public GameObject LeaderboardCanvas;
    public GameObject MainMenu;

    public HighscoreTable highscoreTable;

    public void PlayGame () 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //GameOverCanvas.SetActive(false);
    }

    public void LoadMenu () 
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void submit()
    {
       highscoreTable = new HighscoreTable();
        string Name = name.text;
        Debug.Log(Name);
        Debug.Log(scoreText.transform.GetComponent<TextMeshProUGUI>().text);
        string score = scoreText.transform.GetComponent<TextMeshProUGUI>().text;
        char[] charSeparator = new char[] { ' ' };
        score = score.Split(charSeparator, StringSplitOptions.None)[0];
        Debug.Log(score);
        int Score = int.Parse(score);

        //int score = int.Parse(scoreText.transform.GetComponent<TextMeshProUGUI>().text);
        Debug.Log(Score);
        highscoreTable.AddHighscoreEntry(Score, Name);
        Debug.Log(Score);
        Debug.Log(Name);

    }

    public void showLeaderboard()
    {
        Debug.Log("show Leaderboard");
       // GameOverCanvas.SetActive(false);
        LeaderboardCanvas.SetActive(true);
    }
}
