using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject GameOverCanvas;
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    private float timeBeforeLoading =5f;
    private float timePassed;
    private bool tutorial;
    private GameObject character;
    static Animator anim;
    public bool isOutro;
    

    // Start is called before the first frame update
    void Start()
    {
        tutorial = false;
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            //This time scale is need for the character animation in the main menu
            //But it breaks the Pause menu if used in the main game, thus if statement
            Time.timeScale = 1;
        }



        anim = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().name == "Outro")
        {
            Time.timeScale = 1;
            anim.Play("waking");
        }

        if (SceneManager.GetActiveScene().name == "Intro") {
            anim.SetBool("isOutro", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();

        //After the intro has played fo 4 seconds, start loading the Tutorial on the background
        timePassed += Time.deltaTime;
        if (SceneManager.GetActiveScene().name == "Intro" && tutorial == false && timePassed >=4f)
        {
      

            StartCoroutine(LoadAsyncronousy("Tutorial"));
            tutorial = true;
        }

        if (SceneManager.GetActiveScene().name == "Outro" && timePassed >= 2f){
      
            GameOverCanvas.SetActive(true);
        }
    }

    //Load any scene on the background - will help loading bigger scenes faster, needs a scene name to be called
    IEnumerator LoadAsyncronousy(string Name) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(Name);
        while (operation.isDone == false) {
            Debug.Log(operation.progress);

            yield return null;
        }
    }


    //Play Game is being used in Main Menu
    public void PlayGame()
    {
        SceneManager.LoadScene("Intro");
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
