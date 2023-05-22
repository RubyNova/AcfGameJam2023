using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] private GameObject[] _dialoguePages;
    
    public void EnableDialoguePage(int pageNumber)
    {
        _dialoguePages[pageNumber].SetActive(true);
    }
    
    public void DisableDialoguePage(int pageNumber)
    {
        _dialoguePages[pageNumber].SetActive(false);
    }
}
