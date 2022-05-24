using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : Manager<GameManager>
{
    [Header("Set in inspector")]
    public Player[] players = new Player[2];

    private List<Enemy> deadEnemyList = new List<Enemy>();
    [Header("Set Dynamically")]
    public Transform[] checkPoints;

    public Action OnGameOver;
    void Start()
    {
        checkPoints = new Transform[2];

        Assert.IsNotNull(players[1], "Second Player not set in the editor");
        Assert.IsNotNull(players[0], "First p not set in the editor");
    }

    public static void CheckPointReached(Transform pos1, Transform pos2)
    {
        Get().checkPoints[0] = pos1;
        Get().checkPoints[1] = pos2;
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
            if (!enemy.IsAlive)
            {
                enemy.gameObject.SetActive(true);
                enemy.Respawn();
            }

        }
        M.deadEnemyList.Clear();
               
        if (M.checkPoints != null && M.checkPoints.Length == 2)
        {
            playerOne.Respawn(M.checkPoints[0]);
            playerTwo.Respawn(M.checkPoints[1]);
  
        }
        
        M.OnGameOver();
    }


}
