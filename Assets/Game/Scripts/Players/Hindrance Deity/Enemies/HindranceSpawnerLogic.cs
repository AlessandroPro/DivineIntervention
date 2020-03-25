using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HindranceSpawnerLogic : MonoBehaviour
{
    public GameObject enemyPrefab;
    public DragonEnemyTracker dragonEnemyTracker;


    public void SpawnHindrance(Vector2 position, Transform wingedSpirit)
    {
        Vector3 spawnPosition = new Vector3(position.x, position.y, 0.0f);

        GameObject newSpawn = Instantiate(enemyPrefab, transform);
        newSpawn.transform.localPosition = spawnPosition;

        dragonEnemyTracker.AddToEnemyList(newSpawn);

    }
}
