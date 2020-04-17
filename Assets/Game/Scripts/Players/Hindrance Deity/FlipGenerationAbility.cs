using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlipGenerationAbility : MonoBehaviour
{

    //This should have an event that the block generator listens for
    public GameObject specialEffect;
    public BlockGenerator blockGenerator;

    public void FlipGeneration()
    {
        blockGenerator.SwapCall();
        GameObject newEffect = Instantiate(specialEffect, specialEffect.transform.position, specialEffect.transform.rotation);
        Destroy(newEffect, 0.8f);
    }
}
