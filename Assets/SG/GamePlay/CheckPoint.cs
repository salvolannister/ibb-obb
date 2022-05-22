using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPoint : MonoBehaviour
{
    [Header("Set dynamically")]
    [SerializeField]
    private bool isChecked = false;
    private int playerLayer;
    public void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Object entered {other.gameObject.name}");
        if (!isChecked && other.gameObject.layer == playerLayer)
        {
            Debug.Log("CheckPoint reached");
            GameManager.CheckPointReached(gameObject.transform.GetChild(0).transform, gameObject.transform.GetChild(1).transform);
            isChecked = true;
            gameObject.SetActive(false);
        }
    }
        
}
