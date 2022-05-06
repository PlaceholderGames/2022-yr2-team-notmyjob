using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMusic : MonoBehaviour
{
    public AudioSource src;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (GameManager.getInstance().isPaused())
        {
            src.Pause();
        }
        else
        {
            src.UnPause();
        }
    }
}
