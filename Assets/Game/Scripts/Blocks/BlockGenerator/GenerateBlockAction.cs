using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GenerateBlockAction : Action
{
	private BlockGenerator blockGenerator;

	public override void OnStart()
	{
		blockGenerator = gameObject.GetComponent<BlockGenerator>();
	}

	public override TaskStatus OnUpdate()
	{
		if(blockGenerator == null)
		{
			Debug.LogError("The Block Generator game object is missing a Block Generator script!");
			return TaskStatus.Failure;
		}

		blockGenerator.generateBlock();
		return TaskStatus.Success;
	}
}