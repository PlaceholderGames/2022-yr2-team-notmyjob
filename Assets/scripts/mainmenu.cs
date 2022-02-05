using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class mainmenu : MonoBehaviour
{
    public void PlayGame()      //This FUnction will load the following scene from the menu
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   //Loads the level that is after the menu scene 
    
    }

    public void ExitGame()      //This function will quit the application
    {
        Application.Quit();    
    
    }
}
