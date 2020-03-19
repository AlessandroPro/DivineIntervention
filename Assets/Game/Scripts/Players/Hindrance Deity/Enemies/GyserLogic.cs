using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyserLogic : MonoBehaviour
{
    public float riseSpeed = 5.0f;
    public float shrinkSpeed = 1.8f;
    public float lifeTime = 3.0f;
    public float damage = 5.0f;
    public float destroySpeed = 2.0f;


    private bool blockCollide;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (blockCollide == false)
        {
            MoveUp();
        }
        else
        {
            Shrink();
        }
        ReduceLifeTime();
    }

    private void ReduceLifeTime()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0.0f)
        {
            DestroyGyser();
        }
    }

    private void DestroyGyser()
    {
        this.gameObject.transform.localScale = new Vector3(transform.localScale.x - (destroySpeed * Time.deltaTime), transform.localScale.y, transform.localScale.z);
       

        if(transform.localScale.x <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void MoveUp()
    {
        if(blockCollide == true)
        {
            return;
        }

        transform.parent.localScale += new Vector3(0.0f, riseSpeed * Time.deltaTime, 0.0f);
    }

    private void Shrink()
    {
        transform.parent.localScale -= new Vector3(0.0f, shrinkSpeed * Time.deltaTime, 0.0f);

        if(transform.parent.localScale.y < 0.0f)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Block")
        {
            blockCollide = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        WingedSpiritController spiritCon = other.gameObject.GetComponent<WingedSpiritController>();

        if (spiritCon != null)
        {
            spiritCon.TakeDamage(damage);
        }
    }
}
