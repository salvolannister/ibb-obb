using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : Manager<GameManager>
{
    [Header("Set in inspector")]
    public Transform[] checkPoints;
    public Player[] players = new Player[2];

    private int checkPointReached = 0;
    private List<Enemy> deadEnemyList = new List<Enemy>();

    void Start()
    {
        Assert.IsNotNull(players[1], "Second Player not set in the editor");
        Assert.IsNotNull(players[0], "First p not set in the editor");
        Assert.IsNotNull(checkPoints, "You didn't set any check point");
    }

    public static void CheckPointReached()
    {
        Get().checkPointReached++;
        Get().deadEnemyList.Clear();
    }

    public static void AddDeadEnemy(Enemy enemy)
    {
        Get().deadEnemyList.Add(enemy);
    }

    public static void GameOver(string deadTag)
    {
        GameManager M = Get();
        Player playerOne = M.players[0];
        Player playerTwo = M.players[1];
        if (playerOne.tag == deadTag)
        {
            playerOne.Die(() => playerTwo.Die());
        }

        // reactivate enemies
        foreach (Enemy enemy in M.deadEnemyList)
        {
            if (enemy.IsAlive)
            {
                enemy.gameObject.SetActive(true);
                enemy.Respawn();
            }

        }
        M.deadEnemyList.Clear();
        
        // respawn players! 
        int cpStartIndex = M.checkPointReached - 1;
        if (cpStartIndex >= 0 && cpStartIndex + 1 < M.checkPoints.Length)
        {
            playerOne.gameObject.transform.position = M.checkPoints[cpStartIndex].position;
            playerOne.gameObject.SetActive(true);
            playerTwo.gameObject.transform.position = M.checkPoints[cpStartIndex + 1].position;
            playerTwo.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError($" checkpoint index is wrong i:{cpStartIndex}");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
