using UnityEngine;
using System.Collections;

public class InputGrabber : MonoBehaviour {
	ActionDispatcher actionDispatcher;
	
	EffectsCollection effects;
	AbilitesCollection abilities;
	AbilityController abilityControler;
	
	Vector3 lastScreenPos;
	
	public Texture2D[] icons;
	private int[] abilityIDs;
	public Rect toolBarLocation;
	
	private int toolbarbutton = -1;
	private int clickedToolbarbutton = -1;
	
	public CastingBarScript castingBarPrefab;
	private CastingBarScript castingBarObject;
	
	// Use this for initialization
	void Start () {
		actionDispatcher = GameObject.Find("Level").GetComponent<ActionDispatcher>();
		
		effects = GetComponent<EffectsCollection>();
		abilities = GetComponent<AbilitesCollection>();
		abilityControler = GetComponent<AbilityController>();
				
		icons = new Texture2D[6];
		
		int[] indicies = {0,1,2,3,4,5};
		abilityIDs = new int[6];
		for (int idx = 0; idx<6; ++idx)
			abilityIDs[idx] = -1;
		abilityIDs[1] = 100;
		abilityIDs[3] = 100;
		abilities.FillAbilityBar(icons, abilityIDs);
		
		castingBarObject = (CastingBarScript)Instantiate(castingBarPrefab);
		
		/*
		Texture2D tex2d = (Texture2D)Resources.Load("Textures/Icons/class/warlock_icon");		
		GameObject testobj = new GameObject("btn1");
		GUITexture tgt = (GUITexture)testobj.AddComponent("GUITexture");
		tgt.texture = tex2d;
		tgt.transform.localScale = Vector3.zero;
		tgt.transform.position = Vector3.zero;
		tgt.pixelInset = new Rect(10,10,30,30);
		*/
	}
	
	// Update is called once per frame
	void Update () {		
		Rect finallocation = new Rect();
  		finallocation.x = toolBarLocation.x * Screen.width;
  		finallocation.width = toolBarLocation.width * Screen.width;
	  	
  		finallocation.y = toolBarLocation.y * Screen.height;
  		finallocation.height = toolBarLocation.height * Screen.height;
		
		// The difference between the real mouse pos and what the GUI thinks it is, is 
		//	that the GUI thinks the 'y' should be flipped
		Vector3 windowMousePos = Input.mousePosition;
		windowMousePos.y = Screen.height - windowMousePos.y;
		
		// No speed modifications or anything of the like are done here because that will be handled by the server.
		// values will be either -1, 0, or 1 in keyboard mode
		Vector3 moveDirection = Vector3.zero;
  		moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
		
		Vector3 selectorPos = new Vector3(-1,-1,-1);
		bool mouseOverGUI;
		mouseOverGUI = finallocation.Contains(windowMousePos);
		
		// Using get axis raw here too becuase "GetButton" only returns true on the frist
		//	cycle of when it was actually pressed and won't return true untill it is pressed again.
		Vector3 viewChange = Vector3.zero;
		if (Input.GetAxisRaw("LookAtPoint") != 0 && !mouseOverGUI)
			viewChange = Input.mousePosition - lastScreenPos;
		lastScreenPos = Input.mousePosition;
		
		// this is a left click of the mouse and is used for selecting/targeting
		if (Input.GetButton("Cast") && !mouseOverGUI)
		{
			selectorPos = Input.mousePosition;
		}
		
		toolbarbutton = -1;
		if (Input.GetButton("Skill1"))
			toolbarbutton = 0;
		if (Input.GetButton("Skill2"))
			toolbarbutton = 1;
		if (Input.GetButton("Skill3"))
			toolbarbutton = 2;
		if (Input.GetButton("Skill4"))
			toolbarbutton = 3;
		if (Input.GetButton("Skill5"))
			toolbarbutton = 4;
		if (Input.GetButton("Skill6"))
			toolbarbutton = 5;
		
		int skillPressed = -1;
		if (toolbarbutton != -1 || clickedToolbarbutton != -1)
		{
			if (toolbarbutton != -1)
				skillPressed = abilityIDs[toolbarbutton];

			if (clickedToolbarbutton != -1)
				skillPressed = abilityIDs[clickedToolbarbutton];
		}
		
		abilityControler.StartAbility(skillPressed);		
		
		if (effects)
		{
		}
		
		Vector3 updatedMove;
		abilities.ConfirmMovement(moveDirection, out updatedMove);
		moveDirection = updatedMove;		
		
		// now that everything is done, make the character do stuff
		actionDispatcher.UpdateMovement(
			gameObject,
			moveDirection,
			viewChange);
		
		// layer mask needs to be converted to a bitmask
		// This will get the mask to collide against			
		int layerMask = 1 << LayerMask.NameToLayer("otherPlayers");		
		actionDispatcher.SelectionRay(gameObject, layerMask, selectorPos);
		
		if (abilities)
		{
			if (selectorPos.x != -1)
				abilities.UseAbility();
		}		
	}
	
	void OnGUI()
	{   	
	  	Rect finallocation = new Rect();
	  	finallocation.x = toolBarLocation.x * Screen.width;
	  	finallocation.width = toolBarLocation.width * Screen.width;
	  	
	  	finallocation.y = toolBarLocation.y * Screen.height;
	  	finallocation.height = toolBarLocation.height * Screen.height;
	  	
		clickedToolbarbutton = -1;
		clickedToolbarbutton = GUI.Toolbar(finallocation, toolbarbutton, icons);
	}
	
	// Casts a spell for the giving time
	void OnCastSpell(float castTime)
	{
		castingBarObject.Display(castTime);
	}
	
	// channels a spell for the maximum amount of time
	void OnChanneled(float maxTime)
	{
		
	}
	
	// if attack button is held down "charges" the ability up untill the max time.
	void OnCharged(float maxTime)
	{
		
	}
}
