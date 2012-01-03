using UnityEngine;
using System.Collections;

public class RemotePlayer : MonoBehaviour {
	
	public Transform hostCharacter;
	public float showOnMiniMapDistance = 10.0f;
	
	public GameObject minimapIndicator;
	
	private GameObject selectedProjector;
	
	
	// Use this for initialization
	void Start () {
		selectedProjector = transform.Find("SelectedProjector").gameObject;
		selectedProjector.active = false;
		
		minimapIndicator = transform.Find("mapIndicator").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 dist = transform.position - hostCharacter.position;
		minimapIndicator.active = dist.magnitude < showOnMiniMapDistance;
	}
	
	public void ObjectSelected()
	{
		selectedProjector.active = true;
	}
	
	public void ObjectDeselected()
	{
		selectedProjector.active = false;
	}
}
