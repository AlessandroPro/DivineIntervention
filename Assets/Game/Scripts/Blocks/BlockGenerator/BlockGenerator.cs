using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockGenerator : MonoBehaviour
{
    public float size;
    public float minimumGapWidth;
    public float blockThickness;
    public float startHeight;
    public float dropSpeed;
    public float timeInterval;

    public GameObject blockPrefab;
    public bool enableBlockAI = false;

    private int outPlaneID = 1;

    public bool BlockGeneratorOn { get; set; } = false;
    public float RemainingSpace { get; set; }
    public float Size { get { return size; } set { size = value; } }
    public float MinBlockWidth { get; set; }


    private void Awake()
    {
        dropSpeed = GameManager.Instance.scrollSpeed;
        RemainingSpace = size;
        MinBlockWidth = size * 0.15f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
           swapPlanes(); 
        }
    }

    public void startGeneratingBlocks()
    {
        BlockGeneratorOn = true;
    }

    public void stopGeneratingBlocks()
    {
        BlockGeneratorOn = false;
    }

    public void generateBlock()
    {

        // Randomly choose if default position is in the plane or outside
        int blockPlaneID = outPlaneID;
        if(Random.Range(0, 2) == 0)
        {
            blockPlaneID = 0;
        }

        float generateDistance = (Size * 0.5f) - (blockThickness * 0.5f);

        // Randomized block size
        int blockScale = (int) Random.Range(MinBlockWidth, Mathf.Min(RemainingSpace, 0.7f * Size));
        float blockScaleX = 1;
        float blockScaleY = 1;

        blockScaleX = blockScale;

        float blockHalfWidth = blockScaleX * 0.5f;

        // Randomized block plane and position
        float blockPosZ = blockPlaneID * generateDistance;

        float blockPosX = -(Size * 0.5f) + (Size - RemainingSpace) + Random.Range(blockHalfWidth, RemainingSpace - blockHalfWidth);
        float blockPosY = startHeight;
        RemainingSpace = (Size * 0.5f) - blockPosX - blockHalfWidth - minimumGapWidth;

        Vector3 blockPos = new Vector3(blockPosX, blockPosY, blockPosZ);

        // Create block and set its properties

        GameObject block = NetworkManager.Instance.InstantiateGameObject("Block", transform.position, transform.rotation);
        block.transform.parent = transform;
        block.transform.localScale = new Vector3(blockScaleX, blockScaleY, blockThickness);
        block.transform.localPosition = blockPos;
        Block blockData = block.GetComponent<Block>();
        blockData.dropSpeed = dropSpeed;
        blockData.moveDistance = generateDistance;
        blockData.outPlaneID = outPlaneID;
        blockData.onlyShowIn2D = false;

        if (blockPlaneID == 0)
        {
            blockData.insidePlane = true;
        }

        ProtectionDeityAI protectionAi = block.GetComponent<ProtectionDeityAI>();

        if (protectionAi != null)
        {
            //protectionAi.enabled = enableBlockAI;
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
