using UnityEngine;
using System.Collections;

[AddComponentMenu("Character/Animation Controller")]
[RequireComponent (typeof (GameCharacterController))]
public class AnimationController : MonoBehaviour {

	float runSpeedScale = 1.0f;
	float walkSpeedScale = 1.0f;
	
	// Use this for initialization
	void Start () {
		// By default loop all animations
		animation.wrapMode = WrapMode.Loop;
	
		animation["run"].layer = -1;
		animation["walk"].layer = -1;
		animation["idle"].layer = -2;
		animation.SyncLayer(-1);
	
		animation["ledgefall"].layer = 9;	
		animation["ledgefall"].wrapMode = WrapMode.Loop;
	
	
		// The jump animation is clamped and overrides all others
		animation["jump"].layer = 10;
		animation["jump"].wrapMode = WrapMode.ClampForever;
	
		animation["jumpfall"].layer = 10;	
		animation["jumpfall"].wrapMode = WrapMode.ClampForever;
	
		// This is the jet-pack controlled descent animation.
		animation["jetpackjump"].layer = 10;	
		animation["jetpackjump"].wrapMode = WrapMode.ClampForever;
	
		animation["jumpland"].layer = 10;	
		animation["jumpland"].wrapMode = WrapMode.Once;
	
		animation["walljump"].layer = 11;	
		animation["walljump"].wrapMode = WrapMode.Once;
	
		// we actually use this as a "got hit" animation
		animation["buttstomp"].speed = 0.15f;
		animation["buttstomp"].layer = 20;
		animation["buttstomp"].wrapMode = WrapMode.Once;	
		var punch = animation["punch"];
		punch.wrapMode = WrapMode.Once;
	
		// We are in full control here - don't let any other animations play when we start
		animation.Stop();
		animation.Play("idle");	
	}
	
	// Update is called once per frame
	void Update () {
		GameCharacterController playerController = GetComponent<GameCharacterController>();
		float currentSpeed = playerController.GetSpeed();

		if (currentSpeed > 0.1)
		{
			animation.CrossFade("run");
			// We fade out jumpland realy quick otherwise we get sliding feet
			animation.Blend("jumpland", 0);
		}
		// Fade out walk and run
		else
		{
			animation.Blend("walk", 0.0f, 0.3f);
			animation.Blend("run", 0.0f, 0.3f);
			animation.Blend("run", 0.0f, 0.3f);
		}
		
		animation["run"].normalizedSpeed = runSpeedScale;
		animation["walk"].normalizedSpeed = walkSpeedScale;
		
		/*
		if (playerController.IsJumping ())
		{
			if (playerController.IsControlledDescent())
			{
				animation.CrossFade ("jetpackjump", 0.2f);
			}
			else if (playerController.HasJumpReachedApex ())
			{
				animation.CrossFade ("jumpfall", 0.2f);
			}
			else
			{
				animation.CrossFade ("jump", 0.2f);
			}
		}
		// We fell down somewhere
		else if (!playerController.IsGroundedWithTimeout())
		{
			animation.CrossFade ("ledgefall", 0.2f);
		}
		// We are not falling down anymore
		else
		{
			animation.Blend ("ledgefall", 0.0, 0.2f);
		}
		*/	
	}
}
