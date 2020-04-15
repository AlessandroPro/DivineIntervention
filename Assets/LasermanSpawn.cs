using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LasermanSpawn : MonoBehaviour
{
    public float spawnTime = 0.5f;
    void Start()
    {
        Destroy(this.gameObject, spawnTime);        
    }

}
