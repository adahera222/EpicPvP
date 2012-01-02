using UnityEngine;
using System.Collections;

[ExecuteInEditMode()] // This allows the code to run in edit mode
public class FollowMouse : MonoBehaviour {
	
	public Transform attachedTo;
	public Vector3 positionOffset;
	public Vector3 lookAtOffset;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		float moveScaler = 0.0f;
		float val = Input.GetAxis("Vertical");
		if 		(val == 1)  moveScaler = val;
		else if (val == -1) moveScaler = val;
		if (moveScaler != 0)
		{			
	        Ray ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);			
			RaycastHit hit;
	        if (Physics.Raycast(ray, out hit, 10000))
	        {
				attachedTo.forward = hit.point - attachedTo.position;
				attachedTo.forward.Normalize();				
	        }
		}
		
		float velocity = 10.0f;
		attachedTo.position += attachedTo.forward * velocity * moveScaler * Time.deltaTime;
		
		transform.position = attachedTo.position + positionOffset;		
		transform.LookAt(attachedTo.position + lookAtOffset);
	}
}
