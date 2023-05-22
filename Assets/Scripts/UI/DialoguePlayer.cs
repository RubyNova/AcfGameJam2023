using Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] private GameObject[] _dialoguePages;
    
    public void EnableDialoguePage(int pageNumber)
    {
        var target = _dialoguePages[pageNumber];
        target.SetActive(true);
        StartCoroutine(WaitForFrameBeforeCrossFade(target));
    }
    
    public void DisableDialoguePage(int pageNumber)
    {
        var target = _dialoguePages[pageNumber];
        target.SetActive(false);
        StartCoroutine(WaitForFrameBeforeCrossFade(target));
    }


    IEnumerator WaitForFrameBeforeCrossFade(GameObject target)
    {
        yield return null;
        target.GetComponent<ColourCrossFader>().DoCrossFade();
    }
}
