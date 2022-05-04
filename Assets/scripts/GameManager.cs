using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
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

    [SerializeField] private GameObject PauseUI;
    [SerializeField] private GameObject GameUI;

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

    public static string GetControllerName(KeyCode key, ControlTheme theme = ControlTheme.Dark)
    {
        return $"{key.ToString()}_Key_{theme.ToString()}";
    }
    
    private static KeyCode GetKeyCode(string keyName)
    {
        // Replace the spaces with underscores
        keyName = keyName.Replace(" ", "");
        
        // Capitalize the first letter
        keyName = keyName.Substring(0, 1).ToUpper() + keyName.Substring(1);
        return (KeyCode) Enum.Parse(typeof(KeyCode), keyName);
    }

    public static KeyCode[] GetKeyCodesFromAxis(string axisName)
    {
        List<KeyCode> keys = new List<KeyCode>();

        var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];  
        SerializedObject obj = new SerializedObject(inputManager);
        SerializedProperty axisArray = obj.FindProperty("m_Axes");
        if (axisArray.arraySize == 0)
            Debug.Log("No Axes");

        // Check if the axis exists
        bool axisFound = false;
        int axisIndex = 0;
        for (int i = 0; i < axisArray.arraySize; i++)
        {
            SerializedProperty tAxis = axisArray.GetArrayElementAtIndex(i);
            if(tAxis.displayName == axisName)
            {
                axisFound = true;
                break;
            }
            axisIndex++;
        }

        if (axisFound)
        {
            SerializedProperty axis = axisArray.GetArrayElementAtIndex(axisIndex);

            // Skip details
            axis.Next(true); //axis.displayName	"Name"	string
            axis.Next(false); //axis.displayName	"Descriptive Name"	string
            axis.Next(false); //axis.displayName	"Descriptive Negative Name"	string

            for (int i = 0; i < 4; i++)
            {
                // Add negative keys
                axis.Next(false); //axis.displayName	"Negative Button"	string
                if(axis.stringValue != "") keys.Add(GetKeyCode(axis.stringValue));
            }
        }
        return keys.ToArray();
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

    private void Update()
    {
        GameUI.SetActive(!_paused);
        PauseUI.SetActive(_paused);
    }
}

public enum ControlTheme
{
    [EnumMember(Value = "Light")]
    Light,
    [EnumMember(Value = "Dark")]
    Dark
}
