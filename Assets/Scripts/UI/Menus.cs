using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    
    bool _isPaused = false;
    
    void Update() // I need to fix this later to not use Update but it works for now
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            _pauseMenu.SetActive(true);
            Time.timeScale = isPaused ? 0 : 1;
        }
        if (isPaused == false)
        {
            Time.timeScale = 1;
        }
    }
    
    public bool isPaused
    {
        get { return _isPaused; }
        set { _isPaused = value; }
    }
}
