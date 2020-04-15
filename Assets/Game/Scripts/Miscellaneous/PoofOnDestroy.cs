using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoofOnDestroy : MonoBehaviour
{
    public GameObject deathEffect;

    private void OnDestroy()
    {
        if (deathEffect != null)
        {
            GameObject newDeathEffect = Instantiate(deathEffect, transform.position, deathEffect.transform.rotation);
            Destroy(newDeathEffect, 2);
        }
    }
}
