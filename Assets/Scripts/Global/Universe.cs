using UnityEngine;
using System.Collections;

public static class Universe  {
	
	/** 
	 * get all Players (objects with tag player), as an array, of this scene. 
	 * */
	public static GameObject[] getPlayers() {
		return (GameObject.FindGameObjectsWithTag(Phobos.Vars.PLAYER_TAG));	
	}
	
	/** 
	 * get all Players (objects with tag player), as an array, of this scene. 
	 * */
	public static GameObject getPlayer() {
		return (GameObject.FindGameObjectWithTag(Phobos.Vars.PLAYER_TAG));	
	}
	
	/**
	 * Clears all building preview's, 
	 * to use when switching to another control type than "building"
	 * */
	public static void clearAllBuildingPreview() {
		var allBuildingsPreview = GameObject.FindGameObjectsWithTag(Phobos.Vars.BUILDING_PREVIEW);
		foreach (GameObject build in allBuildingsPreview) {
			GameObject.Destroy(build); 	
		}
	}
	
	public static GameObject getMainCamera() {
		var mainCamera = GameObject.FindGameObjectWithTag(Phobos.Vars.MAIN_CAMERA_TAG);
		return mainCamera;
	}
	
	public static GameObject getCameraContainer() {
		return GameObject.FindGameObjectWithTag(Phobos.Vars.CAMERA_CONTAINER);
	}
	
	public static void switchCameraFollow(bool newFollow) {	
		GameObject mainCamera = Universe.getCameraContainer();
		if (mainCamera) {
			UniverseCamera univCam = (UniverseCamera) mainCamera.GetComponent(typeof(UniverseCamera));
			univCam.followingObject = newFollow ; 
		}
	}
}
