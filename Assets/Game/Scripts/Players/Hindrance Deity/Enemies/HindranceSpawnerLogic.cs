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

        GameObject newSpawn = NetworkManager.Instance.InstantiateGameObject(enemyPrefab.name, spawnPosition, enemyPrefab.transform.rotation);
        newSpawn.transform.parent = this.gameObject.transform;

        dragonEnemyTracker.AddToEnemyList(newSpawn);

    }
}
