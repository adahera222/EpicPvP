using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour {
	
	public float speed = 20.0f;
	public float lifeSpawn = 10.0f;
	
	Vector3 moveDirection;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void FixedUpdate()
	{
		transform.Translate(moveDirection * speed * Time.fixedDeltaTime, Space.World);
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
	
	public virtual void SetTarget(Vector3 point)
	{
		moveDirection = point - transform.position;
		moveDirection.Normalize();		
	}	
}
