using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour {
		
	float lifeSpawn = 10.0f;
	float damage = 3.0f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void FixedUpdate()
	{		
		transform.Translate(transform.forward * 20.0f * Time.fixedDeltaTime, Space.World);
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
}
