using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class EnemyParts
{
    public const string Spirit = "Spirit";
    public const string Crawler = "Crawler";
}

public class Enemy : MonoBehaviour
{
    /*
    Characters can kill an enemy
    Characters can be killed by enemy
    The enemies move around
    */
    public float velocity; 
    public bool IsAlive
    {
        get { return m_IsAlive; }
    }
    private bool boxTaken = false;
    private Vector3 originalPos;
    private bool m_IsAlive;

    void OnEnable()
    {
        m_IsAlive = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void Die()
    {
        Debug.Log(" Enemy died");
        m_IsAlive = false;
        GameManager.AddDeadEnemy(this);
        // Die effect
        // spawn box 

    }

    internal void Respawn()
    {
        transform.position = originalPos;
        m_IsAlive=true;
    }
}
