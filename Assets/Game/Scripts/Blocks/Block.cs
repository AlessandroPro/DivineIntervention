using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviourPun, IPunObservable
{
    public float dropSpeed;
    public float yThreshold = 0;

    private MeshRenderer mRenderer;

    public int outPlaneID = 0;
    public float moveDistance = 0;
    public bool insidePlane = false;

    private float zPosTarget = 0;
    public GameObject blockOutline;

    // Start is called before the first frame update
    void Start()
    {
        mRenderer = GetComponent<MeshRenderer>();
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

        if (!mRenderer.enabled)
        {
            mRenderer.enabled = true;
        }
    }

    public void toggleOutline()
    {
        if(blockOutline)
        {
            blockOutline.SetActive(!blockOutline.activeSelf);
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
        }
        else
        {
            zPosTarget = (float)stream.ReceiveNext();
            outPlaneID = (int)stream.ReceiveNext();
            moveDistance = (float)stream.ReceiveNext();
            insidePlane = (bool)stream.ReceiveNext();
        }
    }
}
