using UnityEngine;
using System.Collections;

public class CastingBarScript : MonoBehaviour {

	private enum CastingPhase { NONE, CASTING, FINISHED_CASTING, FADEOUT, DONE };
	private CastingPhase phase = CastingBarScript.CastingPhase.NONE;
	
	public Texture2D backgroundImage;
	private GameObject backgroundObject;	
	
	public Texture2D updateImage;
	private GameObject updateObject;
	private GUITexture updateTexture;
	
	public Texture2D overlayImage;
	private GameObject overlayObject;	
	
	private float castingTime = 0.0f;
	private float currCastTime = 0.0f;
	
	public float fadeOutTime = 0.5f;
	private float currFadeOut = 0.0f;
	
	private float imageWidth;
	private float imageHeight;
	private float imageX = 0.383f;
	private float imageY = 0.11f;
	private float finalLocX = 0;
	private float finalLocY = 0;
		
	public void Display(float time){
		currCastTime = 0.0f;
		castingTime = time;
		phase = CastingPhase.CASTING;
		
		Color color = backgroundObject.guiTexture.color;
		color.a = 1.0f;
		backgroundObject.guiTexture.color = color;
		
		updateObject.guiTexture.color = Color.red;
		
		color = overlayObject.guiTexture.color;
		color.a = 1.0f;
		overlayObject.guiTexture.color = color;
		overlayObject.guiText.enabled = true;
		
		gameObject.SetActiveRecursively(true);
	}
	
	// Use this for initialization
	void Start () {		
		GUITexture tgt;
		
		imageWidth = backgroundImage.width;
		imageHeight = backgroundImage.height *0.5f;
		
		finalLocX = Screen.width*imageX;
		finalLocY = Screen.height*imageY;
		
		backgroundObject = new GameObject("backgroundCastingBar");
		tgt = (GUITexture)backgroundObject.AddComponent("GUITexture");
		tgt.texture = backgroundImage;
		tgt.transform.localScale = Vector3.zero;
		tgt.transform.position = new Vector3(0,0,-0.2f);
		tgt.pixelInset = new Rect(finalLocX,finalLocY,imageWidth,imageHeight);
		tgt.color = new Color(0,0,0,0);
		
		updateObject = new GameObject("updateCastingBar");
		updateTexture = (GUITexture)updateObject.AddComponent("GUITexture");
		updateTexture.texture = updateImage;
		updateTexture.transform.localScale = Vector3.zero;
		updateTexture.transform.position = new Vector3(0,0,-0.1f);
		updateTexture.pixelInset = new Rect(finalLocX,finalLocY,0,imageHeight);
		updateTexture.color = new Color(1,0,0,0);
		
		overlayObject = new GameObject("castingoverlay");
		tgt = (GUITexture)overlayObject.AddComponent("GUITexture");
		tgt.texture = overlayImage;
		tgt.transform.localScale = Vector3.zero;
		tgt.transform.position = Vector3.zero;
		tgt.pixelInset = new Rect(finalLocX,finalLocY,imageWidth,imageHeight);
		Color color = tgt.color;
		color.a = 0;
		tgt.color = color;
		
		GUIText gtxt = (GUIText)overlayObject.AddComponent("GUIText");
		gtxt.text = "Casting...";
		gtxt.pixelOffset = new Vector2(finalLocX + 100, finalLocY + 16);
		gtxt.enabled = false;
		
		gameObject.SetActiveRecursively(false);
	}
	
	// Update is called once per frame
	void Update () {
		switch(phase)
		{
		case CastingPhase.NONE:
			break;
		
		case CastingPhase.CASTING:
			currCastTime += Time.deltaTime;
			float widthscalar = currCastTime / castingTime;
			if (widthscalar > 1.0f) {
				widthscalar = 1.0f;
				phase = CastingPhase.FINISHED_CASTING;
			}
			
			updateTexture.pixelInset = new Rect(finalLocX, finalLocY, imageWidth * widthscalar, imageHeight);
			break;
		
		case CastingPhase.FINISHED_CASTING:
			currFadeOut = fadeOutTime;
			phase = CastingBarScript.CastingPhase.FADEOUT;
			updateObject.guiTexture.color = Color.green;
			break;
		
		case CastingPhase.FADEOUT:
			currFadeOut -= Time.deltaTime;			
			float percentage = currFadeOut / fadeOutTime;
			if (percentage < 0.0f) {
				phase = CastingBarScript.CastingPhase.DONE;
				percentage = 0.0f;
			}
			
			Color color = backgroundObject.guiTexture.color;
			color.a = percentage;
			backgroundObject.guiTexture.color = color;
			
			color = updateObject.guiTexture.color;
			color.a = percentage;
			updateObject.guiTexture.color = color;
			
			color = overlayObject.guiTexture.color;
			color.a = percentage;
			overlayObject.guiTexture.color = color;
			overlayObject.guiText.enabled = false;
			break;
		
		case CastingPhase.DONE:
			/*
			Destroy(backgroundObject);
			Destroy(updateObject);
			Destroy(overlayObject);
			Destroy(this);
			Destroy(gameObject);
			*/
			phase = CastingPhase.NONE;
			gameObject.SetActiveRecursively(false);
			break;
		}
	}
	
	void Interrupted()
	{
		phase = CastingPhase.FADEOUT;
	}
}
		
