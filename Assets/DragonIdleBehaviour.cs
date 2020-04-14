using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonIdleBehaviour : StateMachineBehaviour
{
    public float idleTime;
    private float currentIdleTime = 0.0f;

    override public void OnStateEnter(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentIdleTime = 0.0f;
    }

    override public void OnStateUpdate(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentIdleTime += Time.deltaTime;


        if (currentIdleTime >= idleTime)
        {
            currentIdleTime = 0.0f;

            fsm.SetTrigger("Scan");
        }
    }

}
