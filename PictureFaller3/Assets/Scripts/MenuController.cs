using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
    }

    //Play Game is being used in Main Menu
    public void PlayGame()
    {
        print("changeScene");
        SceneManager.LoadScene("World01big");
    }

    public void MoveCharacter() {

        //if the script is connected to a player character, "animate" the character (this is used in main menu only)
        if (this.tag == "Player")
        {
            transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * 1f, 1), transform.position.z);
        }
    }
}
