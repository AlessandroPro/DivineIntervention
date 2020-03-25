using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonEnemyTracker : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();

    public void AddToEnemyList(GameObject enemy)
    {
        enemyList.Add(enemy);

        ClearNull();
    }

    private void ClearNull()
    {
        for (var i = enemyList.Count - 1; i > -1; i--)
        {
            if (enemyList[i] == null)
                enemyList.RemoveAt(i);
        }
    }
}
