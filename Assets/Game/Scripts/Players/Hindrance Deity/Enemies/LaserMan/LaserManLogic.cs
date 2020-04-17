using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class LaserManLogic : MonoBehaviour
{
    public GameObject laserPrefab;
    public Vector3 laserSpawnOffset;

    public float speed = 1.0f;
    public float fallSpeed = 3.0f;
    public float coolDown = 3.0f;
    public LayerMask layerMask;

    public GameObject currentBlockBlocking;

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

    private Animator animator;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        scroller = GetComponent<Scrollable>();

        if (NetworkManager.Instance.IsViewMine(photonView) == false)
        {
            Destroy(transform.GetChild(0).GetComponent<Animator>());
            Destroy(scroller);
            Destroy(this);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        FindSpirit();
        rig = GetComponent<Rigidbody>();
        moveVel = new Vector3(speed, 0, 0);
        fallVel = new Vector3(0, -fallSpeed, 0);
        animator = GetComponent<Animator>();


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
        animator.SetBool("Run Forward", true);

        rig.MovePosition(transform.position - moveVel * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    public void MoveRight()
    {
        animator.SetBool("Run Forward", true);

        rig.MovePosition(transform.position + moveVel * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    public void Stop()
    {
        animator.SetBool("Run Forward", false);
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<SpiritAttack>())
        {
            NetworkManager.Instance.DestroyGameObject(this.gameObject);
        }


        Block blockFound = other.transform.GetComponent<Block>();
        if (blockFound != null)
        {
            block = other.gameObject;
            blockCol = block.GetComponent<BoxCollider>();
            blockFound.FreezeBlockCall();
            ChangeLandState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<Block>() != null)
        {
            ChangeLandState(false);
        }
    }



    private void OutOfBoundsCheck()
    {
        if (NetworkManager.Instance.IsViewMine(photonView) == false)
        {
            return;
        }

        if (transform.localPosition.y <= 0.0f)
        {
            NetworkManager.Instance.DestroyGameObject(transform.gameObject);
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
        if(spirit == null)
        {
            return;
        }

        float left = this.transform.position.x - this.transform.localScale.x * 0.25f;
        float right = this.transform.position.x + this.transform.localScale.x * 0.25f;

        RaycastHit hit1;
        Physics.Raycast(new Vector3(left, this.transform.position.y, this.transform.position.z), (spirit.transform.position - transform.position), out hit1, layerMask);

        RaycastHit hit2;
        Physics.Raycast(new Vector3(right, this.transform.position.y, this.transform.position.z), (spirit.transform.position - transform.position), out hit2, layerMask);


        if (hit1.transform.GetComponent<Block>() == null && hit2.transform.GetComponent<Block>() == null)
        {
            lineOfSight = true;
            currentBlockBlocking = null;
        }
        else
        {
            if (hit1.transform.gameObject == hit2.transform.gameObject)
            {
                if(hit1.transform.GetComponent<Block>().canMove)
                {
                    currentBlockBlocking = hit1.transform.gameObject;
                }
            }
            else
            {
                currentBlockBlocking = null;
            }
            lineOfSight = false;
        }
    }

    public void FireLaser()
    {
        if(spirit == null)
        {
            return;
        }

        animator.SetBool("Run Forward", false);

        if (NetworkManager.Instance.IsViewMine(photonView) == false)
        {
            return;
        }

        GameObject laser = NetworkManager.Instance.InstantiateGameObject(laserPrefab.name, transform.position + laserSpawnOffset, transform.rotation);
        Vector3 target = spirit.transform.position - laser.transform.position;

        Quaternion newRot = Quaternion.FromToRotation(laser.transform.up, target);
        laser.transform.rotation = newRot;

        LaserLogic laserLogic = laser.GetComponentInChildren<LaserLogic>();

        laserLogic.SetCurrentBlock(blockCol);

        currentCoolDown = coolDown;

    }

    private void FindSpirit()
    {
         spirit = GameManager.Instance.wingedSpirit;
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
