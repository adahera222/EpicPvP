using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour {
		
	float lifeSpawn = 10.0f;
	Vector3 target;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void FixedUpdate()
	{
		Vector3 moveDirection = target - transform.position;
		moveDirection.Normalize();
		transform.Translate(moveDirection * 20.0f * Time.fixedDeltaTime, Space.World);
		lifeSpawn -= Time.fixedDeltaTime;
		if (lifeSpawn <= 0.0f)
		{
			lifeSpawn = 0.0f;
			Kill();
		}
	}
	
	public void Initialize()
	{
		
	}
	
	void OnCollisionEnter(Collision collision)
	{
		//collision.gameObject.GetComponent<>();
		
		Kill();
	}
	
	void Kill()
	{
		// stop emitting particles nicely
		ParticleEmitter emitter = GetComponent<ParticleEmitter>();
		if (emitter)
			emitter.emit = false;
		
		transform.DetachChildren();
		
		// kill this object
		Destroy(gameObject);
	}
	
	public virtual void SetTarget(GameObject proj_target)
	{
		if (proj_target.transform.position != Vector3.zero)
		{
			target = proj_target.transform.position;
			target.y += 0.7f;
		}
		else
		{
			
		}
	}	
}
