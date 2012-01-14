
// Make the script also execute in edit mode
@script ExecuteInEditMode()


var gSkin : GUISkin;
var backdrop : Texture2D; // our backdrop image goes in here.

var icons : Texture2D[];
var toolBarLocation : Rect;
//var minimapLocation : 
private var buttonWidth : float;

var timerWait : float = 0.5f; // how long to hold the mouse button down to move an ability
private var timerHeldDown : float;
private var heldTexture : Texture2D;

var heldTextureCenter : Vector2;

var hostPlayer : GameObject;

function Start()
{
	//GetAbilities();
}

function OnGUI()
{
	if (gSkin)
		GUI.skin = gSkin;
	else
		Debug.Log("HUD is missing its GUI Skin object!");	
		
 	//GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, Vector3 
 	//	(Screen.width / 2040.0, Screen.height / 1152.0, 1)); 		
		
	// Draw the main gui skin to the screen
	//var backgroundStyle : GUIStyle = new GUIStyle();
	//backgroundStyle.normal.background = backdrop;
 	//GUI.Label( Rect( 0, Screen.height - 250, Screen.width, 250), "", backgroundStyle);
 	
 	// Check for web play (we would never want to attempt to close a browser
 	/*
 	var isWebPlayer = (Application.platform == RuntimePlatform.OSXWebPlayer || 
   Application.platform == RuntimePlatform.WindowsWebPlayer);
 if (!isWebPlayer)
  {
  if (GUI.Button( Rect( (Screen.width/2)-70, Screen.height - 80, 140, 70), "Quit"))
   Application.Quit();
  }
  */
   	
  	var finallocation : Rect;
  	finallocation.x = toolBarLocation.x * Screen.width;
  	finallocation.width = toolBarLocation.width * Screen.width;
  	
  	finallocation.y = toolBarLocation.y * Screen.height;
  	finallocation.height = toolBarLocation.height * Screen.height;
  	buttonWidth = finallocation.width / icons.length;
  	
	GUI.Toolbar(finallocation, -1, icons);
	
	// show the icon moving the new spot
	if (heldTexture)
	{
		heldTextureCenter.x = Input.mousePosition.x;
		heldTextureCenter.y = Screen.height - Input.mousePosition.y;
		
		var textureRect : Rect = Rect(
			heldTextureCenter.x - heldTexture.width*0.5f,
			heldTextureCenter.y - heldTexture.height*0.5f,
			heldTexture.width,
			heldTexture.height );
			
  		GUI.Box(textureRect, heldTexture);
	}	
}

function Update () {
	
	var buttonId : int;
	var selectedVal = Input.GetAxisRaw("Cast");
	if (selectedVal != 0)
	{				
		var finallocation : Rect;
  		finallocation.x = toolBarLocation.x * Screen.width;
  		finallocation.width = toolBarLocation.width * Screen.width;
  		finallocation.y = toolBarLocation.y * Screen.height;
  		finallocation.height = toolBarLocation.height * Screen.height; 
  		
  		var mousepos : Vector2;
  		mousepos.x = Input.mousePosition.x;
  		mousepos.y = Screen.height - Input.mousePosition.y;
		if (finallocation.Contains(mousepos))
		{
			timerHeldDown += Time.deltaTime;
			
			var xval : float = mousepos.x - finallocation.x;
			buttonId = parseInt(xval / buttonWidth);
		}
	}
	
	if (selectedVal == 0 && heldTexture)
	{
		timerHeldDown = 0;
  		finallocation.x = toolBarLocation.x * Screen.width;
  		finallocation.width = toolBarLocation.width * Screen.width;
  		finallocation.y = toolBarLocation.y * Screen.height;
  		finallocation.height = toolBarLocation.height * Screen.height; 
  		
  		mousepos.x = Input.mousePosition.x;
  		mousepos.y = Screen.height - Input.mousePosition.y;
		if (finallocation.Contains(mousepos))
		{
			xval = mousepos.x - finallocation.x;
			buttonId = parseInt(xval / buttonWidth);
			icons[buttonId] = heldTexture;
		}
		heldTexture = null;
	}		
	
	if (timerHeldDown > timerWait && !heldTexture)
	{
		timerHeldDown = 0;
		heldTexture = icons[buttonId];
		var clearTexture : Texture2D;
		icons[buttonId] = clearTexture;
	}
}

function GetAbilities()
{	
	var collection : AbilitesCollection = hostPlayer.GetComponent("AbilitesCollection");
	collection.FillAbilityBar(icons);
}

