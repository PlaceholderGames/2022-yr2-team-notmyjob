using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{   
    //making it public so it's accessible for other functions
    public static bool isPaused = false;

    [SerializeField] GameObject PauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)      //checking is the game pasued
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()   
    {
        PauseMenuUI.SetActive(false);       //disables pausemenu canvas
        Time.timeScale = 1.0f;              //returns in game time to normal
        isPaused = false;
    }

    public void Pause()        //pausing the game
    {
        PauseMenuUI.SetActive(true);        //sets pausemenu canvas to active
        Time.timeScale = 0.0f;              //stops in game time
        isPaused = true;                    
    }

    public void MainMenu() 
    {
        SceneManager.LoadScene("DevRoom");

    }
    public void Quit()
    {
        Application.Quit();
    }
}
