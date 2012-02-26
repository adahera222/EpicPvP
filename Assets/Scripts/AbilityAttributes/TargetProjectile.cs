using UnityEngine;
using System.Collections;

public class TargetProjectile : TargetingType {
	
	public float distanceUnitCost = 1.0f;
	public float speedUnitCost = 1.0f;	
	
	public override void Initialize()
	{
		type = TargetingType.Type.Projectile;
	}
	
	// Update is called once per frame
	public override void Update () {
	
	}	
}
