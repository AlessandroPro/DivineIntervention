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
    public float timeInterval;

    public GameObject blockPrefab;

    private float timeElapsed = 0;

    public bool enableBlockAI = false;

    private int outPlaneID = 1;
    

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed > timeInterval)
        {
            generateBlock();
            timeElapsed = 0;
        }
    }

    private void generateBlock()
    {
        // Randomly choose if default position is in the plane or outside
        int blockPlaneID = outPlaneID;
        if(Random.Range(0, 2) == 0)
        {
            blockPlaneID = 0;
        }
        float generateDistance = (size * 0.5f) - (blockThickness * 0.5f);

        // Randomized block size
        int blockScale = (int) Random.Range(0.2f * size, 0.5f * size);
        float blockScaleX = 1;
        float blockScaleY = 1;

        int orientation = Random.Range(0, 2);
        //if(orientation == 0)
        //{
            blockScaleX = blockScale;
        //}
        //else
        //{
            //blockScaleY = blockScale;
        //}

        float blockHalfWidth = blockScaleX * 0.5f;

        // Randomized block plane and position
        float blockPosZ = blockPlaneID * generateDistance;
        float baseHalfWidth = size * 0.5f;
        float blockPosX = Random.Range(-baseHalfWidth + blockHalfWidth, baseHalfWidth - blockHalfWidth);
        float blockPosY = startHeight;

        Vector3 blockPos = new Vector3(blockPosX, blockPosY, blockPosZ);

        // Create block and set its properties
        if (only2D && blockPlaneID != 0)
        {
            return;
        }

        GameObject block = Instantiate(blockPrefab, transform);
        block.transform.localScale = new Vector3(blockScaleX, blockScaleY, blockThickness);
        block.transform.localPosition = blockPos;
        Block blockData = block.GetComponent<Block>();
        blockData.dropSpeed = dropSpeed;
        blockData.moveDistance = generateDistance;
        blockData.outPlaneID = blockPlaneID;

        ProtectionDeityAI protectionAi = block.GetComponent<ProtectionDeityAI>();

        if(protectionAi != null)
        {
            protectionAi.enabled = enableBlockAI;
        }
        
    }

    public void swapPlanes()
    {
        outPlaneID *= -1;


    }
}
