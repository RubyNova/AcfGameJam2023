using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diary_Pages : MonoBehaviour
{
    [SerializeField] private GameObject _diaryPage;

    private List<GameObject> _pages = new List<GameObject>();
    
    public void AddPage(GameObject page)
    {
        // how to do this? 
        // What are the pages, and where is their textual data stored?
        
        // Should also unlock the button that opens the diary page
    }
    
    public void OpenDiary()
    {
        _diaryPage.SetActive(true);
    }
}
