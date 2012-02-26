using UnityEngine;
using System.Collections;

public class TargetSelf : TargetingType {

	public override void Initialize()
	{
		type = TargetingType.Type.Self;
	}
	
	// Update is called once per frame
	public override void Update () {
	
	}
}
