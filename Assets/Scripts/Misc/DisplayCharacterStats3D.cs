using UnityEngine;
using System.Collections;

public class DisplayCharacterStats3D : MonoBehaviour {
	
	public Transform worldCamera;
	public Transform statusPlate;
	public Vector3 statusPlateOffset;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		/*
		Vector3 position = Camera.mainCamera.WorldToViewportPoint(worldCamera.position);
		position.x -= statusPlateOffset.x;
		position.y -= statusPlateOffset.y;
		position.z -= statusPlateOffset.z;
		statusPlate.transform.position = position;
		*/
		
		transform.right 	= worldCamera.right;
		transform.up 		= worldCamera.up;
		transform.forward 	= worldCamera.forward;		
	}
}
