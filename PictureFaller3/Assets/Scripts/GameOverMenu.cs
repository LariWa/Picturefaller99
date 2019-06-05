using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverMenu : MonoBehaviour
{
    public GameObject scoreText;
    public Text name;

    public HighscoreTable highscoreTable;

    public void PlayGame () 
    {
        SceneManager.LoadScene("World01big");
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
        //int score = int.Parse(scoreText.transform.GetComponent<TextMeshProUGUI>().text);

        highscoreTable.AddHighscoreEntry(10284934, "Mert");
        Debug.Log(score);
        Debug.Log(Name);

    }

}
