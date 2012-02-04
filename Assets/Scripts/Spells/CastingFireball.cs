using UnityEngine;
using System.Collections;

public class CastingFireball : MonoBehaviour {
	
	public float rotationRate = 0.14f;
	
	// Use this for initialization
	void Start () {
		
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up, rotationRate * Time.deltaTime);
	}
}
