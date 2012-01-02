using UnityEngine;
using System.Collections;

public class InputGrabber : MonoBehaviour {
	ActionDispatcher actionDispatcher;
	
	Vector3 lastScreenPos;
	
	// Use this for initialization
	void Start () {
		actionDispatcher = GameObject.Find("Level").GetComponent<ActionDispatcher>();
	}
	
	// Update is called once per frame
	void Update () {
		// No speed modifications or anything of the like are done here because that will be handled by the server.
		// values will be either -1, 0, or 1 in keyboard mode
		Vector3 moveDirection = Vector3.zero;
  		moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
		
		// Using get axis raw here too becuase "GetButton" only returns true on the frist
		//	cycle of when it was actually pressed and won't return true untill it is pressed again.
		Vector3 viewChange = Vector3.zero;
		if (Input.GetAxisRaw("LookAtPoint") != 0)
		{
			viewChange = Input.mousePosition - lastScreenPos;
		}
		lastScreenPos = Input.mousePosition;
		
		actionDispatcher.UpdateMovement(
			gameObject,
			moveDirection,
			viewChange);
		
		// this is a left click of the mouse and is used for selecting/targeting
		if (Input.GetButton("Fire1"))
		{
			// layer mask needs to be converted to a bitmask
			// This will get the mask to collide against			
			int layerMask = 1 << LayerMask.NameToLayer("otherPlayers");
			Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
			actionDispatcher.SelectionRay(gameObject, layerMask, ray);
		}
	}
}
