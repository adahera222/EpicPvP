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
	
	public bool StartAbility(int id)
	{
		return collection.StartAbility(id);
	}
}
