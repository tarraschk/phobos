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
		}
	}
	
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
	
	private void dockTo(Dockable dockData) {
		switch(dockData.type) {
			case Phobos.dockType.station:
			break; 
			case Phobos.dockType.warp:
				this.warpTo(dockData.warpDestination); 
			break; 
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
	
	public void moveTo(Vector3 destination) {
		this.addNetInput(Phobos.Commands.MOVE_TO, destination); 
		this.setBehavior(BehaviorTypes.moving); 
		this.unsetTarget(); 
		Propulsors prop = (Propulsors) this.GetComponent(typeof(Propulsors));
		prop.setTargetPos(destination);
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
