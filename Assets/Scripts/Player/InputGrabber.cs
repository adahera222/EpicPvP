using UnityEngine;
using System.Collections;

public class InputGrabber : MonoBehaviour {
	ActionDispatcher actionDispatcher;
	
	EffectsCollection effects;
	AbiliitesCollection abilities;
	
	Vector3 lastScreenPos;
	
	// Use this for initialization
	void Start () {
		actionDispatcher = GameObject.Find("Level").GetComponent<ActionDispatcher>();
		
		effects = GetComponent<EffectsCollection>();
		abilities = GetComponent<AbiliitesCollection>();
	}
	
	// Update is called once per frame
	void Update () {
		// No speed modifications or anything of the like are done here because that will be handled by the server.
		// values will be either -1, 0, or 1 in keyboard mode
		Vector3 moveDirection = Vector3.zero;
  		moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
		
		Vector3 selectorPos = new Vector3(-1,-1,-1);
		
		// Using get axis raw here too becuase "GetButton" only returns true on the frist
		//	cycle of when it was actually pressed and won't return true untill it is pressed again.
		Vector3 viewChange = Vector3.zero;
		if (Input.GetAxisRaw("LookAtPoint") != 0)
			viewChange = Input.mousePosition - lastScreenPos;
		lastScreenPos = Input.mousePosition;
		
		// this is a left click of the mouse and is used for selecting/targeting
		if (Input.GetButton("Cast"))
		{
			selectorPos = Input.mousePosition;
		}
		
		int skillPressed = -1;
		if (Input.GetButton("Skill1"))
			skillPressed = 0;
		if (Input.GetButton("Skill2"))
			skillPressed = 1;
		if (Input.GetButton("Skill3"))
			skillPressed = 2;
		if (Input.GetButton("Skill4"))
			skillPressed = 3;
		if (Input.GetButton("Skill5"))
			skillPressed = 4;
		if (Input.GetButton("Skill6"))
			skillPressed = 5;
		
		if (abilities)
		{
			if (skillPressed != -1)
				abilities.StartAbility(skillPressed);
			if (selectorPos.x != -1)
				abilities.UseAbility();
		}
		
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
	}
}
