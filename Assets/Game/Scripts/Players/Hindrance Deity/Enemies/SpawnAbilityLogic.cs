using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAbilityLogic : MonoBehaviour
{
    public GameObject spawnPrefab;


    public void SpawnHindrance()
    {
        GameObject newSpawn = Instantiate(spawnPrefab, transform);
    }
}
