using UnityEngine;
using System.Collections;

public class AbilityController : MonoBehaviour {
	
	AbilitesCollection collection;
	//HUDScript hud;
	
	// Use this for initialization
	void Start () {
		collection = GetComponent<AbilitesCollection>();		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void StartAbility(int id)
	{
		collection.StartAbility(id);
	}
}
