using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryPagesUnlocker : MonoBehaviour
{
    [SerializeField] private GameObject[] _diaryPages = new GameObject[16];
    
    public void EnableDiaryPage(int pageNumber)
    {
        //enable the page object by number
        _diaryPages[pageNumber].SetActive(true);
    }
}
