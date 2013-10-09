using UnityEngine;
using System.Collections;
 
public class LabelPositioning : MonoBehaviour {
 
public Transform target;  // Object that this label should follow
public Vector3 offset = Vector3.up;    // Units in world space to offset; 1 unit above object by default
public bool clampToScreen = false;  // If true, label will be visible even if object is off screen
public float clampBorderSize = 0.05f;  // How much viewport space to leave at the borders when a label is being clamped
public bool useMainCamera = true;   // Use the camera tagged MainCamera
public Camera cameraToUse ;   // Only use this if useMainCamera is false
Camera cam ;
public bool locked = false; 
Transform thisTransform;
Transform camTransform;
 
	void Start () 
    {
	    thisTransform = transform;
    if (useMainCamera)
        cam = Camera.main;
    else
        cam = cameraToUse;
		
    camTransform = cam.transform;
		
		//Get the name of the ship 
		if (this.target != null) {
			if (guiText) {
				var ship = this.transform.parent; 
				PhotonView PV = (PhotonView) ship.GetComponent(typeof(PhotonView)); 
				var shipName = " " + PV.viewID; 
				guiText.text += shipName; 		
			}
		}
	}

 
    void Update()
    {
 		if (this.target != null) {
	        if (clampToScreen)
	        {
	            Vector3 relativePosition = camTransform.InverseTransformPoint(target.position);
	            relativePosition.z =  Mathf.Max(relativePosition.z, 1.0f);
	            thisTransform.position = cam.WorldToViewportPoint(camTransform.TransformPoint(relativePosition + offset));
	            thisTransform.position = new Vector3(Mathf.Clamp(thisTransform.position.x, clampBorderSize, 1.0f - clampBorderSize),
	                                             Mathf.Clamp(thisTransform.position.y, clampBorderSize, 1.0f - clampBorderSize),
	                                             thisTransform.position.z);
	 
	        }
	        else
	        {
	            thisTransform.position = cam.WorldToViewportPoint(target.position + offset);
	        }
		}
		else {
			this.disableTexture(); 
		}
    }
	
	public void setTooltipToShow() {
		this.switchTooltipShow(true); 
	}
	
	public void setTooltipToHide() {
		this.switchTooltipShow(false); 	
	}
	
	public void lockTooltip() {
		this.locked = true ; 
	}
	
	public void unlockTooltip() {
		this.locked = false; 	
	}
	
	public void enableTexture() {
		if (guiTexture) 
			guiTexture.enabled = true;  
		if (guiText) 
			guiText.enabled = true ; 
	}
	
	public void disableTexture() {
		if (guiTexture) 
			guiTexture.enabled = false;  
		if (guiText) 
			guiText.enabled = false ; 
	}
	
	private void switchTooltipShow(bool show) {
		GUIText TooltipText = (GUIText) this.GetComponent(typeof(GUIText));	
		GUITexture TooltipTexture = (GUITexture) this.GetComponent(typeof(GUITexture));	
		if (TooltipText != null) 
			TooltipText.enabled = show; 
		if (TooltipTexture != null) 
			TooltipTexture.enabled = show;  	
	}
}