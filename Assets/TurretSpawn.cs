using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawn : MonoBehaviour
{
    public float spawnTime = 0.5f;
    public MeshRenderer turretBodyRender;
    public SphereCollider sphereCollider;
    public MeshRenderer barrelRender;
    public Animator fsm;


    void Start()
    {
        Destroy(this.gameObject, spawnTime);
    }

    private void OnDestroy()
    {
        turretBodyRender.enabled = true;
        sphereCollider.enabled = true;
        barrelRender.enabled = true;
        fsm.enabled = true;
    }
}
