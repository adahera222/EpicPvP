using UnityEngine;
using System.Collections;

public class AttDamage : AbilityAttribute {
	
	public float damageValue;
	public float damageCost;
	public int assocaitedStat; // fire, ice, poison, etc...
	
	// Update is called once per frame
	public override void Update () {
	
	}
	
	public override void Use(float strength) {
		
	}	
}
