using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    // This class should be used as a data class
    // and should only include static variables.


    /// <summary>
    /// This property allows the game to check whether
    /// the game should be paused or not.
    /// </summary>
    private static bool _paused = false;

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
}
