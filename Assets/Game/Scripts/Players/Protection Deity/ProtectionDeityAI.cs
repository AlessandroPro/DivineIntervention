using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionDeityAI : MonoBehaviour
{
    private float toggleTimer = 0;
    private float toggleInterval = 0;
    private bool isOn = true;

    private BoxCollider bc;
    private MeshRenderer mr;
    

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider>();
        mr = GetComponent<MeshRenderer>();
        toggleInterval = Random.Range(3.0f, 20.0f);
    }

    // Update is called once per frame
    void Update()
    {
        toggleTimer += Time.deltaTime;
        if(toggleTimer > toggleInterval)
        {
            if(isOn && bc.enabled == mr.enabled)
            {
                bc.enabled = false;
                mr.enabled = false;
                isOn = false;
            }
            else if(!isOn)
            {
                bc.enabled = true;
                mr.enabled = true;
                isOn = true;
            }
            toggleTimer = 0;
        }
    }
}
