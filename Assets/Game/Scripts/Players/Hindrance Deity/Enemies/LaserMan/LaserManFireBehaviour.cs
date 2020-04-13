using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManFireBehaviour : StateMachineBehaviour
{
    private LaserManLogic laserManLogic;

    override public void OnStateEnter(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (laserManLogic == null)
        {
            laserManLogic = fsm.gameObject.GetComponentInParent<LaserManLogic>();
        }
    }

    override public void OnStateUpdate(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (laserManLogic.lineOfSight == false)
        {
            fsm.SetTrigger("Move");
        }

        if (laserManLogic.fireCooldown == false)
        {
            laserManLogic.Stop();
            laserManLogic.FireLaser();
        }
    }
}
