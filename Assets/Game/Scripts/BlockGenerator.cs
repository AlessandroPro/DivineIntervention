using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockGenerator : MonoBehaviour
{
    public float size;
    public float blockThickness;
    public float startHeight;
    public bool only2D = false;
    public float dropSpeed;
    public float interval;

    public GameObject blockprefab;

    private float timeElapsed = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed > interval)
        {
            generateBlock();
            timeElapsed = 0;
        }
    }

    private void generateBlock()
    {
        int planeID = Random.Range(-1, 2);
        float generateDistance = (size * 0.5f) - (blockThickness * 0.5f);

        float blockPosZ = planeID * generateDistance;
        float blockPosX = Random.Range(-size * 0.5f, size *0.5f);
        float blockPosY = startHeight;

        Vector3 blockPos = (new Vector3(blockPosX, blockPosY, blockPosZ)) + transform.position;

        GameObject block = Instantiate(blockprefab, blockPos, Quaternion.identity);
        block.GetComponent<Block>().dropSpeed = dropSpeed;
    }
}
