using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFireBehaviour : StateMachineBehaviour
{
    private DragonLogic dragon;
    private Animator dragonAnimator;

    override public void OnStateEnter(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (dragon == null)
        {
            dragon = fsm.gameObject.transform.parent.GetComponent<DragonLogic>();
        }

        if(dragonAnimator == null)
        {
            dragonAnimator = dragon.GetComponent<Animator>();
        }

        dragonAnimator.SetTrigger("Attack");
    }

    override public void OnStateUpdate(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (dragon.destroyTarget != null)
        {
            dragon.DestroyTarget(dragon.destroyTarget);
        }
        else if(dragon.freezeTarget != null)
        {
            dragon.FreezeTarget(dragon.freezeTarget);
        }

        fsm.SetTrigger("Idle");
    }

}
