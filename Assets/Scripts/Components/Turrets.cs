using UnityEngine;
using System.Collections;

//Turret required, turrets is the turret manager
//Propulsors required (TODO)

[RequireComponent (typeof (Turret))]

public class Turrets : MonoBehaviour {
	
	public Transform target = null;
	
	public ArrayList currentEquipment; //RAW DATA FOR TESTING PURPOSES. MUST BE REPLACED BY A JSON OBJECT THAT IS THE OBJECT REAL WEAPON DATA. 
	public Turret[] turrets = new Turret[3]{null, null, null}; 
	public int turretsCount = 3;
	
	public Turret theWeapon;
	
	// Use this for initialization
	void Start () {
		this.initiateTurrets() ; 
		//this.theWeapon = (Turret) gameObject.AddComponent("Turret");
	}
	
	/**
	 * Main Update
	 */
	void Update () {
		if (this.hasTarget()) {
			this.hasTargetBehavior();
		}
	}
	
	/**
	 * Initiate all the turrets, based on the player's ship data. To be changed to something proper. 
	 */
	private void initiateTurrets() {
		
		GameObject electrifierProjectile = (GameObject) Resources.Load ("Prefabs/Weapons/Projectile/Laser1") ; 
		GameObject helloProjectile = (GameObject) Resources.Load ("Prefabs/Weapons/Projectile/Laser2") ; 
		GameObject ElectrifierObject = (GameObject) Resources.Load ("Prefabs/Weapons/Turret/Electrifier1") ; 
		int i = 0; 
		this.initiateTEST(); // ON LIT EN DUR DES DONNEES, normalement c du JSON envoyé par le serveur pou rce joueur !!
		foreach (string equip in this.currentEquipment) {
			Turret newTurret = (Turret) gameObject.AddComponent("Turret");
			Debug.Log (equip);
			switch(equip) {
				case "Electrifier":
				Debug.Log ("set turret script");
					newTurret.cooldownTime = 0.28f; 
					newTurret.wName = "Electrifierrrr"; 
					newTurret.setProjectile(electrifierProjectile); 
					newTurret.power = 1; 
					newTurret.range = 40; 
				break;
				
				case "HelloLaser":
					newTurret.cooldownTime = 1.28f; 
					newTurret.wName = "HelloLaser"; 
					newTurret.setProjectile(helloProjectile); 
					newTurret.power = 5; 
					newTurret.range = 60; 
				break ;
			}
			this.turrets[i] = newTurret; 
			i++;
		}
	}
	
	private void initiateTEST() {
		this.currentEquipment = new ArrayList(); 
		this.currentEquipment.Add("Electrifier"); 
		this.currentEquipment.Add("Electrifier"); 
		this.currentEquipment.Add("HelloLaser"); 
	}
	
	/**
	 * Main Update if we have a target for the turrets. 
	 */
	private void hasTargetBehavior() {
		foreach(Turret t in this.turrets) {
			if (t.isCanFire()) 
			{
				t.fire();
			}
		}
	}
	
	public bool hasTarget() {
		return (this.target != null); 	
	}
	
	public void unsetTarget() {
		this.target = null ; 	
	}
	
	public void setTarget(Transform newTarget) {
		this.target = newTarget;
		foreach (Turret t in this.turrets) {
			Debug.Log ("SET TURRET 2 " + t);
			t.setTarget(newTarget);
		}
	}
	
	public Transform getTarget() {
		return this.target; 	
	}
	
	public void attack (Transform target) {
		Propulsors prop = (Propulsors) this.GetComponent(typeof(Propulsors));
		prop.setTargetPos(target.position); //TODO CALCULATE RIGHT POSITION, SO THAT IT DOESNT GO ALL THE WAY
		this.setTarget(target);	
	}
	
	public bool isPlayer() {
		return this.tag == "Player"; 	
	}
	
	public int getMaximumRange() {
		int maxRange = 0 ; 
		foreach(Turret t in this.turrets) {
			if (t.range < maxRange)
				maxRange = t.range ; 
		}
		return maxRange; 
	}
	
	public bool getAllWeaponsInRange() {
		foreach(Turret t in this.turrets) {
			if (!t.checkTargetInRange())
				return false;	
		}
		return true; 
	}
	
	
}