using UnityEngine;
using System.Collections;

public class TimerType : ScriptableObject {
	
	public float maxTime = 0.0f;
	public float costPerUnit = 1.0f;
	
	// ability collection will have the handle to the prefab
	private CastingBarScript castingBarObject;
	
	protected float currRemainingTime = 0.0f;
	protected bool readied = true;
	
	public enum Type { None=0, Cast, Channeled, Charge, Instant }
	protected Type type = Type.None;
	public Type TypeID
	{
		get { return type; }
		set { type = value; }
	}
	
	public virtual void Initialize(CastingBarScript castingBarPrefab)
	{
		/*
		GameObject barobject = (GameObject)Resources.Load("GUIPrefabs/CastingBarPrefab");
		castingBarObject = barobject.GetComponent<CastingBarScript>();
		*/
		castingBarObject = (CastingBarScript)Instantiate(castingBarPrefab);
	}
	
	public virtual bool Start() {
		bool result = readied;
		if (readied)
		{
			currRemainingTime = maxTime;
			readied = false;
			result = true;
			
			switch (type)
			{
			case Type.Instant: // don't show casting bar for this type of ability
				break;
			
			case Type.Cast:
			case Type.Charge:
				castingBarObject.StartDisplay(0);
				break;
			case Type.Channeled:				
				castingBarObject.StartDisplay(currRemainingTime/maxTime );
				break;
			}
		}
		return result;
	}
	
	// Update is called once per frame
	public virtual bool Update () {
		bool result = false;
		if (isCasting())
		{
			currRemainingTime -= Time.deltaTime;
			if (currRemainingTime <= 0.0f)
				result = Finished();
			
			switch (type)
			{
			case Type.Instant: // don't show casting bar for this type of ability
				break;
			case Type.Cast:
			case Type.Charge:
				castingBarObject.UpdateDisplay((maxTime-currRemainingTime)/maxTime);
				break;
			case Type.Channeled:
				castingBarObject.UpdateDisplay(currRemainingTime/maxTime);
				break;
			}
		}
		return result;
	}
	
	public virtual bool Finished() {
		castingBarObject.Finished();
		currRemainingTime = 0.0f;
		readied = true;
		return readied;
	}
	
	public virtual float Cost()
	{
		return maxTime * costPerUnit;
	}
	
	// Strength of ability based on time, this really only applies to charged abilities
	public virtual float Strength()
	{
		float result = 1.0f;
		switch (type)
		{
		case Type.Charge:
			result = currRemainingTime / maxTime;
			break;
		}
		return result;
	}
	
	public bool isCasting()
	{
		return (currRemainingTime > 0.0f);
	}
	
	public bool StopsMovement()
	{
		return (maxTime > 0.5f) && type != Type.Charge;
	}
	
	public void Caceled()
	{
		currRemainingTime = 0.0f;
	}
}
