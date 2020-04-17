using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonLeaveBehaviour : StateMachineBehaviour
{
    public Vector3 goToLocation;
    private DragonLogic dragon;

    override public void OnStateEnter(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dragon = fsm.gameObject.transform.parent.GetComponent<DragonLogic>();

        if (dragon != null)
        {
            dragon.NewLocation(goToLocation);
        }
    }

    override public void OnStateUpdate(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (dragon.arrived)
        {
            dragon.DestroyDragon();
        }
    }
}
