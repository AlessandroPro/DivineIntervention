using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBallScript : MonoBehaviour
{
    public float timeToReach = 1.0f;
    public bool isAR = false;


    private GameObject target;
    private Vector3 startPosition;
    private float t;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;

        if (isAR == false)
        {
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime / timeToReach;
        transform.position = Vector3.Lerp(startPosition, target.transform.position, t);

        if (isAR == false)
        {
            transform.localScale = new Vector3(t, t, t);
        }

        if (t >= 1.0f)
        {
            //Add freeze here

            Destroy(this.gameObject);
        }
    }

    public void SetTarget(GameObject _target)
    {
        target = _target;
    }
}
