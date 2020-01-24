using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float dropSpeed;
    public float yThreshold = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, -dropSpeed * Time.deltaTime, 0));

        if(transform.position.y < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
