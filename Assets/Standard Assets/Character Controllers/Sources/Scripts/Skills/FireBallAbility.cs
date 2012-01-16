using UnityEngine;
using System.Collections;

public class FireBallAbility : BaseAbility {
	
	public string projectPath = "ProjectilePrefabs/FireballPrefab";
	GameObject fireballPrefab;
	
	// Use this for initialization
	public override void Initialize (AbilitesCollection coll, GameCharacterController cont) {		
		base.Initialize(coll, cont);
		
		castTime = 1.0f;		
		skill = 10.0f;
		damage = 10.0f;
		resourceCost = 10.0f;
		globalCoolDown = 0.5f;
		relatedAbilityCoolDown = 0.6f;
		coolDownTimer = 1.0f;
		coolDownRemainder = 0.0f;
		coolingdown = false;
		
		fireballPrefab = (GameObject)Resources.Load(projectPath);
		image = (Texture2D)Resources.Load("Textures/Icons/class/warlock_icon");
		
		abilityID = 100;
	}
	
	// the actual cast of the ability
	public override bool UseAbility()
	{
		bool result = base.UseAbility();
		if (!result)
			return result;
		
		Vector3 startPos = controller.transform.position;
		startPos.y += 2.0f;
		startPos += controller.transform.forward*2.0f;
		
		GameObject projectile = (GameObject)Instantiate(fireballPrefab, startPos, controller.transform.rotation);
		
		projectile.GetComponent<FireballController>().Initialize();
		projectile.GetComponent<FireballController>().SetTarget(controller.GetSelectedObject());
		return result;
	}	
}
