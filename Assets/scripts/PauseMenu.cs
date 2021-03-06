using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{   
    [SerializeField] GameObject PauseMenuUI;

    private void Start()
    {
        // Start by setting the PauseMenu UI visiblity
        // to whether the game is paused
        PauseMenuUI.SetActive(GameManager.getInstance().isPaused());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle whether game is paused
            // e.g If game is paused (true), set it to not true (false)
            GameManager.getInstance().setPaused(!GameManager.getInstance().isPaused());
        }

        // Only show pause menu if game is paused
        PauseMenuUI.SetActive(GameManager.getInstance().isPaused());

        // Convert boolean into int (Will turn false into 1, and true into 0)
        // Setting the timescale to the converted integer.
        Time.timeScale = Convert.ToInt32(!GameManager.getInstance().isPaused());

    }

    public void Resume()
    {
        GameManager.getInstance().setPaused(false);
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
