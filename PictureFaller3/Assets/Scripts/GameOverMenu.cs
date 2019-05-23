using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverMenu : MonoBehaviour
{
    public void PlayGame () 
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadMenu () 
    {
        SceneManager.LoadScene("MainMenu");
    }
}
