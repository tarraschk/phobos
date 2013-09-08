using UnityEngine;
using System.Collections;

public class MainGUI : MonoBehaviour {
	
	public Transform GUITarget ; 
	public bool active = false ; 
	
	public Destructible TargetDestr ; 
	public Cargohold TargetCargo ; 
	public PlayerStats TargetStats ; 
	public ShipController TargetController ; 
	public Controls playerControls ; 
	
	public Texture inventorySlot ; 
	
	public bool showBuildMenu = false ; 
	public bool showDockMenu = false ; 
	
	void Start() {
	}
	
	void OnGUI () {
		if (active) {
			GUI.backgroundColor = Color.cyan;
			this.inventory(); 
			this.playerStats(); 
			if (this.showBuildMenu) {
				this.buildMenu();	
			}
			if (this.showDockMenu) {
				this.dockMenu(); 	
			}
		}
	}
	
	/**
	 * Show GUI when ship is building stuff. 
	 **/
	
	public void toggleShowBuildMenu() {
		this.setShowBuildMenu(!this.showBuildMenu); 	
	}
	
	
	/**
	 * Show GUI when ship is docked to a station. 
	 **/
	public void toggleShowDockMenu() {
		this.setShowDockMenu(!this.showDockMenu); 	
	}
	
	public void setGUITarget(Transform newPlayer) {
		this.GUITarget = newPlayer;
		playerControls = (Controls) GameController.getControls(); 
		TargetController = (ShipController) this.GUITarget.GetComponent(typeof(ShipController));
		TargetDestr = (Destructible) this.GUITarget.GetComponent(typeof(Destructible));
		TargetCargo = (Cargohold) this.GUITarget.GetComponent(typeof(Cargohold));	 
		TargetStats = (PlayerStats) this.GUITarget.GetComponent(typeof(PlayerStats));	 
	}
	
	public void setActive(bool newActive) {
		this.active = newActive ; 
	}
	
	private void playerStats() {
		GUI.Box (new Rect (10,Screen.height - 100,100,30), "Health : " + TargetDestr.energy);
		GUI.Box (new Rect (10,Screen.height - 70,150,30), "Level " + TargetStats.level + " (" +TargetStats.currentXP +" / " + TargetStats.requiredXP +")");
		GUI.Box (new Rect (10,Screen.height - 50,100,30), "$" + TargetStats.money);
	}
	
	private void inventory() {
		if (!this.inventorySlot) {
			Debug.LogError("Missing inventory texture.");
			return; 
		}
		string cargoNames = ""; 
		var cargoHold = TargetCargo.getCargoContent(); 
		foreach (Transform trans in cargoHold.transform) {
			Collectable collectItem = (Collectable) trans.GetComponent(typeof(Collectable));
			cargoNames = collectItem.quantity + " "; 
			GUI.Label (new Rect (Screen.width - 120,110,64,64), new GUIContent(cargoNames, collectItem.icon));
			
		}
		foreach (Transform j in cargoHold) {
			cargoNames += j.name + "\n"; 
		}
		if (GUI.Button (new Rect (10,Screen.height - 200,100,30), "Build")) {
			this.buildButtonDown(); 
		}
		GUI.Box (new Rect (Screen.width - 150,75,100,30), "Cargo : " + TargetCargo.capacity + " / " + TargetCargo.capacityMax);
		//GUI.Button (new Rect (10,170,250,100), "Cargo : " + TargetCargo.capacity + " / " + TargetCargo.capacityMax +"\n" + cargoNames);
	}
	
	private void buildButtonDown() {
		GameController.getControls().switchBuildingControls(); 
	}
	
	private void buildMenu() {
		string building1 = "Cruiser"; 
		if (GUI.Button (new Rect (Screen.width / 2,Screen.height - 250,100,30), building1)) {
			this.buildingButtonDown(building1); 
		}
	}
	
	private void dockMenu() {
		if (GUI.Button (new Rect (Screen.width / 2,Screen.height - 250,100,30), "YOU ARE DOCK. CLICK TO UNDOCK. ")) {
			this.undockButtonDown(); 
		}
	}
	
	private void buildingButtonDown(string buildingName) {
		GameController.createBuildingPreview(buildingName); 
	}
	
	private void undockButtonDown() {
		playerControls.undock(); 
	}
		
	private void setShowDockMenu(bool newShowDockMenu) {
		this.showDockMenu = newShowDockMenu ; 	
	}
	
	private void setShowBuildMenu(bool newShowBuildMenu) {
		this.showBuildMenu = newShowBuildMenu ; 	
	}
}
