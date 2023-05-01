using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Button : MonoBehaviour
{
    
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject returnButton;
    [SerializeField] private GameObject itemsButton;
    [SerializeField] private GameObject abilitiesButton;
    [SerializeField] private GameObject optionsButton;
    [SerializeField] private GameObject quitButton;
    // Start is called before the first frame update
    void Start()
    {
        //Scene_Manager.Load_Scene("Pause_Menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
