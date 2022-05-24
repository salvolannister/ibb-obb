using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public static class PortalType
//{
//    public static readonly string Green = "Green";
//    public static readonly string White = "White";
//    public static readonly string Red = "Red";
//}


public class Portal : MonoBehaviour
{
    [Header("Dynamically Set")]
    private int playerLayer;
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            Player pl = other.gameObject.GetComponent<Player>();
            if (pl != null)
            {
                pl.ReverseGravity();
            }
        }
    }
}
