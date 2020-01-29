using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawnerLogic : MonoBehaviour
{
    public GameObject turretPrefab;


    public void SpawnTurret(Vector2 position)
    {
        Vector3 spawnPosition = new Vector3(position.x, position.y, 0.0f);

        GameObject newTurret = Instantiate(turretPrefab, transform);
        newTurret.transform.localPosition = spawnPosition;
    }
}
