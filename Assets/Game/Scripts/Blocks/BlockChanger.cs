using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockChanger : MonoBehaviour
{
    public Material enterMat;
    public Material exitMat;
    public bool removeOutline = false;


    private void OnTriggerEnter(Collider other)
    {
        Block block = other.GetComponent<Block>();
        if (block)
        {
            block.changeMaterial(enterMat);
            if(removeOutline)
            {
                block.toggleOutline();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Block block = other.GetComponent<Block>();
        if (block)
        {
            block.changeMaterial(exitMat);
            if (removeOutline)
            {
                block.toggleOutline();
            }
        }
    }
}
