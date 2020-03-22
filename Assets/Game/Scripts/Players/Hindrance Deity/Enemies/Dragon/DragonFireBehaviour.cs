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
        foreach(GameObject block in dragon.destroyTargets)
        {
            if(block != null)
            {
                dragon.DestroyTarget(block);
            }
        }

        if(dragon.detectedSpiritBlock != null)
        {
            dragon.FreezeTarget(dragon.detectedSpiritBlock);
        }

        fsm.SetTrigger("Scan");
    }

}
