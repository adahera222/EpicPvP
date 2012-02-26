using UnityEngine;
using System.Collections;

public class Ability : ScriptableObject {

	public TargetingType targetType;
	public TimerType timerType;
	public AbilityAttribute[] attributes; 
	public float manaCost;
	
	public Texture2D image;	// the image that represents this ability
	
	private bool readyToUse = false;
	private bool usedAbilityLastFrame = false;
	private bool startedUsing = false;
	
	private enum UseStates { None=0, Start, Use };
	
	protected int abilityID;	
	public int ID
	{
		get { return abilityID; }
	}
	
	public virtual void Initialize(CastingBarScript castingBarPrefab)
	{
		abilityID = -1;
		targetType.Initialize();
		timerType.Initialize(castingBarPrefab);
		/*
		foreach (AbilityAttribute att in attributes)
			att.Initialize();
			*/
	}
	
	public virtual void TestInitialize(int type, CastingBarScript castingBarPrefab)
	{
		name = "TestAbility";
		targetType = (TargetingType)ScriptableObject.CreateInstance("TargetingType");
		timerType = (TimerType)ScriptableObject.CreateInstance("TimerType");
		switch (type)
		{
		case 0:
			timerType.maxTime = 3.0f;
			timerType.TypeID = TimerType.Type.Cast;
			break;
		case 1:
			timerType.maxTime = 3.0f;
			timerType.TypeID = TimerType.Type.Charge;
			break;
		case 2:
			timerType.maxTime = 3.0f;
			timerType.TypeID = TimerType.Type.Channeled;
			break;			
		}
		
		Initialize(castingBarPrefab);
		
		manaCost = 10.0f;
	}	
	
	// Update is called once per frame
	public void Update () {
		
		// if we are not casting or using this ability, just leave.
		if (!timerType.isCasting())
			return;
		
		switch (timerType.TypeID)
		{
		case TimerType.Type.Cast:
			readyToUse = timerType.Update();
			Debug.Log("Cast time updated! rdy: " + readyToUse);
			break;
		case TimerType.Type.Instant:
			timerType.Update();
			break;
		case TimerType.Type.Charge:
		case TimerType.Type.Channeled:
			// keep track if we stop activly using this ability so that it is
			// not prementaly set to readyToUse
			if (!usedAbilityLastFrame && readyToUse && startedUsing)
			{
				readyToUse = false;
			}
			Debug.Log("Ability updating - readyToUse:" + readyToUse);
			break;
		}
		usedAbilityLastFrame = false;
	}
	
	// Starts this ability up
	public virtual bool Start()
	{
		// Attempt to start the ability
		// thigns that can stop it
		// - It is still casting
		if (timerType.isCasting())
			return false;
		
		timerType.Start();
		Debug.Log("Ability started!");
		
		switch (timerType.TypeID)
		{
		case TimerType.Type.Cast:
			readyToUse = false;
			break;
		case TimerType.Type.Instant:
		case TimerType.Type.Charge:
		case TimerType.Type.Channeled:
			readyToUse = true;
			break;
		}
		
		return true;
	}	
	
	// the actual use of the ability
	public bool UseAbility()
	{
		Debug.Log("Attempting to use ability");
		if (readyToUse)
		{
			float strengthMult = timerType.Strength();
			Debug.Log("Ability strength modifier: " + strengthMult);
			
			/*
			foreach (AbilityAttribute att in attributes)
					att.Use(strengthMult);
					*/
			
			switch (timerType.TypeID)
			{
			case TimerType.Type.Cast:
			case TimerType.Type.Instant:
				Debug.Log("Ability USED and must be recasted to be used again!");
				readyToUse = false;
				break;
			case TimerType.Type.Charge:
			case TimerType.Type.Channeled:
				readyToUse = !timerType.Update();
				usedAbilityLastFrame = true;
				startedUsing = true;
				Debug.Log("Ability in USE readyToUse:" + readyToUse);
				break;
			}
		}
		return readyToUse;
	}
	
	// Does this ability stop movement?
	public bool StopsMovement()
	{
		return (timerType.isCasting() && timerType.StopsMovement());
	}
	
	public void Caceled()
	{
		timerType.Caceled();
		readyToUse = false;
	}
}
