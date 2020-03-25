using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonLogic : MonoBehaviour
{
    public Vector3 spawnLocation;

    public float lifeTime = 15.0f;
    private float curretLifeTime = 0.0f;
    public bool lifeOver = false;


    public GameObject freezeBall;
    public GameObject fireBall;
    
    public bool arrived = true;

    public List<GameObject> destroyTargets = new List<GameObject>();

    public float speed = 2.0f;
    public GameObject detectedSpiritBlock;



    private DragonEnemyTracker enemyTracker;
    private float distance;
    private Vector3 targetPosition;

    private Transform spirit;




    // Start is called before the first frame update
    void Start()
    {
        transform.position = spawnLocation;
        spirit = GameObject.Find("WingedSpirit").transform;
        enemyTracker = transform.parent.GetComponent<DragonEnemyTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        SpiritRayCast();

        if(arrived == false)
        {
            MoveToLocation();
        }

        curretLifeTime += Time.deltaTime;

        if(curretLifeTime >= lifeTime)
        {
            lifeOver = true;
        }
    }

    private void SpiritRayCast()
    {
        RaycastHit hit;
        Physics.Raycast(spirit.transform.position, new Vector3(0.0f, 1.0f, 0.0f), out hit, 4.0f);

        if (hit.transform != null && hit.transform.gameObject.GetComponent<Block>() != null)
        {
            detectedSpiritBlock = hit.transform.gameObject;
        }
        else
        {
            detectedSpiritBlock = null;
        }
    }

    public void DestroyTarget(GameObject block)
    {
        GameObject newFireBall = Instantiate(fireBall, new Vector3(transform.position.x, transform.position.y + 9, transform.position.z), Quaternion.Euler(Vector3.zero));

        FireballScript fireballScript = newFireBall.GetComponent<FireballScript>();
        fireballScript.SetTarget(block);

    }

    public void FreezeTarget(GameObject block)
    {
        GameObject newFreezeBall = Instantiate(freezeBall, new Vector3(transform.position.x, transform.position.y + 9, transform.position.z), Quaternion.Euler(Vector3.zero));

        FreezeBallScript freezeballScript = newFreezeBall.GetComponent<FreezeBallScript>();
        freezeballScript.SetTarget(block);
    }

    public void NewLocation(Vector3 position)
    {
        targetPosition = position;
        arrived = false;
    }

    public List<GameObject> GetEnemyList()
    {
        return enemyTracker.enemyList;
    }

    private void MoveToLocation()
    {
        if (targetPosition != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            CheckDistance();
        }

    }

    private void CheckDistance()
    {
        distance = Vector3.Distance(transform.position, targetPosition);
        if (distance <= 0.1)
        {
            arrived = true;
        }
    }
}
