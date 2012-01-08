using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class GameCharacterController : MonoBehaviour {	
	
	public float headRotateSpeed = 10.0f;
	public float movementSpeed = 10.0f;
	float spinBodyBy = 0;
	public Transform cameraTarget;
	float camHeight = 3;
	CharacterController controller;
	
	Vector3 currViewChange;
	Vector3 movementUpdate;
	
	public string characterName;
	
	GameObject selectedObject;
	
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
	
	void LateUpdate()
	{
		// twist the head around
		float spinHead = currViewChange.x * headRotateSpeed * Time.deltaTime;
		Transform head = transform.Find("rootJoint/torso/shoulders/head");
		head.Rotate(Vector3.up, spinHead*0.5f, Space.World);
		Transform torso = transform.Find("rootJoint/torso");
		torso.Rotate(Vector3.up, spinHead*0.5f, Space.World);
		
		spinBodyBy += spinHead;
		
		controller.Move(movementUpdate);
		cameraTarget.Rotate(Vector3.up, spinHead, Space.World);
		
		
		// move to face the direction
		if (movementUpdate.x != 0.0f || movementUpdate.z != 0.0f)
		{
			transform.Rotate(Vector3.up, spinBodyBy, Space.World);
			head.Rotate(Vector3.up, -spinBodyBy*0.5f, Space.World);
			torso.Rotate(Vector3.up, -spinBodyBy*0.5f, Space.World);
			cameraTarget.Rotate(Vector3.up, -spinBodyBy, Space.World);
			spinBodyBy = 0;
		}
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
	
	public void SelectionRay(int layerMask, Ray ray)
	{
		RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000, layerMask ))
        {
			if ( selectedObject )
				selectedObject.GetComponent<RemotePlayer>().ObjectDeselected();
			
			selectedObject = hit.transform.gameObject;
			selectedObject.GetComponent<RemotePlayer>().ObjectSelected();
        }
	}
	
	public float GetCameraHeight()
	{
		return camHeight;
	}
}
