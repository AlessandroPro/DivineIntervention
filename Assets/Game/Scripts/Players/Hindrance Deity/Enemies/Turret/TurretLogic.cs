using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLogic : MonoBehaviour
{

    public float aimSpeed = 10.0f;

    public float rateOfFire = 2.0f;

    public LayerMask layerMask;

    public GameObject currentBlockBlocking;


    [HideInInspector]
    public bool lineOfSight = false;

    [HideInInspector]
    public bool fireCooldown = false;

    private float currentFireCooldown = 0.0f;

    public GameObject barrel;
    public GameObject projectile;

    public GameObject spirit;

    private bool destroyed = false;
    private float timeSincePlaced = 0;

    private Vector3 vFacing;
    private Vector3 vT2E;
    private Vector3 vResult;

    public float vDp;


    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        if(NetworkManager.Instance.IsViewMine(photonView) == false)
        {
            GetComponent<Animator>().enabled = false;
            Destroy(GetComponent<Scrollable>());
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentFireCooldown -= Time.deltaTime;

        if(currentFireCooldown < 0.0f)
        {
            fireCooldown = false;
        }
        else
        {
            fireCooldown = true;
        }

        if (spirit == null)
        {
            FindSpirit();
        }
        else
        {
            CheckFacing();
            CheckLineOfSight();
            OutOfBoundsCheck();
        }

        timeSincePlaced += Time.deltaTime;
    }

    private void CheckFacing()
    {
        vFacing = this.transform.right;
        vFacing.Normalize();

        vT2E = (spirit.transform.position - this.transform.position);
        vT2E.Normalize();

        //Do the work
        vDp = Vector3.Dot(this.transform.right, vT2E);
    }

    private void CheckLineOfSight()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, (spirit.transform.position - transform.position), out hit, layerMask);

        if (hit.transform.GetComponent<Block>() == null)
        {
            lineOfSight = true;
            currentBlockBlocking = null;
        }
        else
        {
            currentBlockBlocking = hit.transform.gameObject;
            lineOfSight = false;
        }
    }


    public void TurnLeft()
    {
        this.transform.Rotate(transform.forward, (-aimSpeed * Time.deltaTime), Space.World);
    }

    public void TurnRight()
    {
        this.transform.Rotate(transform.forward, (aimSpeed * Time.deltaTime), Space.World);
    }

    private void OutOfBoundsCheck()
    {
        if (transform.position.y <= 0.0f)
        {
            NetworkManager.Instance.DestroyGameObject(gameObject);
        }
    }

    public void Fire()
    {
        if (!destroyed && NetworkManager.Instance.IsViewMine(photonView))
        {
            NetworkManager.Instance.InstantiateGameObject("Bullet", barrel.transform.position, barrel.transform.rotation);
            currentFireCooldown = rateOfFire;
        }
    }

    private void FindSpirit()
    {
        WingedSpiritController spiritControl = FindObjectOfType<WingedSpiritController>();

        if(spiritControl != null)
        {
            spirit = spiritControl.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (NetworkManager.Instance.IsViewMine(photonView) == false)
        {
            return;
        }

        if (other.gameObject.GetComponent<SpiritAttack>())
        {
            NetworkManager.Instance.DestroyGameObject(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (NetworkManager.Instance.IsViewMine(photonView) == false)
        {
            return;
        }

        Block block = collision.gameObject.GetComponent<Block>();

        // Knocks the turret out of the 2D plane if hit by a block, then it is destroyed
        if (block != null && !destroyed)
        {
            destroyed = true;

            if (timeSincePlaced < 0.5f)
            {
                // TODO: this is only for turrets that spawn within blocks
                // Remove this once the Hindrance AI is smart enough to avoid this
                Destroy(gameObject);
            }
            else
            {
                Destroy(GetComponent<Rigidbody>());
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                rb.AddForce(Vector3.forward * 20 * block.outPlaneID * -1, ForceMode.Impulse);
                Destroy(gameObject, 2);
            }
        }
    }
}
