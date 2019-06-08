using System.Collections;
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
    public Text personalScore;
    public GameObject GameOverCanvas;
    public GameObject LeaderboardCanvas;
    public GameObject MainMenu;
    public Button submitButton;

    public GameObject nameUsed;
    public HighscoreTable highscoreTable;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //GameOverCanvas.SetActive(false);
    }

    public void Awake()
    {
        string score = scoreText.transform.GetComponent<TextMeshProUGUI>().text;
        char[] charSeparator = new char[] { ' ' };
        score = score.Split(charSeparator, StringSplitOptions.None)[0];
        int dif = highscoreTable.getHighestScore() - int.Parse(score);
        personalScore.text = "Your Score: " + score + "\n only " + dif + " missing to the highscore!";
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void submit()
    {
        string Name = name.text;
        string score = scoreText.transform.GetComponent<TextMeshProUGUI>().text;
        char[] charSeparator = new char[] { ' ' };
        score = score.Split(charSeparator, StringSplitOptions.None)[0];
        int Score = int.Parse(score);

        //highscoreTable.AddHighscoreEntry(Score, Name)
        bool sub = highscoreTable.AddHighscoreEntry(Score, Name);
        Debug.Log(sub);
  
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
