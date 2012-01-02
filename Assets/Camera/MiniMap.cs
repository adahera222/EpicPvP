using UnityEngine;
using System.Collections;

[ExecuteInEditMode()] // This allows the code to run in edit mode
public class MiniMap : MonoBehaviour {
	
	public Transform target;
	public Vector3 positionOffset;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = target.position + positionOffset;
	}
}
