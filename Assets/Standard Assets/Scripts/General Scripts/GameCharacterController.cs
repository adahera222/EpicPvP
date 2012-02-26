using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class GameCharacterController : MonoBehaviour {	
	
	public float headRotateSpeed = 10.0f;
	public float lookatspeed = 10.0f;
	public float movementSpeed = 10.0f;
	
	float spinBodyBy = 0;
	
	public Transform cameraTarget;
	
	CharacterController controller;
	
	Vector3 currViewChange;
	Vector3 movementUpdate;
	
	public string characterName;
	
	GameObject selectedObject;
	Transform selectedPosition;
	Vector3 selectedPoint;
	
	// First function that is called when a scene is loaded
	void Awake()
	{
		currViewChange = Vector3.zero;
	}
	
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		currViewChange = transform.forward;	
		spinBodyBy = 0;
	
		TextMesh mesh = transform.Find("DisplayStats/ObjectsName").GetComponent<TextMesh>();
		mesh.text = characterName;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Do the updates here so that way animations can be tempered with.
	void LateUpdate()
	{
		// twist the head around
		float spinHead = currViewChange.x * headRotateSpeed * Time.deltaTime;
		Transform head = transform.Find("rootJoint/torso/shoulders/head");
		head.Rotate(Vector3.up, spinHead*0.5f, Space.World);
		Transform torso = transform.Find("rootJoint/torso");
		torso.Rotate(Vector3.up, spinHead*0.5f, Space.World);
		
		spinBodyBy += spinHead;
		
		// move to face the direction
		if (movementUpdate.x != 0.0f || movementUpdate.z != 0.0f)
		{
			transform.Rotate(Vector3.up, spinBodyBy, Space.World);
			head.Rotate(Vector3.up, -spinBodyBy*0.5f, Space.World);
			torso.Rotate(Vector3.up, -spinBodyBy*0.5f, Space.World);
			cameraTarget.Rotate(Vector3.up, -spinBodyBy, Space.World);
			spinBodyBy = 0;
		}
		
		float updown = currViewChange.y * lookatspeed * Time.deltaTime;
		cameraTarget.Rotate(Vector3.right, -updown);
		cameraTarget.Rotate(Vector3.up, spinHead, Space.World);
	}
	
	void FixedUpdate()
	{	
		// project movement onto a xz plane to detect if we are running or not.
		Vector3 cross = Vector3.Cross(movementUpdate, Vector3.up);
		Vector3 projected = Vector3.Cross(Vector3.up, cross);
		if (projected.magnitude != 0.0f)
			animation.Play("run");
		else
			animation.Play("idle");
				
		controller.Move(movementUpdate);
	}
	
	public void UpdateMovement(Vector3 movementDirection, Vector3 viewChange)
	{
		currViewChange = viewChange;
		movementDirection *= movementSpeed;
		movementDirection.y -= 20.0f; // gravity
		movementDirection *= Time.deltaTime;		
		movementUpdate = movementDirection;
	}
	
	public bool IsJumping()
	{
		return false;
	}
	
	public void SelectionRay(int layerMask, Vector3 mousePos)
	{
		selectedObject = null;
		Ray ray = Camera.mainCamera.ScreenPointToRay(mousePos);
		RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000, layerMask ))
        {
			if ( selectedObject )
				selectedObject.GetComponent<RemotePlayer>().ObjectDeselected();
			
			selectedObject = hit.transform.gameObject;
			selectedObject.GetComponent<RemotePlayer>().ObjectSelected();
			
			selectedPosition = hit.transform;
		}
		
        if (Physics.Raycast(ray, out hit, 10000))
        {			
			selectedPoint = hit.point;
        }		
	}
	
	public GameObject GetSelectedObject()
	{
		return selectedObject;
	}
	
	public Transform GetSelectedTransform()
	{
		return selectedPosition;
	}
	
	public Vector3 GetSelectedPoint	()
	{
		return selectedPoint;
	}	
	
	public float GetSpeed()
	{
		return movementUpdate.magnitude;
	}
	
}
