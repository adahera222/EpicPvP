using UnityEngine;
using System.Collections;

public class TargetingType : ScriptableObject {
	
	public float baseCost = 1.0f;
	
	protected enum Type { None=0, Projectile, RangedInstant, RangeTouch, Self }
	protected Type type = Type.None;
	
	public virtual void Initialize()
	{
	}
	
	// Update is called once per frame
	public virtual void Update () {
	
	}
}
