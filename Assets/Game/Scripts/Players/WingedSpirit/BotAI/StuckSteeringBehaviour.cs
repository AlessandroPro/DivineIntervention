using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckSteeringBehaviour : SteeringBehaviourBase
{

    private float stuckTimer = 0;
    public float maxStuckTime = 1;
    private float prevPosX;

    public override Vector3 calculateForce()
    {
        Vector3 force = Vector3.zero;

        if (stuckTimer > maxStuckTime)
        {
            if (Mathf.Abs(transform.position.x - prevPosX) < 2)
            {
                force = -transform.parent.forward;
            }
            prevPosX = transform.position.x;
            stuckTimer = 0;
        }

        stuckTimer += Time.deltaTime;
        return force;
    }

    // Start is called before the first frame update
    void Start()
    {
        prevPosX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
