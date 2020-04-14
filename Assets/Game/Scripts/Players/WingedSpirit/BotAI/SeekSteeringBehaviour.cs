using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekSteeringBehaviour : SteeringBehaviourBase
{
	public Vector3 upTarget;
	public Vector3 downTarget;

	public float upThreshold;
	public float downThreshold;

	public override Vector3 calculateForce()
	{
		if(transform.position.y > upThreshold)
		{
			target = downTarget;
		}
		else if(transform.position.y < downThreshold)
		{
			target = upTarget;
		}

		Vector3 desiredVelocity = (target - transform.parent.position).normalized;
		desiredVelocity = desiredVelocity * steeringAgent.maxSpeed;
		return desiredVelocity - steeringAgent.velocity;
	}

	private void OnDrawGizmos()
	{
		DebugExtension.DebugWireSphere(target, debugColor);
	}
}
