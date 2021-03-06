﻿using Photon.Pun;
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

    public GameObject destroyTarget;
    public GameObject freezeTarget;

    public float speed = 2.0f;
    public GameObject detectedSpiritBlock;

    public Transform attackSpawnLocation;



    private DragonEnemyTracker enemyTracker;
    private float distance;
    private Vector3 targetPosition;

    private Transform spirit;

    private PhotonView photonView;


    // Start is called before the first frame update
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        if(NetworkManager.Instance.IsViewMine(photonView) == false)
        {
            Destroy(transform.GetChild(0).GetComponent<Animator>());
            Destroy(this);
        }
    }

    void Start()
    {
        transform.position = spawnLocation;
        enemyTracker = transform.parent.GetComponent<DragonEnemyTracker>();

        if(GameManager.Instance.wingedSpirit != null)
        {
            spirit = GameManager.Instance.wingedSpirit.transform;
        }

        if(attackSpawnLocation == null)
        {
            Debug.LogError("Jaw not found. Using default spawn location");
            NetworkManager.Instance.DestroyGameObject(this.gameObject);
        }
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
        if(NetworkManager.Instance.IsViewMine(photonView) == false)
        {
            return;
        }

        RaycastHit hit;
        Physics.Raycast(spirit.position, new Vector3(0.0f, 1.0f, 0.0f), out hit, 4.0f);

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
        if (NetworkManager.Instance.IsViewMine(photonView))
        {
            GameObject newFireBall = NetworkManager.Instance.InstantiateGameObject(fireBall.name, attackSpawnLocation.position, Quaternion.Euler(Vector3.zero));

            FireballScript fireballScript = newFireBall.GetComponent<FireballScript>();
            fireballScript.SetTarget(block);
        }
    }

    public void FreezeTarget(GameObject block)
    {
        if (NetworkManager.Instance.IsViewMine(photonView))
        {
            GameObject newFreezeBall = NetworkManager.Instance.InstantiateGameObject(freezeBall.name, attackSpawnLocation.position, Quaternion.Euler(Vector3.zero));

            FreezeBallScript freezeballScript = newFreezeBall.GetComponent<FreezeBallScript>();
            freezeballScript.SetTarget(block);
        }
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

    public void DestroyDragon()
    {
        NetworkManager.Instance.DestroyGameObject(this.gameObject);
    }
}
