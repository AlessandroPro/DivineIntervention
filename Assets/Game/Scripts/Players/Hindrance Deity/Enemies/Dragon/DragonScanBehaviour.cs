using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonScanBehaviour : StateMachineBehaviour
{
    public float targetingTime;
    public int maxDestroyTargets = 4;

    private float currentTargetingTime = 0.0f;
    private DragonLogic dragon;




    override public void OnStateEnter(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (dragon == null)
        {
            dragon = fsm.gameObject.transform.parent.GetComponent<DragonLogic>();
        }

        dragon.destroyTargets.Clear();
    }

    override public void OnStateUpdate(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (dragon.lifeOver)
        {
            fsm.SetTrigger("Leave");
            return;
        }


        currentTargetingTime += Time.deltaTime;

        List<GameObject> enemies = dragon.GetEnemyList();
        foreach (GameObject enemy in enemies)
        {
            bool duplicate = false;

            if (enemy == null)
            {
                continue;
            }


            TurretLogic turret = enemy.GetComponentInChildren<TurretLogic>();

            if (turret != null)
            {
                foreach (GameObject target in dragon.destroyTargets)
                {
                    if (target == turret.currentBlockBlocking)
                    {
                        duplicate = true;
                        break;
                    }
                }

                if (duplicate == true)
                {
                    continue;
                }

                if (dragon.destroyTargets.Count == maxDestroyTargets)
                {
                    dragon.destroyTargets.RemoveAt(0);
                    dragon.destroyTargets.Add(turret.currentBlockBlocking);
                }
                else
                {
                    dragon.destroyTargets.Add(turret.currentBlockBlocking);
                }

                continue;
            }


            LaserManLogic laserMan = enemy.GetComponent<LaserManLogic>();

            if (laserMan != null)
            {
                if (laserMan.landed == false)
                {
                    continue;
                }

                if (laserMan.currentBlockBlocking == laserMan.blockCol.gameObject)
                {
                    continue;
                }


                foreach (GameObject target in dragon.destroyTargets)
                {
                    if (target == laserMan.currentBlockBlocking)
                    {
                        duplicate = true;
                        break;
                    }
                }

                if (duplicate == true)
                {
                    continue;
                }


                if (dragon.destroyTargets.Count == maxDestroyTargets)
                {
                    dragon.destroyTargets.RemoveAt(0);
                    dragon.destroyTargets.Add(laserMan.currentBlockBlocking);
                }
                else
                {
                    dragon.destroyTargets.Add(laserMan.currentBlockBlocking);
                }
            }
        }

        if (currentTargetingTime >= targetingTime)
        {
            currentTargetingTime = 0.0f;

            fsm.SetTrigger("Fire");
        }


    }

}
