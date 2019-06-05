using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverMenu : MonoBehaviour
{
    public GameObject scoreText;
    public void PlayGame () 
    {
        SceneManager.LoadScene("World01big");
    }

    public void LoadMenu () 
    {
        SceneManager.LoadScene("MainMenu");
    }
}
