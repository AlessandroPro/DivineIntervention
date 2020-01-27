using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyserLogic : MonoBehaviour
{
    public float riseSpeed = 5.0f;
    public float lifeTime = 3.0f;
    private bool blockCollide;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveUp();
    }

    private void MoveUp()
    {
        if(blockCollide == true)
        {
            return;
        }

       transform.Translate(transform.up * riseSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Block")
        {
            blockCollide = true;
        }
    }
}
