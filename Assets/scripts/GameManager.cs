using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    // This class should be used as a data class
    // and should only include static variables.


    /// <summary>
    /// This property allows the game to check whether
    /// the game should be paused or not.
    /// </summary>
    private static bool _paused = false;


    /// <summary>
    /// This property allows the game to globally access
    /// the player controller.
    /// </summary>
    private static PlayerController _player = null;

    public static KeyCode objectRotateButton = KeyCode.R;

    /// <summary>
    /// This method allows setting the paused
    /// property to be modified.
    /// </summary>
    /// <param name="value"></param>
    public static void setPaused(bool value)
    {
        _paused = value;
    }

    /// <summary>
    /// This method returns the value of paused
    /// property.
    /// </summary>
    /// <returns></returns>
    public static bool isPaused()
    {
        return _paused;
    }


    /// <summary>
    /// Get the Player<br></br>
    /// To access the GameObject, use: <c>GameManager.getPlayer().gameObject</c>
    /// </summary>
    /// <returns></returns>
    public static PlayerController getPlayer()
    {
        return _player;
    }


    /// <summary>
    /// Set the player GameObject
    /// </summary>
    /// <param name="player"></param>
    public static void setPlayer(PlayerController player)
    {
        _player = player;
    }
}
