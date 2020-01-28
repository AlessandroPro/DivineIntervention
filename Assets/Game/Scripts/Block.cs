﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float dropSpeed;
    public float yThreshold = 0;

    private MeshRenderer mRenderer;

    public int outPlaneID = 0;
    public float moveDistance = 0;
    public bool insidePlane = false;

    private float zPosTarget = 0;

    // Start is called before the first frame update
    void Start()
    {
        mRenderer = GetComponent<MeshRenderer>();
        zPosTarget = outPlaneID * moveDistance;

        // Randomly select an out plane ID for the block if it was generated inside the 2D plane
        if(outPlaneID == 0)
        {
            insidePlane = true;
            if(Random.Range(0, 2) == 0)
            {
                outPlaneID = -1;
            }
            else
            {
                outPlaneID = 1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = transform.localPosition.x;
        float yPos = transform.localPosition.y - dropSpeed * Time.deltaTime;
        float zPos = Mathf.Lerp(transform.localPosition.z, zPosTarget, 0.1f);
        transform.localPosition = new Vector3(xPos, yPos, zPos);

        if(transform.localPosition.y < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void toggleMove()
    {
        if(insidePlane)
        {
            zPosTarget = moveDistance * outPlaneID;
        }
        else
        {
            zPosTarget = 0;
        }

        insidePlane = !insidePlane;
    }

    public void changeMaterial(Material mat)
    {
        mRenderer.material = mat;

        if (!mRenderer.enabled)
        {
            mRenderer.enabled = true;
        }
    }
}