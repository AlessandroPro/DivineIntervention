using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManMoveBehaviour : StateMachineBehaviour
{
    public LayerMask layerMask;
    public float lineOfSightCheckOffset = 0.5f;

    private LaserManLogic laserManLogic;
    private Transform laserMan;
    private Transform spirit;

    private bool checkLeft = true;
    private bool checkRight = true;

    private bool leftLoSFound = false;
    private bool rightLoSFound = false;

    private Vector3 checkLeftLoS;
    private Vector3 checkRightLoS;

    private int leftChecks;
    private int rightChecks;

    override public void OnStateEnter(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(laserManLogic == null)
        {
            laserManLogic = fsm.gameObject.GetComponentInParent<LaserManLogic>();

            Debug.Assert(laserManLogic != null, "LaserManLogic must not be null!");
        }

        if(laserMan == null)
        {
            laserMan = laserManLogic.transform;
        }
    }

    override public void OnStateUpdate(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(spirit == null)
        {
            spirit = laserManLogic.spirit.transform;
        }

        if (laserManLogic.lineOfSight)
        {
            fsm.SetTrigger("Fire");
            return;
        }

        if(laserManLogic.landed == false)
        {
            return;
        }

        checkLeft = true;
        checkRight = true;

        leftLoSFound = false;
        rightLoSFound = false;

        leftChecks = 0;
        rightChecks = 0;

        Vector3 offset = new Vector3(lineOfSightCheckOffset, 0, 0);



        while (checkLeft)
        {
            RaycastHit blockHit;
            Physics.Raycast(laserMan.position - offset, new Vector3(0, -1, 0), out blockHit, 1.05f, layerMask);

            if(blockHit.transform == null)
            {
                checkLeft = false;
                break;
            }

            RaycastHit spiritHitLeft;
            Physics.Raycast(laserMan.position - offset, (spirit.transform.position - (laserMan.position - offset)), out spiritHitLeft);

            if (spiritHitLeft.transform.GetComponent<WingedSpiritController>() != null)
            {
                leftLoSFound = true;
                checkLeftLoS = laserMan.position - offset;
                checkLeft = false;
                break;
            }
            else
            {
                offset.x += lineOfSightCheckOffset;
                leftChecks++;
            }
        }

        offset.x = lineOfSightCheckOffset;

        while (checkRight)
        {
            RaycastHit blockHit;
            Physics.Raycast(laserMan.position + offset, new Vector3(0, -1, 0), out blockHit, 1.2f, layerMask);

            if (blockHit.transform == null)
            {
                checkRight = false;
                break;
            }

            RaycastHit spiritHitRight;
            Physics.Raycast(laserMan.position + offset, (spirit.transform.position - (laserMan.position + offset)), out spiritHitRight);

            if (spiritHitRight.transform.GetComponent<WingedSpiritController>() != null)
            {
                rightLoSFound = true;
                checkRightLoS = laserMan.position + offset;
                checkRight = false;
                break;
            }
            else
            {
                rightChecks++;
                offset.x += lineOfSightCheckOffset;

                if(leftLoSFound && rightChecks > leftChecks)
                {
                    checkRight = false;
                }
            }
        }

        if(leftLoSFound && rightLoSFound)
        {
            if(leftChecks == rightChecks)
            {
                float leftdistance = Vector3.Distance(spirit.position, checkLeftLoS);
                float rightDistance = Vector3.Distance(spirit.position, checkRightLoS);

                if(leftdistance < rightDistance)
                {
                    laserManLogic.MoveLeft();
                }
                else
                {
                    laserManLogic.MoveRight();
                }
            }
            else
            {
                laserManLogic.MoveLeft();
            }
        }
        else if(leftLoSFound)
        {
            laserManLogic.MoveLeft();
        }
        else if(rightLoSFound)
        {
            laserManLogic.MoveRight();
        }
        else
        {
            laserManLogic.Stop();
        }
    }


}
