using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryPagesUnlocker : MonoBehaviour
{
    public static DiaryPagesUnlocker Instance { get; private set; }
    
    [SerializeField] private GameObject[] _diaryPages = new GameObject[16];
    
    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void EnableDiaryPage(int pageNumber)
    {
        //enable the page object by number
        _diaryPages[pageNumber].SetActive(true);
    }
}