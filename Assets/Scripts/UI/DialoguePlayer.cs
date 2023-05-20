using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] private GameObject[] _dialoguePages;
    
    //listen to the "DialogueTrigger" script to make it visible or not
    public void EnableDialoguePage(int pageNumber)
    {
        //enable the page object by number
        _dialoguePages[pageNumber].SetActive(true);
    }
}
