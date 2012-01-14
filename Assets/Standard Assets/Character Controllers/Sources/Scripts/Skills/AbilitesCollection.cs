using UnityEngine;
using System.Collections;

public class AbilitesCollection : MonoBehaviour {

	int toolbarID = 0; // which of the four tool bars are active
	bool casting = false;  // are we in the middle of casting
	
	float globalCoolDownTimer = 0.0f;
	bool globalActive = false;
	
	BaseAbility[,] abilities;
	BaseAbility readiedAbility;
	BaseAbility castingAbility;
	
	// Use this for initialization
	void Start () {
		toolbarID = 0;
		
		abilities = new BaseAbility[1,1];
		abilities[0,0] = (BaseAbility)ScriptableObject.CreateInstance("FireBallAbility");
		foreach (BaseAbility ability in abilities)
		{
			ability.Initialize(this, GetComponent<GameCharacterController>());
		}
	}
	
	void FixedUpdate()
	{
		foreach (BaseAbility ability in abilities)
		{
			ability.FixedUpdate();
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
	public void StartAbility(int toolbaroffset)
	{
		if (casting || globalActive || toolbaroffset < 0 || toolbaroffset > 5)
			return;
		
		castingAbility = abilities[toolbarID, 0];
		castingAbility.StartAbility();
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
	
	public void FillAbilityBar(Texture2D[] abilityImages)
	{
		int counter = 0;
		foreach (BaseAbility ability in abilities)
			abilityImages[counter++] = ability.image;
	}
}
