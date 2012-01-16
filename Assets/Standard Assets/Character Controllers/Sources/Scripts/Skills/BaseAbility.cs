using UnityEngine;
using System.Collections;

public class BaseAbility : ScriptableObject {
	
	protected float currCastingTime = 0.0f;
	protected bool casting = false;
	public float castTime = 1.0f;
	
	protected bool stopMovement = true; // if casting does it stop movement
	protected bool readied = true;
	protected AbilitesCollection collection;
	protected GameCharacterController controller;
	
	public float skill = 10.0f; // a value of 1-10 to aid different aspects for how powerful this spell is
	public float damage = 10.0f;
	
	public float resourceCost = 10.0f; // mana or whatever else could be thought up
	
	public float globalCoolDown = 0.5f;
	public float relatedAbilityCoolDown = 0.6f;	// how long untill can use an ability of a simialr type
	
	public float coolDownTimer = 1.0f;	// how long to reuse this ability
	protected float coolDownRemainder = 0.0f;
	protected bool coolingdown = false;
	
	public Texture2D image;	// the image that represents this ability
	
	protected int abilityID;
	public int ID
	{
		get { return abilityID; }
	}
	
	public virtual void Initialize(AbilitesCollection coll, GameCharacterController cont)
	{
		collection = coll;
		controller = cont;
		abilityID = -1;
	}
	
	public void FixedUpdate()
	{
		if (casting)
		{
			currCastingTime -= Time.deltaTime;
			if (currCastingTime <= 0.0f)
				FinishedCasting();
		}
		
		if (coolingdown)
		{
			coolDownRemainder -= Time.deltaTime;
			if (coolDownRemainder <= 0.0f)
				coolingdown = false;
		}
	}
	
	public virtual void FinishedCasting()
	{
		currCastingTime = 0.0f;
		casting = false;
		readied = true;
		
		coolingdown = true;
		coolDownRemainder = coolDownTimer;
		
		collection.AbilityFinished(this);
	}
	
	public virtual bool StartAbility()
	{
		if (coolingdown)
			return false;
		collection.AbilityStarted(globalCoolDown);
		
		casting = true;
		currCastingTime = castTime;
		return casting;
	}
	
	public virtual bool StopMovement()
	{
		return stopMovement;
	}
	
	// the actual cast of the ability, creates the fireball, swing the sword
	public virtual bool UseAbility()
	{
		if (!readied)
			return false;
		
		readied = false;
		return true;
	}	
}
