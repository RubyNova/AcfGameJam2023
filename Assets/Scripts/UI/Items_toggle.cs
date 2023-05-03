using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items_toggle : MonoBehaviour
{
    // When toggled, if Items_1 is active then deactivate it and activate Items_2, else do the opposite
    public void OnClick()
    {
        if (GameObject.Find("Items_1").activeSelf)
        {
            GameObject.Find("Items_1").SetActive(false);
            GameObject.Find("Items_2").SetActive(true);
        }
        else
        {
            GameObject.Find("Items_1").SetActive(true);
            GameObject.Find("Items_2").SetActive(false);
        }
    }
}
