using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exits : MonoBehaviour
{
    public GameObject roomToEnter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<Collider2D>().IsTouching(GameObject.Find("Player").GetComponent<Collider2D>()))
        {
            GameObject.Find("Player").transform.position = roomToEnter.transform.position;
        }
    }
}
