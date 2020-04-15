using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonScanBehaviour : StateMachineBehaviour
{

    private DragonLogic dragon;

    private Transform spirit;

    private List<TurretLogic> turretsBlocked = new List<TurretLogic>();
    private List<LaserManLogic> laserMenBlocked = new List<LaserManLogic>();


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


        turretsBlocked.Clear();
        laserMenBlocked.Clear();
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
                    laserMenBlocked.Add(laserMan);
                }
            }

            //Lasermen are given priority over turrets
            if(laserMenBlocked.Count > 0)
            {
                continue;
            }

            TurretLogic turret = enemy.GetComponent<TurretLogic>();

            if (turret != null)
            {
                if(turret.currentBlockBlocking != null)
                {
                    turretsBlocked.Add(turret);
                }

            }
        }

        float winningDistance = Mathf.Infinity;

        foreach(LaserManLogic laserMan in laserMenBlocked)
        {

            if (laserMan == null)
            {
                continue;
            }

            float distance = Vector3.Distance(spirit.position, laserMan.transform.position);

            if(distance < winningDistance)
            {
                dragon.destroyTarget = laserMan.currentBlockBlocking;
                winningDistance = distance;
            }
        }


        if (laserMenBlocked.Count == 0)
        {
            foreach (TurretLogic turret in turretsBlocked)
            {
                if(turret == null)
                {
                    continue;
                }

                float distance = Vector3.Distance(spirit.position, turret.transform.position);

                if (distance < winningDistance)
                {
                    dragon.destroyTarget = turret.currentBlockBlocking;
                    winningDistance = distance;
                }
            }
        }

        if (dragon.detectedSpiritBlock != null)
        {
            Block block = dragon.detectedSpiritBlock.GetComponent<Block>();

            if (block != null)
            {
                if (block.insidePlane == true && block.canMove == true)
                {
                    dragon.freezeTarget = dragon.detectedSpiritBlock;
                }
            }
        }


        if (dragon.destroyTarget != null || dragon.freezeTarget != null)
        {
            fsm.SetTrigger("Fire");
        }


    }

}
