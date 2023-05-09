using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Diary_Pages : MonoBehaviour
{
    [SerializeField] private List<GameObject> _pages;
    
    private static Diary_Pages _instance;

    public void AddPage(GameObject page)
    {
        _pages.Add(page);
    }
    
    public void OpenPage(int index)
    {
        //make the child object named "entry" visible, and the rest invisible
        for (int i = 0; i < _pages.Count; i++)
        {
            if (i == index)
            {
                _pages[i].SetActive(true);
            }
            else
            {
                _pages[i].SetActive(false);
            }
        }
    }
}
