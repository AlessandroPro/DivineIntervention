using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehaviourBase : MonoBehaviour
{
	public float weight = 1.0f;
	public bool useMouseInput = true;
    public Vector3 target = Vector3.zero;
    public Color debugColor = Color.blue;

	public abstract Vector3 calculateForce();

	[HideInInspector] public SteeringAgent steeringAgent;
}
