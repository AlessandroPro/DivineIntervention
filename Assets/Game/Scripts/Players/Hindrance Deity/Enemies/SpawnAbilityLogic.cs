using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAbilityLogic : MonoBehaviour
{
    public GameObject spawnPrefab;


    public void SpawnHindrance()
    {
        GameObject newSpawn = NetworkManager.Instance.InstantiateGameObject(spawnPrefab.name, transform.position, spawnPrefab.transform.rotation);
        newSpawn.transform.parent = this.gameObject.transform;
    }
}
