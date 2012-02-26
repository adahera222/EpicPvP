using UnityEngine;
using System.Collections;

public class TargetTouch : TargetingType {

	public override void Initialize()
	{
		type = TargetingType.Type.RangeTouch;
	}
	
	// Update is called once per frame
	public override void Update () {
	
	}
}
