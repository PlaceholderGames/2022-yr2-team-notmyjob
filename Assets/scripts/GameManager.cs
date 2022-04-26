using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    
    /// <summary>
    /// This property allows the game to check whether
    /// the game should be paused or not.
    /// </summary>
    private bool _paused = false;


    /// <summary>
    /// This property allows the game to globally access
    /// the player controller.
    /// </summary>
    private PlayerController _player = null;

    public KeyCode objectRotateButton = KeyCode.R;

    /// <summary>
    /// This method allows setting the paused
    /// property to be modified.
    /// </summary>
    /// <param name="value"></param>
    public void setPaused(bool value)
    {
        _paused = value;
    }

    /// <summary>
    /// This method returns the value of paused
    /// property.
    /// </summary>
    /// <returns></returns>
    public bool isPaused()
    {
        return _paused;
    }


    /// <summary>
    /// Get the Player<br></br>
    /// To access the GameObject, use: <c>GameManager.getPlayer().gameObject</c>
    /// </summary>
    /// <returns></returns>
    public PlayerController getPlayer()
    {
        return _player;
    }


    /// <summary>
    /// Set the player GameObject
    /// </summary>
    /// <param name="player"></param>
    public void setPlayer(PlayerController player)
    {
        _player = player;
    }

    public static GameManager getInstance()
    {
        return _instance;
    }

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        
        OnInstanceInitialized();
    }

    private void OnInstanceInitialized()
    {
        // Add ScalableObject component onto GameObjects with ScalableObject tag
        GameObject[] scalableObjects = GameObject.FindGameObjectsWithTag("ScalableObject");
        foreach (GameObject scalableObject in scalableObjects)
        {
            // Add ScalableObject component if it doesn't exist
            if (scalableObject.GetComponent<ScalableObject>() == null)
            {
                scalableObject.AddComponent<ScalableObject>();
            }
        }
    }
}
