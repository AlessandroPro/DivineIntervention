using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvoidSteeringBehaviour : SteeringBehaviourBase
{
    public WingedSpiritController spirit;
    private Vector3 dashDir;

    public override Vector3 calculateForce()
    {
        return dashDir;
    }

    // Update is called once per frame
    void Update()
    {
        if(spirit.dashOnCooldown)
        {
            dashDir = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInChildren<GyserLogic>()
            || other.GetComponentInChildren<BulletLogic>()
            || other.GetComponentInChildren<TurretLogic>()
            || other.GetComponentInChildren<LaserLogic>()
            || other.GetComponentInChildren<LaserManLogic>())
        {
            spirit.sendAttack();
            spirit.dash();
            dashDir = transform.position - other.gameObject.transform.position;
            dashDir.Normalize();
            Debug.Log(other.gameObject.name + " hit");
        }
    }
}
