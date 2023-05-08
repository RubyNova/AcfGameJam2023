using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;

    bool _isPaused = false;

    public void Pause()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }
    
    public void Resume()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }
    
    public bool isPaused
    {
        get { return _isPaused; }
        set { _isPaused = value; }
    }
}
