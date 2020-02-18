using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlipGenerationAbility : MonoBehaviour
{

    //This should have an event that the block generator listens for
    public Image specialEffect;


    public void FlipGeneration()
    {
        Debug.Log("Flip triggered");
        //Instantiate(specialEffect, this.transform);
    }
}
