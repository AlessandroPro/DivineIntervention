using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWaveLogic : MonoBehaviour
{
    private GameObject spawner;
    private GameObject gyserPrefab;
    void Start()
    {
        spawner = this.transform.parent.gameObject;
        gyserPrefab = spawner.GetComponent<GyserSpawnerLogic>().gyserPrefab;
    }

    // Update is called once per frame
    public void SpawnDeathWave(Vector2 position, Transform wingedSpirit)
    {
        GameObject geyser = null;

        for(int i = -10; i < 12; i += 2 )
        {
            geyser = NetworkManager.Instance.InstantiateGameObject(gyserPrefab.name, transform.position, transform.rotation);
            geyser.transform.rotation = transform.rotation;
            geyser.transform.localPosition = new Vector3(i, 0, 0);
        }
    }
}
