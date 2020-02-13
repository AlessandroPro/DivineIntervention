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
    

    // Start is called before the first frame update
    void Start()
    {

    }

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
        int planeID = Random.Range(-1, 2);
        float generateDistance = (size * 0.5f) - (blockThickness * 0.5f);

        // Randomized block size
        int blockScaleX = (int) Random.Range(0.2f * size, 0.7f * size);
        float blockHalfWidth = blockScaleX * 0.5f;
        int blockScaleY = (int)Random.Range(0.05f * size, 0.2f * size);

        // Randomized block plane and position
        float blockPosZ = planeID * generateDistance;
        float baseHalfWidth = size * 0.5f;
        float blockPosX = Random.Range(-baseHalfWidth + blockHalfWidth, baseHalfWidth - blockHalfWidth);
        float blockPosY = startHeight;

        Vector3 blockPos = new Vector3(blockPosX, blockPosY, blockPosZ);

        // Create block and set its properties
        if (only2D && planeID != 0)
        {
            return;
        }

        GameObject block = Instantiate(blockPrefab, transform);
        block.transform.localScale = new Vector3(blockScaleX, blockScaleY, blockThickness);
        block.transform.localPosition = blockPos;
        Block blockData = block.GetComponent<Block>();
        blockData.dropSpeed = dropSpeed;
        blockData.moveDistance = generateDistance;
        blockData.outPlaneID = planeID;

        ProtectionDeityAI protectionAi = block.GetComponent<ProtectionDeityAI>();

        if(protectionAi != null)
        {
            protectionAi.enabled = enableBlockAI;
        }
        
    }
}
