using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilitesCollection : MonoBehaviour {
	bool casting = false;  // are we in the middle of casting
	
	float globalCoolDownTimer = 0.0f;
	bool globalActive = false;
	
	Dictionary<int, BaseAbility> abilities = new Dictionary<int, BaseAbility>();
	BaseAbility readiedAbility;
	BaseAbility castingAbility;
	
	void Awake()
	{		
		GameCharacterController gcc = GetComponent<GameCharacterController>();
		
		BaseAbility ab = (BaseAbility)ScriptableObject.CreateInstance("FireBallAbility");
		ab.Initialize(this, gcc);
		abilities.Add(ab.ID, ab);
	}
	
	// Use this for initialization
	void Start () {
	}
	
	void FixedUpdate()
	{
		foreach (KeyValuePair<int, BaseAbility> ability in abilities)
		{
			ability.Value.FixedUpdate();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (globalActive)
		{
			globalCoolDownTimer -= Time.deltaTime;
			if (globalCoolDownTimer <= 0.0f)
			{
				globalCoolDownTimer = 0.0f;
				globalActive = false;
			}
		}
	}
	
	// attempts to start an ability
	public void StartAbility(int abilityID)
	{
		if (casting || globalActive || abilityID < 0 )
			return;
		
		castingAbility = abilities[abilityID];
		castingAbility.StartAbility();
		SendMessage("OnCastSpell", castingAbility.castTime);
	}
	
	public void UseAbility()
	{
		if (readiedAbility)
			readiedAbility.UseAbility();
		readiedAbility = null;
	}
	
	public void ConfirmMovement(Vector3 moveDirection, out Vector3 modifiedMove)
	{
		if (casting)
			modifiedMove = Vector3.zero;
		else 
			modifiedMove = moveDirection;
	}
	
	public void ConfirmLookAt()
	{
	}
	
	// an ability has been started and now mark the variables to keep track of it
	public void AbilityStarted(float globalCooldown)
	{
		globalCoolDownTimer = globalCooldown;
		globalActive = true;
		casting = true;
	}
	
	public void AbilityFinished(BaseAbility readied)
	{
		readiedAbility = readied;
		casting = false;
		
	}
	
	public void FillAbilityBar(Texture2D[] abilityImages, int[] ids)
	{
		int counter = 0;
		foreach (int id in ids)
		{
			if (id != -1)
				abilityImages[counter] = abilities[id].image;
			counter++;
		}
	}
}
