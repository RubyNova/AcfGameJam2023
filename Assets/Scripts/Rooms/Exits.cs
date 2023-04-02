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
        if (!collision.TryGetComponent<PlayerController>(out var controller))
        {
            return;
        }

        controller.transform.position = _roomToEnter.transform.position;
    }
}
