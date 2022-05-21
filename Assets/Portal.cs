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
    private string tag;
    // Start is called before the first frame update
    void Start()
    {
        tag = gameObject.tag;
        playerLayer = LayerMask.NameToLayer("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(" Entering collision layer" + other.gameObject.layer + " playerLayer" + playerLayer);
        if(other.gameObject.layer == playerLayer)
        {
            Player pl = (Player)other.gameObject.GetComponent<Player>();
            if(pl != null)
            {
                Debug.Log(" calling reverse gravity");
                pl.ReverseGravity();
            }
            else
            {
                Debug.LogWarning(" you put a player layer on an object that is not a player");
            }
        }
    }
}
