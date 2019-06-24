using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            //This time scale is need for the character animation in the main menu
            //But it breaks the Pause menu if used in the main game, thus if statement
            Time.timeScale = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();

    }

    //Play Game is being used in Main Menu
    public void PlayGame()
    {
        SceneManager.LoadScene("World01big");
    }
    //Load the main menu
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }



    public void MoveCharacter() {

        //if the script is connected to a player character, "animate" the character (this is used in main menu only)
        if (this.tag == "Player")
        {
            transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * 1f, 1), transform.position.z);
        }
    }

    //If "Resume" has been clicked in the Pause Menu in game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

}
