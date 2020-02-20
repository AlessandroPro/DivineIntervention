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
            generateBlock(0, 0.9f);
            generateBlock(1, 0.3f);
            timeElapsed = 0;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            swapPlanes(); 
        }
    }

    private void generateBlock(int orientation, float chance)
    {
        // Chance the block won't generate
        if (Random.value >= chance)
        {
            return;
        }

        // Randomly choose if default position is in the plane or outside
        int blockPlaneID = outPlaneID;
        if(Random.Range(0, 2) == 0)
        {
            blockPlaneID = 0;
        }

        if (only2D && blockPlaneID != 0)
        {
            blockPlaneID = 0;
        }

        float generateDistance = (size * 0.5f) - (blockThickness * 0.5f);

        // Randomized block size
        int blockScale = (int) Random.Range(0.2f * size, 0.7f * size);
        float blockScaleX = 1;
        float blockScaleY = 1;

        if (orientation == 0)
        {
            blockScaleX = blockScale;
            timeInterval = 2;
        }
        else
        {
            blockScaleY = blockScale;
            timeInterval = 5;
        }

        float blockHalfWidth = blockScaleX * 0.5f;

        // Randomized block plane and position
        float blockPosZ = blockPlaneID * generateDistance;
        float baseHalfWidth = size * 0.5f;
        float blockPosX = Random.Range(-baseHalfWidth + blockHalfWidth, baseHalfWidth - blockHalfWidth);
        float blockPosY = startHeight;

        Vector3 blockPos = new Vector3(blockPosX, blockPosY, blockPosZ);

        // Create block and set its properties


        GameObject block = Instantiate(blockPrefab, transform);
        block.transform.localScale = new Vector3(blockScaleX, blockScaleY, blockThickness);
        block.transform.localPosition = blockPos;
        Block blockData = block.GetComponent<Block>();
        blockData.dropSpeed = dropSpeed;
        blockData.moveDistance = generateDistance;
        blockData.outPlaneID = outPlaneID;

        if(blockPlaneID == 0)
        {
            blockData.insidePlane = true;
        }

        ProtectionDeityAI protectionAi = block.GetComponent<ProtectionDeityAI>();

        if(protectionAi != null)
        {
            protectionAi.enabled = enableBlockAI;
        }
    }

    public void swapPlanes()
    {
        outPlaneID *= -1;

        foreach (Block block in GetComponentsInChildren<Block>())
        {
            block.swapSides(outPlaneID);
        }
    }
}
