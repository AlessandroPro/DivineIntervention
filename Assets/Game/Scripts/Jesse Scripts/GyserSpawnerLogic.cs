using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyserSpawnerLogic : MonoBehaviour
{
    public GameObject gyserPrefab;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnGyser(Vector2 position)
    {
        //Vector3 spawnPosition = new Vector3(position.x, transform.position.y, 0.0f);
        //Instantiate(gyserPrefab, spawnPosition, gyserPrefab.transform.rotation);

        GameObject geyser = Instantiate(gyserPrefab, transform);
        geyser.transform.rotation = transform.rotation;
        geyser.transform.localPosition = new Vector3(position.x, 0, 0);
    }
}
