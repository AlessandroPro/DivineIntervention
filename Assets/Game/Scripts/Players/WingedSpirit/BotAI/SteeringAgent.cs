using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
	public enum SummingMethod
	{
		WeightedAverage,
		Prioritized,
		Dithered
	};
	public SummingMethod summingMethod = SummingMethod.WeightedAverage;

	public float mass = 1.0f;
	public float maxSpeed = 1.0f;
	public float maxForce = 10.0f;

	public Vector3 velocity = Vector3.zero;

	private List<SteeringBehaviourBase> steeringBehaviours = new List<SteeringBehaviourBase>();
	private float speed;

	public float angularDampeningTime = 5.0f;
	public float deadZone = 10.0f;

	[HideInInspector] public bool reachedGoal = false;

	private void Start()
	{
		steeringBehaviours.AddRange(GetComponentsInChildren<SteeringBehaviourBase>(true));
		foreach(SteeringBehaviourBase behaviour in steeringBehaviours)
		{
			behaviour.steeringAgent = this;
		}
	}


	private void Update()
	{
		Vector3 steeringForce = calculateSteeringForce();
		steeringForce.z = 0.0f;

		if (reachedGoal == true)
		{
			velocity = Vector3.zero;
		}
		else
		{
			Vector3 acceleration = steeringForce * (1.0f / mass);
			velocity = velocity + (acceleration * Time.deltaTime);
			velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

			speed = velocity.magnitude;
			//transform.position = transform.position + (velocity * Time.deltaTime);
		}

		if (velocity.magnitude > 0.0f)
		{
			float angle = Vector3.SignedAngle(transform.forward, velocity, Vector3.forward);
			//transform.RotateAround(transform.position, Vector3.forward, angle);

			if (Mathf.Abs(angle) <= deadZone)
			{
				//transform.LookAt(transform.position + velocity);
				transform.RotateAround(transform.position, Vector3.forward, angle);
			}
			else
			{
				transform.rotation = Quaternion.Slerp(transform.rotation,
													  Quaternion.LookRotation(velocity, -Vector3.forward),
													  Time.deltaTime * angularDampeningTime);
			}
		}
	}

	private Vector3 calculateSteeringForce()
	{
		Vector3 totalForce = Vector3.zero;

		foreach(SteeringBehaviourBase behaviour in steeringBehaviours)
		{
			if (behaviour.enabled)
			{
				switch(summingMethod)
				{
					case SummingMethod.WeightedAverage:
						totalForce = totalForce + (behaviour.calculateForce() * behaviour.weight);
						totalForce = Vector3.ClampMagnitude(totalForce, maxForce);
						break;

					case SummingMethod.Prioritized:
						break;
				}

			}
		}

		return totalForce;
	}

	public float getSpeed()
	{
		return speed;
	}

	private void OnDrawGizmos()
	{
		Debug.DrawLine(transform.position, transform.position + (transform.forward * 5), Color.red);
	}
}
