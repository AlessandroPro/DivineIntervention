using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrollable : MonoBehaviour
{
    public float dropSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, -dropSpeed * Time.deltaTime, 0), Space.Self);
    }
}
