using UnityEngine;
using System.Collections;

public class ActionDispatcher : MonoBehaviour {
	
	//GameObject playersList;	
	// Use this for initialization
	void Start () {
		//playersList = GameObject.Find("Players");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void UpdateMovement(GameObject character, Vector3 movementDirection, Vector3 viewChange)
	{
		// Update the characters movement that was passed using their own stats
		// if this were networked this is where the info would also go to the server
		// but for now just update it and pass it to the game controller
		GameCharacterController controller = character.GetComponent<GameCharacterController>();
		controller.UpdateMovement(movementDirection, viewChange);
	}
	
	public void SelectionRay(GameObject character, int layerMask, Vector3 mousePos)
	{
		GameCharacterController controller = character.GetComponent<GameCharacterController>();
		controller.SelectionRay(layerMask, mousePos);
	}
}
