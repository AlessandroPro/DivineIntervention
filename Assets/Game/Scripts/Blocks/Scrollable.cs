using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrollable : MonoBehaviour
{
    public float dropSpeed;



    private void Start()
    {
        dropSpeed = GameManager.Instance.scrollSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, -dropSpeed * Time.deltaTime, 0), Space.World);
    }
}
