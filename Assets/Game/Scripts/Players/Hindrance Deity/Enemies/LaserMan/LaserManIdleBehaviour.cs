using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManIdleBehaviour : StateMachineBehaviour
{
    private LaserManLogic laserManLogic;

    override public void OnStateEnter(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (laserManLogic == null)
        {
            laserManLogic = fsm.gameObject.GetComponent<LaserManLogic>();
        }
    }

    override public void OnStateUpdate(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
