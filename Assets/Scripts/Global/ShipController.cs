using UnityEngine;
using System.Collections;

//Controls a ship object, which means
//- Propulsors
//- Turrets
//- Cargohold (?) 

public class ShipController : MonoBehaviour {
	
	public Transform target ; 
	public BehaviorTypes behavior = BehaviorTypes.idle; 
	void Update () {
		switch (this.behavior) {
			case BehaviorTypes.idle: 
			
			break;
			case BehaviorTypes.collecting: 
				this.collectBehavior();
			break; 
			case BehaviorTypes.docking: 
				this.dockBehavior(); 
			break ; 
			case BehaviorTypes.moving:
			
			break;
			
			case BehaviorTypes.attacking:
				this.attackBehavior(); 
			break; 
		}
	}
	
	
	public void dockOwn(Transform target) {
		this.addNetInput(Phobos.Commands.DOCK, target); 
		this.dock (target);	
	}
	
	
	public void dock(Transform target) {
		Propulsors prop = (Propulsors) this.GetComponent(typeof(Propulsors));
		this.setBehavior(BehaviorTypes.docking); 
		this.setTarget(target); 
		prop.setTargetPos(target.transform.position);	
	}
	
	/**
	 * When docked to a station, undock the ship to it.  
	 */
	public void undock() {
		Debug.Log ("TRY UNDOCK") ;
		Controls playerControls = GameController.getControls(); 
		Transform PlayersList = (Transform) GameController.getPlayerContainer().transform; 
		gameObject.transform.parent = PlayersList ; 
		this.enableAndShow();
		playerControls.switchDockingControls(); 
	}
	
	/**
	 * Every frame executed when ship is commanded to dock a warp or station. 
	 */
	private void dockBehavior() {
		GameObject currentShip = gameObject; 
		if (this.target != null) {
			var remainingDistance = Vector3.Distance(this.target.transform.position, this.transform.position);
			if (remainingDistance < Phobos.Vars.WARP_DISTANCE) {
				Dockable dockData = (Dockable) target.GetComponent(typeof(Dockable));
				this.dockTo (dockData);
			}
			
		}
		
	}
	
	/**
	 * Station or warp is in range, we warp or dock to station.  
	 */
	private void dockTo(Dockable dockData) {
		switch(dockData.type) {
			case Phobos.dockType.station:
				this.dockToStation(); 
			break; 
			case Phobos.dockType.warp:
				this.warpTo(dockData.warpDestination); 
			break; 
		}
	}
	
	/**
	 * Station is in range, we dock it. 
	 */
	private void dockToStation() {
		Controls playerControls = GameController.getControls(); 
		Transform dockingBay = target.transform.FindChild (Phobos.Vars.DOCKINGBAY); 
		gameObject.transform.parent = dockingBay ; 
		this.disableAndHide();
		this.setBehavior (BehaviorTypes.idle);
		playerControls.switchDockingControls(); 
	}
	
	/**
	 * Attack ! 
	 */
	private void attackBehavior() {
		Turrets turr = (Turrets) this.GetComponent(typeof(Turrets));
		int maximumRange = turr.getMaximumRange(); 
		Propulsors prop = (Propulsors) this.GetComponent(typeof(Propulsors));
		Transform currentTarget = turr.getTarget(); 
		if (turr.getAllWeaponsInRange()) {
			prop.stop ();
			prop.lookAt(currentTarget.transform.position); 
		}
		else {
			this.propulsorsGoTo(currentTarget.transform.position); 	
		}
	}
	
	private void warpTo(string sectorName) {
		GameController.switchSector(sectorName); 	
	}
	
	private void collectBehavior() {
		GameObject currentShip = gameObject; 
		Transform cargoBay = transform.FindChild(Phobos.Vars.CARGOBAY); 
		if (this.target != null) {
			
			var remainingDistance = Vector3.Distance(this.target.transform.position, this.transform.position);
			if (remainingDistance < Phobos.Vars.COLLECT_DISTANCE) {
				Cargohold cargoPlayer = (Cargohold) this.GetComponent(typeof(Cargohold));
				Collectable collectableTarg = (Collectable) target.GetComponent(typeof(Collectable));
				if (cargoPlayer.addObjectAtCargo(target, cargoBay)) {
					this.setBehavior (BehaviorTypes.idle);
					this.unsetTarget(); 
				}
			}
		}
		else {
			this.setBehavior (BehaviorTypes.idle);	
		}
	}
	
	
	public void moveToOwn(Vector3 destination) {
		this.addNetInput(Phobos.Commands.MOVE_TO, destination); 
		this.moveTo(destination); 
	}
	
	public void moveTo(Vector3 destination) {
		this.setBehavior(BehaviorTypes.moving); 
		this.unsetTarget(); 
		this.propulsorsGoTo(destination); 
	}
	
	private void propulsorsGoTo(Vector3 destination) {
		Propulsors prop = (Propulsors) this.GetComponent(typeof(Propulsors));
		prop.setTargetPos(destination);
	}
	
	private void enableAndShow() {
		Debug.Log ("Activate");
		gameObject.SetActive(true);	
	}
	
	private void disableAndHide() {
		gameObject.SetActive(false);	
	}
	
	/*
	 * Uses attack script when WE are the controller of this ship.
	 * Implies the net input and the GUI modifications. 
	 */
	public void attackOwn(Transform target) {
		this.addGUIInput(Phobos.Commands.ATTACK, target); 
		this.addNetInput(Phobos.Commands.ATTACK, target); 
		this.attack (target);
	}
	
	//Order the ship to attack the target. 
	public void attack(Transform target) {
		this.setBehavior(BehaviorTypes.attacking); 
		Turrets turr = (Turrets) this.GetComponent(typeof(Turrets));
		turr.attack(target);	
	}
	
	/*
	 * Uses collect script when WE are the controller of this ship.
	 * Implies the net input and the GUI modifications. 
	 */
	public void collectOwn(Transform target) {
		this.addNetInput(Phobos.Commands.COLLECT, target); 
		this.collect (target);
	}
	
	public void collect(Transform target) {
		Propulsors prop = (Propulsors) this.GetComponent(typeof(Propulsors));
		this.setBehavior(BehaviorTypes.collecting); 
		this.setTarget(target); 
		prop.setTargetPos(target.transform.position);	
	}
	
	public void tryBuild(Transform buildingPoint) {
		Build buildObj = (Build) buildingPoint.GetComponent(typeof(Build)); 
		buildObj.build(buildingPoint.position, buildingPoint.rotation); 
	}
	
	
	public void setTarget(Transform newTarget) {
		this.target =  newTarget; 
	}
	
	public void unsetTarget() {
		this.target = null; 
	}
	
	public void setBehavior(BehaviorTypes newBehavior) {
		switch (newBehavior) {
			case BehaviorTypes.idle:
				Propulsors prop = (Propulsors) this.GetComponent(typeof(Propulsors));
				prop.stop ();
			break; 
		}
		this.behavior = newBehavior; 
	}
	
	/**
	 * Adds input stack for the networking 
	 * */
	private void addNetInput(string command, Vector3 data) {
		PlayerNetscript netScript = (PlayerNetscript) this.GetComponent(typeof(PlayerNetscript));
		switch (command) {
			case Phobos.Commands.MOVE_TO:
				netScript.sendNetMoveTo(data); 
			break; 
		}
	}
	
	/**
	 * Adds input stack for the networking 
	 * */
	private void addNetInput(string command, Transform data) {
		PlayerNetscript netScript = (PlayerNetscript) this.GetComponent(typeof(PlayerNetscript));
		switch (command) {
			case Phobos.Commands.ATTACK:
				netScript.sendNetAttack(data);  
			break; 
			
			case Phobos.Commands.COLLECT:
				netScript.sendNetCollect(data);  
			break; 
		}
	}
	
	/**
	 * Adds input stack for the networking 
	 * */
	private void addNetInput(string command) {
		
	}
	
	/**
	 * Affects the GUI with new command
	 * */
	private void addGUIInput(string command, Transform data) {
		var GUIContainer = GameObject.FindGameObjectWithTag("GUIContainer");
		if (GUIContainer) {
			var attackTargetGUI = GUIContainer.transform.FindChild("AttackTarget");
			LabelPositioning labelPos = (LabelPositioning) attackTargetGUI.GetComponent(typeof(LabelPositioning));
			labelPos.target = data;
			labelPos.enableTexture();
		}
	}
	
}