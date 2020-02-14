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
    public bool willMove = false;

    private Vector3 moveVel;
    private Vector3 fallVel;

    public float coolDown = 3.0f;

    private float currentCoolDown = 0.0f;
    private Rigidbody rig;
    private bool landed;


    private GameObject spirit;
    // Start is called before the first frame update
    void Start()
    {
        FindSpirit();
        rig = GetComponent<Rigidbody>();
        moveVel = new Vector3(speed, 0, 0);
        fallVel = new Vector3(0, -fallSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        TrackSpirit();
        Cooldown();

        if (landed == false)
        {
            Fall();
        }
    }

    private void Fall()
    {
        rig.MovePosition(transform.position + fallVel * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.GetComponent<Block>() != null)
        {
            landed = true;
            GetComponent<Scrollable>().enabled = true;
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
        if (Physics.Raycast(transform.position, (spirit.transform.position - transform.position), out hit, layerMask))
        {
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
