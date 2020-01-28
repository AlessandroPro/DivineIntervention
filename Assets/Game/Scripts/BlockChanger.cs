using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockChanger : MonoBehaviour
{
    public Material enterMat;
    public Material exitMat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Block block = other.GetComponent<Block>();
        if (block)
        {
            block.changeMaterial(enterMat);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Block block = other.GetComponent<Block>();
        if (block)
        {
            block.changeMaterial(exitMat);
        }
    }
}
