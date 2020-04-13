using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockChanger : MonoBehaviour
{
    public Material enterMat;
    public Material exitMat;
    public int planeID;


    private void OnTriggerEnter(Collider other)
    {
        Block block = other.GetComponent<Block>();
        if (block)
        {
            block.changeMaterial(enterMat);
            block.OnEnteredPlane(planeID);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Block block = other.GetComponent<Block>();
        if (block)
        {
            block.changeMaterial(exitMat);
            block.OnExitedPlane(planeID);
        }
    }
}
