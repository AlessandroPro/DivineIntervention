using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Scrollable))]
public class LaserManLogic : MonoBehaviour
{
    public LayerMask layerMask;
    public GameObject laserPrefab;

    public float speed = 1.0f;
    public float fallSpeed = 3.0f;
    public float coolDown = 3.0f;
    public bool willMove = false;


    private Vector3 moveVel;
    private Vector3 fallVel;


    private float currentCoolDown = 0.0f;
    private Rigidbody rig;
    private bool landed;

    private GameObject block;
    private BoxCollider blockCol;
    private Scrollable scroller;


    private GameObject spirit;
    // Start is called before the first frame update
    void Start()
    {
        FindSpirit();
        rig = GetComponent<Rigidbody>();
        moveVel = new Vector3(speed, 0, 0);
        fallVel = new Vector3(0, -fallSpeed, 0);
        scroller = GetComponent<Scrollable>();
    }


    // Update is called once per frame
    void Update()
    {
        CheckBlock();
        if (landed == false)
        {
            Fall();
        }
        else
        {
            TrackSpirit();
            Cooldown();
        }
        OutOfBoundsCheck();

    }

    private void CheckBlock()
    {
        if(block != null)
        {
            if(blockCol.enabled == false)
            {
                ChangeLandState(false);
                block = null;
            }
        }
    }

    private void ChangeLandState(bool state)
    {
        if (state == false)
        {
            landed = false;
            scroller.enabled = false;
        }
        else
        {
            landed = true;
            scroller.enabled = true;
        }
    }

    private void Fall()
    {
        rig.MovePosition(transform.position + fallVel * Time.deltaTime);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Block>() != null)
        {
            block = collision.gameObject;
            blockCol = block.GetComponent<BoxCollider>();
            ChangeLandState(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.GetComponent<Block>() != null)
        {
            ChangeLandState(false);
        }
    }



    private void OutOfBoundsCheck()
    {
        if (transform.localPosition.y <= 0.0f)
        {
            Destroy(transform.gameObject);
        }

    }

    private void Cooldown()
    {
        if (currentCoolDown > 0)
        {
            currentCoolDown -= Time.deltaTime;
        }
    }

    private void TrackSpirit()
    {

        RaycastHit hit;
        Physics.Raycast(transform.position, (spirit.transform.position - transform.position), out hit, layerMask);

        if (hit.transform.GetComponent<Block>() == null)
        {
            FireMahLaser();
        }
        else
        {
            if (willMove)
            {
                if (spirit.transform.position.x > transform.position.x)
                {
                    rig.MovePosition(transform.position + moveVel * Time.deltaTime);
                }
                else if (spirit.transform.position.x < transform.position.x)
                {
                    rig.MovePosition(transform.position + (moveVel * -1) * Time.deltaTime);
                }
            }

        }


    }

    private void FireMahLaser()
    {
        if (currentCoolDown <= 0)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, transform.rotation);
            Vector3 target = spirit.transform.position - laser.transform.position;

            Quaternion newRot = Quaternion.FromToRotation(laser.transform.up, target);
            laser.transform.rotation = newRot;

            currentCoolDown = coolDown;
        }
    }

    private void FindSpirit()
    {
        WingedSpiritController spiritControl = FindObjectOfType<WingedSpiritController>();

        if (spiritControl != null)
        {
            spirit = spiritControl.gameObject;
        }
    }
}
