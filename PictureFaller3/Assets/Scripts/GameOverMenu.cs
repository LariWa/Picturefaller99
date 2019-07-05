using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameOverMenu : MonoBehaviour
{
    // public GameObject scoreText;
    public Text name;
    public Text personalScore;
    public Text personalHigh;
    public GameObject GameOverCanvas;
    public GameObject LeaderboardCanvas;
    public GameObject MainMenu;
    public Button submitButton;

    public GameObject nameUsed;
    public HighscoreTable highscoreTable;

    public void PlayGame()
    {
        SceneManager.LoadScene("World01big");
        GameOverCanvas.SetActive(false);
    }

    public void Awake()
    {
        int score = PlayerPrefs.GetInt("score");
        int pHighscore = PlayerPrefs.GetInt("personalHighscore");
        personalScore.text += score;
        Debug.Log(personalScore.text);
        char[] charSeparator = new char[] { ' ' };
        // score = score.Split(charSeparator, StringSplitOptions.None)[0];
        Debug.Log(score);

        if (score == pHighscore)
            personalHigh.text = "You beat your personal highscore!";
        else
            personalHigh.text = "Your personal highscore is: " + pHighscore;

       // int dif = highscoreTable.getHighestScore() - int.Parse(score);

            //string message = "";
            //if (dif < 0)
            //    message = "\n You beat the highscore!";
            //else
            // message = "\n only " + dif + " missing to the highscore!";
            // score = "Congratulations \n Your Score is: " + score + message;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void submit()
    {
        string Name = name.text;
        int Score = PlayerPrefs.GetInt("score");
        
        bool sub = highscoreTable.AddHighscoreEntry(Score, Name);
       
        //Debug.Log(sub);

        if (!sub)
        {
            nameUsed.SetActive(true);
        }
        else
        {
            submitButton.interactable = false;
            nameUsed.SetActive(false);
        }
    }

    public void showLeaderboard()
    {
        Debug.Log("show Leaderboard");
        // GameOverCanvas.SetActive(false);
        LeaderboardCanvas.SetActive(true);
    }
}
