using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesToggle : MonoBehaviour
{
    [System.Serializable] public class Abilities
    {
        public GameObject gameObject;
    }

    public Abilities[] abilities;
    
    void Start()
    {
        //for each child object thats name starts with "Ability_*", add it to the array"
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.StartsWith("Ability_"))
            {
                abilities[i].gameObject = transform.GetChild(i).gameObject;
            }
        }
    }

    //when the button "Button_L" is pressed, toggle to the ability on the left
    public void ToggleLeft()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i].gameObject.activeSelf)
            {
                abilities[i].gameObject.SetActive(false);
                if (i == 0)
                {
                    abilities[abilities.Length - 1].gameObject.SetActive(true);
                }
                else
                {
                    abilities[i - 1].gameObject.SetActive(true);
                }
                break;
            }
        }
    }
    
    //when the button "Button_R" is pressed, toggle to the ability on the right
    public void ToggleRight()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i].gameObject.activeSelf)
            {
                abilities[i].gameObject.SetActive(false);
                if (i == abilities.Length - 1)
                {
                    abilities[0].gameObject.SetActive(true);
                }
                else
                {
                    abilities[i + 1].gameObject.SetActive(true);
                }
                break;
            }
        }
    }
}