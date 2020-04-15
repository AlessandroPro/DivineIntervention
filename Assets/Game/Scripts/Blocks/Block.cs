using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviourPun, IPunObservable
{
    public float dropSpeed;
    public float yThreshold = 0;

    public MeshRenderer mRenderer;

    public int outPlaneID = 0;
    public float moveDistance = 0;
    public bool insidePlane = false;
    public bool onlyShowIn2D = true;
    public bool canMove = true;

    private float zPosTarget = 0;
    public GameObject blockOutline;
    public Material frozenMat;

    // Start is called before the first frame update
    void Start()
    {
        mRenderer.enabled = false;
        blockOutline.SetActive(false);
        zPosTarget = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = transform.localPosition.x;
        float yPos = transform.localPosition.y - dropSpeed * Time.deltaTime;
        float zPos = Mathf.Lerp(transform.localPosition.z, zPosTarget, 0.1f);
        transform.localPosition = new Vector3(xPos, yPos, zPos);

        blockOutline.transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (transform.localPosition.y < 0 && NetworkManager.Instance.IsViewMine(photonView))
        {
            NetworkManager.Instance.DestroyGameObject(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        Destroy(blockOutline);
    }

    public void toggleMove()
    {
        if(insidePlane)
        {
            zPosTarget = moveDistance * outPlaneID;
        }
        else
        {
            zPosTarget = 0;
        }

        insidePlane = !insidePlane;
    }

    public void changeMaterial(Material mat)
    {
        mRenderer.material = mat;

        if (!mRenderer.enabled && !onlyShowIn2D)
        {
            mRenderer.enabled = true;
        }
    }

    public void swapSides(int planeID)
    {
        outPlaneID = planeID;

        if(!insidePlane)
        {
            zPosTarget = moveDistance * outPlaneID;
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(zPosTarget);
            stream.SendNext(outPlaneID);
            stream.SendNext(moveDistance);
            stream.SendNext(insidePlane);
            stream.SendNext(canMove);
        }
        else
        {
            zPosTarget = (float)stream.ReceiveNext();
            outPlaneID = (int)stream.ReceiveNext();
            moveDistance = (float)stream.ReceiveNext();
            insidePlane = (bool)stream.ReceiveNext();
            canMove = (bool)stream.ReceiveNext();
        }
    }

    public void OnEnteredPlane(int planeID)
    {
        if (planeID == 0 && insidePlane)
        {
            if (onlyShowIn2D)
            {
                mRenderer.enabled = true;
            }
            else
            {
                blockOutline.SetActive(false);
            }
        }
        else if(planeID != 0)
        {
            if (!onlyShowIn2D)
            {
                blockOutline.SetActive(true);
            }
        }
    }

    public void OnExitedPlane(int planeID)
    {
        if (planeID == 0)
        {
            if (onlyShowIn2D)
            {
                mRenderer.enabled = false;
            }
            else
            {
                blockOutline.SetActive(true);
            }
        }
    }

    //Called by the fireball
    public void DestroyBlockCall()
    {
        photonView.RPC("DestroyBlock", RpcTarget.All);
    }

    [PunRPC]
    private void DestroyBlock()
    {
        if (NetworkManager.Instance.IsViewMine(photonView))
        {
            NetworkManager.Instance.DestroyGameObject(this.gameObject);
        }
        Debug.Log("Block Destroyed");
    }

    //Called by the freezeball
    public void FreezeBlockCall()
    {
        photonView.RPC("FreezeBlock", RpcTarget.All);
    }

    [PunRPC]
    private void FreezeBlock()
    {
        mRenderer.material = frozenMat;
        canMove = false;
        Debug.Log("Block Frozen");
    }
}
