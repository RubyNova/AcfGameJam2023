using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exits : MonoBehaviour
{
    [SerializeField]
    private GameObject _roomToEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = PlayerController.Instance;
        if (collision.gameObject != player.gameObject)
        {
            return;
        }

        player.transform.position = _roomToEnter.transform.position;
    }
}
