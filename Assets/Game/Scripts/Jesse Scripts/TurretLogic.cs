using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLogic : MonoBehaviour
{

    public float aimSpeed = 10.0f;

    public float rateOfFire = 2.0f;
    private float currentFireCooldown = 0.0f;

    public GameObject barrel;
    public GameObject projectile;

    private GameObject spirit;
    private Vector3 vFacing;
    private Vector3 vT2E;
    private Vector3 vResult;

    //public float dp;
    private float vDp;
    private Transform parentTransform;
    // Start is called before the first frame update
    void Start()
    {
        parentTransform = transform.parent;   
    }

    // Update is called once per frame
    void Update()
    {
        currentFireCooldown -= Time.deltaTime;
        if (spirit == null)
        {
            FindSpirit();
        }
        else
        {      
            AimLogicUpdateDot();
            OutOfBoundsCheck();
        }
    }

    private void OutOfBoundsCheck()
    {
        if (parentTransform.localPosition.y <= 0.0f)
        {
            Destroy(parentTransform.gameObject);
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

    private void AimLogicUpdateDot()
    {
        //Compute A and B and nomalize them

        vFacing = this.transform.right;
        vFacing.Normalize();

        vT2E = (spirit.transform.position - this.transform.position);
        vT2E.Normalize();

        //Do the work
        vDp = Vector3.Dot(this.transform.right, vT2E);


        if (vDp > -0.05f && vDp < 0.05f)
        {
            if (currentFireCooldown <= 0.0f)
            {
                Instantiate(projectile, barrel.transform.position, barrel.transform.rotation);
                currentFireCooldown = rateOfFire;
            }
        }
        else if (vDp > 0.0f)
        {
            this.transform.Rotate(transform.forward, (-aimSpeed * Time.deltaTime), Space.World);

        }
        else
        {
            this.transform.Rotate(transform.forward, (aimSpeed * Time.deltaTime), Space.World);
        }

    }
}
