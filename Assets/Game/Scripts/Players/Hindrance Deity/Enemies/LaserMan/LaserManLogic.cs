using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Scrollable))]
public class LaserManLogic : MonoBehaviour
{
    public GameObject laserPrefab;

    public float speed = 1.0f;
    public float fallSpeed = 3.0f;
    public float coolDown = 3.0f;
    public LayerMask layerMask;

    [HideInInspector]
    public bool lineOfSight = false;

    [HideInInspector]
    public bool fireCooldown = false;

    [HideInInspector]
    public bool landed;

    [HideInInspector]
    public BoxCollider blockCol;

    [HideInInspector]
    public GameObject spirit;


    private Vector3 moveVel;
    private Vector3 fallVel;


    private float currentCoolDown = 0.0f;
    private Rigidbody rig;



    private GameObject block;
    private Scrollable scroller;


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
        if (block != null)
        {
            if (blockCol.enabled == false)
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

    public void MoveLeft()
    {
        rig.MovePosition(transform.position - moveVel * Time.deltaTime);
    }

    public void MoveRight()
    {
        rig.MovePosition(transform.position + moveVel * Time.deltaTime);
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
            fireCooldown = true;
        }
        else
        {
            fireCooldown = false;
        }
    }

    private void TrackSpirit()
    {
        float left = this.transform.position.x - this.transform.localScale.x * 0.25f;
        float right = this.transform.position.x + this.transform.localScale.x * 0.25f;

        RaycastHit hit1;
        Physics.Raycast(new Vector3(left, this.transform.position.y, this.transform.position.z), (spirit.transform.position - transform.position), out hit1, layerMask);

        RaycastHit hit2;
        Physics.Raycast(new Vector3(right, this.transform.position.y, this.transform.position.z), (spirit.transform.position - transform.position), out hit2, layerMask);


        if (hit1.transform.GetComponent<Block>() == null && hit2.transform.GetComponent<Block>() == null)
        {
            lineOfSight = true;
        }
        else
        {
            lineOfSight = false;
        }
    }

    public void FireLaser()
    {
        GameObject laser = Instantiate(laserPrefab, transform.position, transform.rotation);
        Vector3 target = spirit.transform.position - laser.transform.position;

        Quaternion newRot = Quaternion.FromToRotation(laser.transform.up, target);
        laser.transform.rotation = newRot;

        LaserLogic laserLogic = laser.GetComponentInChildren<LaserLogic>();

        laserLogic.SetCurrentBlock(blockCol);

        currentCoolDown = coolDown;

    }

    private void FindSpirit()
    {
        WingedSpiritController spiritControl = FindObjectOfType<WingedSpiritController>();

        if (spiritControl != null)
        {
            spirit = spiritControl.gameObject;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    float left = this.transform.position.x - this.transform.localScale.x * 0.25f;
    //    float right = this.transform.position.x + this.transform.localScale.x * 0.25f;

    //    Vector3 leftEnd = new Vector3(left, this.transform.position.y - 1.05f, this.transform.position.z);

    //    Debug.DrawLine(new Vector3(left,this.transform.position.y , this.transform.position.z), leftEnd, Color.red);

    //    Vector3 rightEnd = new Vector3(right, this.transform.position.y - 1.05f, this.transform.position.z);

    //    Debug.DrawLine(new Vector3(right, this.transform.position.y, this.transform.position.z), rightEnd, Color.red);
    //}
}
