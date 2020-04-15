using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonIdleBehaviour : StateMachineBehaviour
{
    public float idleTime;
    private float currentIdleTime = 0.0f;

    private DragonLogic dragon;

    override public void OnStateEnter(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (dragon == null)
        {
            dragon = fsm.gameObject.transform.parent.GetComponent<DragonLogic>();
        }

        currentIdleTime = 0.0f;
    }

    override public void OnStateUpdate(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (dragon.lifeOver)
        {
            fsm.SetTrigger("Leave");
            return;
        }

        currentIdleTime += Time.deltaTime;


        if (currentIdleTime >= idleTime)
        {
            currentIdleTime = 0.0f;

            fsm.SetTrigger("Scan");
        }
    }

}
