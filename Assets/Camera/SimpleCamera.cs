using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/SimpleCamera")]
[RequireComponent (typeof (Camera))]

public class SimpleCamera : MonoBehaviour {
	
	public Transform target;
	public float distance = 10.0f;
	public float distanceScalar = 10.0f;
	
	public float minDistance = 4.0f;
	public float maxDistance = 10.0f;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.right = target.right;
		transform.up = target.up;
		transform.forward = target.forward;
		
		transform.position = target.position - (target.forward * distance);
	}
	
	void AdjustDistance(float modifier)
	{
		distance += -modifier*distanceScalar;
		if (distance < minDistance) distance = minDistance;
		if (distance > maxDistance) distance = maxDistance;
	}
}
