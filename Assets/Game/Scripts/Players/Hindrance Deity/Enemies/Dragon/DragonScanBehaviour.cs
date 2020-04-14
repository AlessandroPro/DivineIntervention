using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonScanBehaviour : StateMachineBehaviour
{

    private DragonLogic dragon;

    private Transform spirit;

    private Dictionary<TurretLogic, GameObject> turretTargets = new Dictionary<TurretLogic, GameObject>();
    private Dictionary<LaserManLogic, GameObject> laserManTargets = new Dictionary<LaserManLogic, GameObject>();


    override public void OnStateEnter(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (dragon == null)
        {
            dragon = fsm.gameObject.transform.parent.GetComponent<DragonLogic>();
        }

        if (spirit == null)
        {
            spirit = GameManager.Instance.wingedSpirit.transform;
        }


        turretTargets.Clear();
        laserManTargets.Clear();
        dragon.destroyTarget = null;
        dragon.freezeTarget = null;
    }

    override public void OnStateUpdate(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (dragon.lifeOver)
        {
            fsm.SetTrigger("Leave");
            return;
        }


        List<GameObject> enemies = dragon.GetEnemyList();
        foreach (GameObject enemy in enemies)
        {

            if (enemy == null)
            {
                continue;
            }


            LaserManLogic laserMan = enemy.GetComponent<LaserManLogic>();

            if (laserMan != null)
            {
                if (laserMan.landed == false)
                {
                    continue;
                }

                if (laserMan.currentBlockBlocking == null || laserMan.currentBlockBlocking == laserMan.blockCol.gameObject)
                {
                    continue;
                }
                else
                {
                    laserManTargets.Add(laserMan, laserMan.currentBlockBlocking);
                }
            }

            //Lasermen are given priority over turrets
            if(laserManTargets.Count > 0)
            {
                continue;
            }

            TurretLogic turret = enemy.GetComponentInChildren<TurretLogic>();

            if (turret != null)
            {
                if(turret.currentBlockBlocking != null)
                {
                    turretTargets.Add(turret, turret.currentBlockBlocking);
                }

            }
        }

        float winningDistance = 0.0f;
        foreach(KeyValuePair<LaserManLogic, GameObject> target in laserManTargets)
        {
            float distance = Vector3.Distance(spirit.position, target.Key.transform.position);

            if(distance < winningDistance || winningDistance == 0.0f)
            {
                dragon.destroyTarget = target.Value;
                winningDistance = distance;
            }
        }


        if (laserManTargets.Count == 0)
        {
            foreach (KeyValuePair<TurretLogic, GameObject> target in turretTargets)
            {
                float distance = Vector3.Distance(spirit.position, target.Key.transform.position);

                if (distance < winningDistance || winningDistance == 0.0f)
                {
                    dragon.destroyTarget = target.Value;
                    winningDistance = distance;
                }
            }
        }

        if (dragon.detectedSpiritBlock != null)
        {
            if (dragon.detectedSpiritBlock.GetComponent<Block>().insidePlane == true)
            {
                dragon.freezeTarget = dragon.detectedSpiritBlock;
            }
        }




        if (dragon.destroyTarget != null || dragon.freezeTarget != null)
        {
            fsm.SetTrigger("fire");
        }


    }

}
