using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    
    bool _isPaused = false;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            _pauseMenu.SetActive(true);
            Time.timeScale = isPaused ? 0 : 1;
        }
    }
    
    public bool isPaused
    {
        get { return _isPaused; }
        set { _isPaused = value; }
    }
}
