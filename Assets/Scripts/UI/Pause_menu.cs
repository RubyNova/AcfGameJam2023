using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pause_Menu : MonoBehaviour
{
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    
    //when hovering over the button named "*Button", the object "H_*Layer" will be visible
    public void HoveringOverButton()
    {
        if (IsPointerOverUIObject())
        {
            GameObject.Find("H_" + EventSystem.current.currentSelectedGameObject.name + "Layer").GetComponent<CanvasGroup>().alpha = 1;
        }
        
        else if (!IsPointerOverUIObject())
        {
            GameObject.Find("H_" + EventSystem.current.currentSelectedGameObject.name + "Layer").GetComponent<CanvasGroup>().alpha = 0;
        }
    }
}