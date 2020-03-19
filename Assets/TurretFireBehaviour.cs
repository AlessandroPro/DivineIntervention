using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFireBehaviour : StateMachineBehaviour
{

    private TurretLogic turretLogic;

    override public void OnStateEnter(Animator fsm, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (turretLogic == null)
        {
            turretLogic = fsm.gameObject.GetComponent<TurretLogic>();
        }
    }
    override public void OnStateUpdate(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (turretLogic.lineOfSight == false)
        {
            fsm.SetTrigger("Idle");
            return;
        }

        if (turretLogic.vDp < -0.05f || turretLogic.vDp > 0.05f)
        {
            fsm.SetTrigger("Turn");
            return;
        }

        if (turretLogic.fireCooldown == false)
        {
            turretLogic.Fire();
        }
    }
}
