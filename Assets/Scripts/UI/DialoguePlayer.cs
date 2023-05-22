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
        StartCoroutine(WaitForFrameBeforeCrossFade(target, false));
    }
    
    public void DisableDialoguePage(int pageNumber)
    {
        var target = _dialoguePages[pageNumber];
        StartCoroutine(WaitForFrameBeforeCrossFade(target, true));
    }


    IEnumerator WaitForFrameBeforeCrossFade(GameObject target, bool disableGameObject)
    {
        if (!disableGameObject)
        {
            target.SetActive(true);
        }

        yield return null;
        yield return target.GetComponent<ColourCrossFader>().DoCrossfadeCoroutine();


        if (disableGameObject)
        {
            target.SetActive(false);
        }
    }
}
