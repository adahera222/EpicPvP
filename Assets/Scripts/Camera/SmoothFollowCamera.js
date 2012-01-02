/// This camera is similar to the one used in Super Mario 64

/*

"Fire2" snaps the camera

*/

// The target we are following
var target : Transform;

// The distance in the x-z plane to the target

var distance = 7.0;

// the height we want the camera to be above the target
var height = 3.0;

var angularSmoothLag = 0.3;
var angularMaxSpeed = 10000.0;

var heightSmoothLag = 0.3;

var clampHeadPositionScreenSpace = 0.75;

var lockCameraTimeout = 0.2;

private var headOffset = Vector3.zero;
private var centerOffset = Vector3.zero;

private var heightVelocity = 0.0;
private var angleVelocity = 0.0;

//private var controller : ThirdPersonController;
private var targetHeight = 100000.0;

function Awake ()
{
	if (target)
	{
		//controller = target.GetComponent(ThirdPersonController);
	}
	
	/*
	if (controller)
	{
		var characterController : CharacterController = target.collider;
		centerOffset = characterController.bounds.center - target.position;
		headOffset = centerOffset;
		headOffset.y = characterController.bounds.max.y - target.position.y;
	}
	else
		Debug.Log("Please assign a target to the camera that has a ThirdPersonController script attached.");
	*/
	
	Cut(target, centerOffset);
}

function DebugDrawStuff ()
{
	Debug.DrawLine(target.position, target.position + headOffset);

}

function AngleDistance (a : float, b : float)
{
	a = Mathf.Repeat(a, 360);
	b = Mathf.Repeat(b, 360);
	
	return Mathf.Abs(b - a);
}

function Apply (dummyTarget : Transform, dummyCenter : Vector3)
{
	// Early out if we don't have a target
	//if (!controller)
		//return;
	
	var targetCenter = target.position + centerOffset;
	var targetHead = target.position + headOffset;

//	DebugDrawStuff();

	// Calculate the current & target rotation angles
	var originalTargetAngle = target.eulerAngles.y;
	var currentAngle = transform.eulerAngles.y;

	// Adjust real target angle when camera is locked
	var targetAngle = originalTargetAngle; 
	
	// Lock the camera when moving backwards!
	// * It is really confusing to do 180 degree spins when turning around.
	if (AngleDistance (currentAngle, targetAngle) > 160 && false /*controller.IsMovingBackwards ()*/)
		targetAngle += 180;

	currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, angleVelocity, angularSmoothLag, angularMaxSpeed);


	// When jumping don't move camera upwards but only down!
	/*
	if (controller.IsJumping ())
	{
		// We'd be moving the camera upwards, do that only if it's really high
		var newTargetHeight = targetCenter.y + height;
		if (newTargetHeight < targetHeight || newTargetHeight - targetHeight > 5)
			targetHeight = targetCenter.y + height;
	}
	// When walking always update the target height
	else
	*/
	//{
		targetHeight = targetCenter.y + height;
	//}

	// Damp the height
	currentHeight = transform.position.y;
	currentHeight = Mathf.SmoothDamp (currentHeight, targetHeight, heightVelocity, heightSmoothLag);

	// Convert the angle into a rotation, by which we then reposition the camera
	currentRotation = Quaternion.Euler (0, currentAngle, 0);
	
	// Set the position of the camera on the x-z plane to:
	// distance meters behind the target
	transform.position = targetCenter;
	transform.position += currentRotation * Vector3.back * distance;

	// Set the height of the camera
	transform.position.y = currentHeight;
	
	// Always look at the target	
	SetUpRotation(targetCenter, targetHead);
}

function LateUpdate () {
	Apply (transform, Vector3.zero);
}

function Cut (dummyTarget : Transform, dummyCenter : Vector3)
{
	var oldHeightSmooth = heightSmoothLag;
	
	heightSmoothLag = 0.001;
	
	Apply (transform, Vector3.zero);	
	heightSmoothLag = oldHeightSmooth;
}

function SetUpRotation (centerPos : Vector3, headPos : Vector3)
{
	// Now it's getting hairy. The devil is in the details here, the big issue is jumping of course.
	// * When jumping up and down we don't want to center the guy in screen space.
	//  This is important to give a feel for how high you jump and avoiding large camera movements.
	//   
	// * At the same time we dont want him to ever go out of screen and we want all rotations to be totally smooth.
	//
	// So here is what we will do:
	//
	// 1. We first find the rotation around the y axis. Thus he is always centered on the y-axis
	// 2. When grounded we make him be centered
	// 3. When jumping we keep the camera rotation but rotate the camera to get him back into view if his head is above some threshold
	// 4. When landing we smoothly interpolate towards centering him on screen
	var cameraPos = transform.position;
	var offsetToCenter = centerPos - cameraPos;
	
	// Generate base rotation only around y-axis
	var yRotation = Quaternion.LookRotation(Vector3(offsetToCenter.x, 0, offsetToCenter.z));

	var relativeOffset = Vector3.forward * distance + Vector3.down * height;
	transform.rotation = yRotation * Quaternion.LookRotation(relativeOffset);

	// Calculate the projected center position and top position in world space
	var centerRay = camera.ViewportPointToRay(Vector3(.5, 0.5, 1));
	var topRay = camera.ViewportPointToRay(Vector3(.5, clampHeadPositionScreenSpace, 1));

	var centerRayPos = centerRay.GetPoint(distance);
	var topRayPos = topRay.GetPoint(distance);
	
	var centerToTopAngle = Vector3.Angle(centerRay.direction, topRay.direction);
	
	var heightToAngle = centerToTopAngle / (centerRayPos.y - topRayPos.y);

	var extraLookAngle = heightToAngle * (centerRayPos.y - centerPos.y);
	if (extraLookAngle < centerToTopAngle)
	{
		extraLookAngle = 0;
	}
	else
	{
		extraLookAngle = extraLookAngle - centerToTopAngle;
		transform.rotation *= Quaternion.Euler(-extraLookAngle, 0, 0);
	}
}

function GetCenterOffset ()
{
	return centerOffset;
}

@script AddComponentMenu ("Third Person Camera/Smooth Follow Camera")
@script RequireComponent (Camera)