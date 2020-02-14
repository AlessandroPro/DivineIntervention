using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTest : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(0.0f, 0.5f, 0.0f);
    }
}
