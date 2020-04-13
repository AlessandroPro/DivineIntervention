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
        transform.position = Vector3.Lerp(startPosition, new Vector3(target.transform.position.x, target.transform.position.y, 0.0f), t);

        if(isAR == false)
        {
            transform.localScale = new Vector3(t, t, t);
        }

        if(t >= 1.0f)
        {
            if(target.GetComponent<Block>().insidePlane == true)
            {
               // Destroy(target);
            }

            Destroy(this.gameObject);
        }
    }

    public void SetTarget(GameObject _target)
    {
        target = _target;
    }
}
