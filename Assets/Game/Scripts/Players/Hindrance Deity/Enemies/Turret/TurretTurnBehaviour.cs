using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTurnBehaviour : StateMachineBehaviour
{


    private TurretLogic turretLogic;

    private Transform spiritTransform;

    override public void OnStateEnter(Animator fsm, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if(turretLogic == null)
        {
            turretLogic = fsm.gameObject.GetComponent<TurretLogic>();
        }

        if (turretLogic != null && spiritTransform == null)
        {
            spiritTransform = turretLogic.spirit.transform;
        }
    }

    override public void OnStateUpdate(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (turretLogic.vDp > -0.05f && turretLogic.vDp < 0.05f)
        {
            if (turretLogic.lineOfSight == true)
            {
                fsm.SetTrigger("Fire");
            }
            else
            {
                fsm.SetTrigger("Idle");
            }
        }
        else if (turretLogic.vDp > 0.0f)
        {
            turretLogic.TurnLeft();
        }
        else
        {
            turretLogic.TurnRight();
        }
    }

}
