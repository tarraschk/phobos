using UnityEngine;
using System.Collections;

/**
 * Allows an object to be dockable. 
 * This means that objects that have propulsors (to change ?) 
 * Can go inside this object, and then be attached to it.
 *    OR
 * If the dockable is a 
 */

public class Dockable : MonoBehaviour {
	
	public Phobos.dockType type ; 
	public string warpDestination = null ; 
	
	void Start () {
		if (warpDestination != null) 
			warpInit() ; //This dockable is a warp hole / gate
		else 
			stationInit() ; //This dockable is a station
	}
	
	void Update () {
	
	}
	
	private void warpInit() {
		this.type = Phobos.dockType.warp ; 
	}
	
	private void stationInit() {
		this.type = Phobos.dockType.station ; 
	}
}