using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
     
public class FollowPlayer : MonoBehaviour
{
    public GameObject tPlayer;
    public Transform tFollowTarget;
    private CinemachineVirtualCamera[] vcam;
     
    void Start()
    {
        vcam = GetComponents<CinemachineVirtualCamera>();
    }
    
    void Update()
    {
        if (tPlayer == null)
        {
            tPlayer = GameObject.FindWithTag("Player");
            if (tPlayer != null)
                for (int i = 0; i < vcam.Length; i++)
                {
                    vcam[i].Follow = tPlayer.transform;
                }
        }
    }
}