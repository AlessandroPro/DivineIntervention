using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyserSpawnerLogic : MonoBehaviour
{
    public GameObject gyserPrefab;

    private GyserChargeScript gyserCharge;


    private void Start()
    {
        Transform child = gameObject.transform.GetChild(0);

        if(child != null)
        {
            gyserCharge = child.GetChild(0).GetComponent<GyserChargeScript>();
        }
        
    }

    public void SpawnGyser(Vector2 position, Transform wingedSpirit)
    {
        int verdict = gyserCharge.GyserRequest();

        if(verdict == 1)
        {
            GameObject geyser = NetworkManager.Instance.InstantiateGameObject(gyserPrefab.name, transform.position, transform.rotation);
            geyser.transform.rotation = transform.rotation;
            geyser.transform.localPosition = new Vector3(position.x, 0, 0);
        }
        else if(verdict == 2)
        {
            GameObject geyser = null;

            for (int i = -10; i < 12; i += 2)
            {
                geyser = NetworkManager.Instance.InstantiateGameObject(gyserPrefab.name, transform.position, transform.rotation);
                geyser.transform.rotation = transform.rotation;
                geyser.transform.localPosition = new Vector3(i, 0, 0);
            }
        }
    }
}
