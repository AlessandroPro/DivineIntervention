using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawn : MonoBehaviour
{
    public MeshRenderer turretBodyRender;
    public SphereCollider sphereCollider;
    public MeshRenderer barrelRender;
    public Animator fsm;


    void Start()
    {
        turretBodyRender.enabled = false;
        sphereCollider.enabled = false;
        barrelRender.enabled = false;
        fsm.enabled = false;
       

        Destroy(this.gameObject, 0.5f);
    }

    private void OnDestroy()
    {
        turretBodyRender.enabled = true;
        sphereCollider.enabled = true;
        barrelRender.enabled = true;
        fsm.enabled = true;
    }
}
