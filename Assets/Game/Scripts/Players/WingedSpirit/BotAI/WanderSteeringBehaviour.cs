using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderSteeringBehaviour : SteeringBehaviourBase
{
	public float WanderDistance = 2.0f;
	public float WanderRadius = 1.0f;
	public float WanderJitter = 20.0f;

	private Vector3 WanderTarget;

	private void Start()
	{
		float theta = (float)Random.value * Mathf.PI * 2;
		WanderTarget = new Vector3(WanderRadius * (float)Mathf.Cos(theta),
								   0.0f,
								   WanderRadius * (float)Mathf.Sin(theta));
	}

	public override Vector3 calculateForce()
	{
		float jitterTimeSlice = WanderJitter * Time.deltaTime;

		WanderTarget = WanderTarget + new Vector3(Random.Range(-1.0f, 1.0f) * jitterTimeSlice,
												  0.0f,
												  Random.Range(-1.0f, 1.0f) * jitterTimeSlice);
		WanderTarget.Normalize();
		WanderTarget = WanderTarget * WanderRadius;

		target = WanderTarget + new Vector3(0, 0, WanderDistance);
		target = transform.rotation * target + transform.position;

		Vector3 wanderForce = target - transform.position;
		wanderForce.Normalize();
		wanderForce = wanderForce * steeringAgent.maxSpeed;

		return wanderForce;
	}

	private void OnDrawGizmos()
	{
		Vector3 circleCenter = transform.rotation * new Vector3(0.0f, 0.0f, WanderDistance) + transform.position;
		DebugExtension.DrawCircle(circleCenter, Vector3.forward, Color.green, WanderRadius);

		Debug.DrawLine(transform.position, target, debugColor);

		DebugExtension.DebugArrow(transform.position, transform.forward * WanderDistance, Color.red);
	}
}
