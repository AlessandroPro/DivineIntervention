using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObject : MonoBehaviour
{
    public ARSessionOrigin arOrigin;
    public GameObject placementObject;

    public ARRaycastManager raycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool isPlaced = false;

    public LayerMask blockMask;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if(DeviceManager.Instance.devMode || !DeviceManager.Instance.IsThisDevice(GameDevice.IPhoneAR))
        {
           this.enabled = false;
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPlaced)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();
        }
        else
        {
            CheckForTaps();
        }

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    private void CheckForTaps()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, blockMask))
            {
                Block block = hit.collider.gameObject.GetComponent<Block>();
                if (block)
                {
                    if(block.canMove)
                    {
                        block.toggleMove();
                        audioSource.Play();
                    }
                }
            }
        }
    }

    private void PlaceObject()
    {
        isPlaced = true;
    }

    private void UpdatePlacementIndicator()
    {
        if(placementPoseIsValid)
        {
            placementObject.SetActive(true);

            arOrigin.MakeContentAppearAt(placementObject.transform, placementPose.position, placementPose.rotation);
            
        }
        else
        {
            placementObject.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
        placementPoseIsValid = hits.Count > 0;

        if(placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            //placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}


