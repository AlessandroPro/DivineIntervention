using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public float timeToReach = 1.0f;
    public bool isAR = false;


    private GameObject target;
    private Vector3 startPosition;
    private float t;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        if (DeviceManager.Instance.device == GameDevice.IPhoneAR || DeviceManager.Instance.editorDevice == GameDevice.IPhoneAR)
        {
            isAR = true;
        }
        else
        {
            isAR = false;
        }
            
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;

        if(isAR == false)
        {
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        t += Time.deltaTime / timeToReach;

        if (isAR == false)
        {
            transform.localScale = new Vector3(t, t, t);
        }

        if(NetworkManager.Instance.IsViewMine(photonView) == false)
        {
            return;
        }

        transform.position = Vector3.Lerp(startPosition, new Vector3(target.transform.position.x, target.transform.position.y, 0.0f), t);



        if(t >= 1.0f && NetworkManager.Instance.IsViewMine(photonView))
        {
            try
            {

                Block blockScript = target.GetComponent<Block>();

                if (blockScript.insidePlane == true)
                {
                    blockScript.DestroyBlockCall();
                }
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
            }

            NetworkManager.Instance.DestroyGameObject(this.gameObject);
        }
    }

    public void SetTarget(GameObject _target)
    {
        target = _target;
    }
}
