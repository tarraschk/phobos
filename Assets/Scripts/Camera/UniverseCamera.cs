using UnityEngine;
using System.Collections;

/**
 * Control the camera within the game world
 * Camera will be moved with the keyboard
* 
* */

public class UniverseCamera : MonoBehaviour {

	//Box limits
	public struct UniverseLimit {
		public int leftLimit;
		public int rightLimit;
		public int bottomLimit;
		public int topLimit;
	}
	
	public static UniverseLimit cameraLimits = new UniverseLimit();
	public static UniverseLimit mouseScrollLimits = new UniverseLimit();
	public static UniverseCamera Instance;
	
	private float cameraMoveSpeed = 300f;
	private float shiftBonus = 45f;
	private float mouseBoundary = 25f;
	
	// Update is called once per frame
	void Update () {
		if (isCameraInput()) {
			Vector3 cameraDesiredMove = getDesiredTranslation();
			if (!isDesiredPositionOverBoundaries(cameraDesiredMove))
			{
				this.transform.Translate (cameraDesiredMove);	
			}
		}
	}
	
	public bool isDesiredPositionOverBoundaries(Vector3 desiredMove)
	{
		return false;	
	}
	
	//Checks if camera input has been used
	public bool isCameraInput() {
		bool keyboardMove;
		bool mouseMove;
		bool canMove;
		
		if (UniverseCamera.areCameraButtonsPressed())
			keyboardMove = true;	
		else keyboardMove = false;
		
		return keyboardMove;
	}
	
	public Vector3 getDesiredTranslation() {
		float moveSpeed = 0f;
		float desiredX = 0f;
		float desiredZ = 0f;
		
		moveSpeed = cameraMoveSpeed * Time.deltaTime;
		
		if (Input.GetKey(KeyCode.UpArrow))
			desiredZ = moveSpeed;
		if (Input.GetKey (KeyCode.DownArrow)) 
			desiredZ = moveSpeed * -1;
		if (Input.GetKey (KeyCode.LeftArrow))
			desiredX = moveSpeed * - 1;
		if (Input.GetKey (KeyCode.RightArrow)) 
			desiredX = moveSpeed;
		
		return new Vector3(desiredX, 0, desiredZ);
	}
		
	
	public static bool areCameraButtonsPressed() {
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow)) 
			return true; 
		else return false;
	}
}
