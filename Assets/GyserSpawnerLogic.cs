using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyserSpawnerLogic : MonoBehaviour
{
    public GameObject gyserPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {

            Vector3 spawnPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, transform.position.y, 0.0f);
            

            Instantiate(gyserPrefab, spawnPosition, gyserPrefab.transform.rotation);
        }
    }
}
