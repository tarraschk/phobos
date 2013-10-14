using UnityEngine;
using System.Collections;
using SimpleJSON; 

//Turret required, turrets is the turret manager
//Propulsors required (TODO)

[RequireComponent (typeof (Turret))]

public class Turrets : MonoBehaviour {
	
	public Transform target = null;
	
	public ArrayList currentEquipment; //Initialized first. Contains only the names and ids of the equipment, not the object. 
	
	//Turrets will be based on current equipment, and will be instanciated after data is read and loaded in currentEquipment. 
	public Turret[] turrets = new Turret[Phobos.Gameplay.EQUIPMENT_MAX]{null, null,null,null,null,null,null,null,null,null}; 
	
	/**
	 * Component initiated
	 */
	void Awake () {
		this.initiateTurrets() ; 
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
	 * Initiate equipment to initial value. 
	 * Will be used later by the object initiator. 
	 */
	public void initiateEquipment() {
		this.currentEquipment = new ArrayList();
		// ON LIT EN DUR DES DONNEES, normalement c du JSON envoyé par le serveur pou rce joueur !!
	}
	
	/**
	 * Push a new equipment.
	 * Used by object initiator to push player's equipment
	 */
	public void pushEquipment(string newEquipment) {
		this.currentEquipment.Add(newEquipment); 
	}
	
	/**
	 * Remove an equipment by the id 
	 * in the array currentEquipment
	 */
	public void removeEquipment(int equipmentId) {
		string turretName ; 
		Turret toRemoveTurret = this.turrets[equipmentId] ; 
		turretName = toRemoveTurret.wName; 
		toRemoveTurret.removeTurret(); 
		turrets[equipmentId] = null ; 
		this.currentEquipment.Remove(turretName);  
	}
	
	/**
	 * @TODO
	 * To be changed to something proper. 
	 * Initiate all the turrets, based on the player's ship data. 
	 * The ship data has been loaded before 
	 * and has pushed the equipment data to 
	 * this.currentEquipment. 
	 */
	private void initiateTurrets() {
		
		GameObject electrifierProjectile = (GameObject) Resources.Load ("Prefabs/Weapons/Projectile/Laser1") ; 
		GameObject helloProjectile = (GameObject) Resources.Load ("Prefabs/Weapons/Projectile/Laser2") ; 
		GameObject ElectrifierObject = (GameObject) Resources.Load ("Prefabs/Weapons/Turret/Electrifier1") ; 
		int i = 0; 
		foreach (string equipName in this.currentEquipment) {
			this.addTurret(equipName, i); 
			i++;
		}
	}
	
	/**
	 * Sets the proprety of an instantiated turret component. Get the game
	 * data associated to this turret.
	 */
	private void initNewTurret(string turretName, Turret newTurret, int turretId) {
		GameObject electrifierProjectile = (GameObject) Resources.Load ("Prefabs/Weapons/Projectile/Laser1") ; 
		GameObject ElectrifierObject = (GameObject) Resources.Load ("Prefabs/Weapons/Turret/Electrifier1") ; 
		var gameplayData = GameController.getGameplayData() ; 
		JSONNode turretJSONData = gameplayData.getTurret(turretName); //We get the game data about the turret
		Debug.Log(turretJSONData); 
		newTurret.cooldownTime = gameplayData.getFloatNodeProprety("cooldownTime", turretJSONData);   
		newTurret.wName = gameplayData.getStringNodeProprety("fullName", turretJSONData);    
		newTurret.setProjectile(electrifierProjectile); 
		newTurret.power = gameplayData.getIntNodeProprety("power", turretJSONData) ;
		newTurret.range = gameplayData.getIntNodeProprety("range", turretJSONData); 
		newTurret.setTurretPrefab(ElectrifierObject); 
		newTurret.setLaserSpawnPosition(Phobos.EquipmentPoints.playerShip1.points[turretId]); 
		newTurret.instantiateTurretPrefab(); 	
	}
	
	private void addTurret(string turretName, int turretId) {
		/**
		 * To be deleted when turret data will be in JSON !
		 **/
		GameObject electrifierProjectile = (GameObject) Resources.Load ("Prefabs/Weapons/Projectile/Laser1") ; 
		GameObject helloProjectile = (GameObject) Resources.Load ("Prefabs/Weapons/Projectile/Laser2") ; 
		GameObject ElectrifierObject = (GameObject) Resources.Load ("Prefabs/Weapons/Turret/Electrifier1") ; 
		
		Turret newTurret = (Turret) gameObject.AddComponent("Turret");
		this.initNewTurret(turretName, newTurret, turretId); 
		this.turrets[turretId] = newTurret; 
	}
	
	public bool hasTurrets() {
		Turret hasTurret = (Turret) this.GetComponent(typeof(Turret));
		return (hasTurret != null) ; 	
	}
	
	/**
	 * Main Update if we have a target for the turrets. 
	 */
	private void hasTargetBehavior() {
		if (this.hasTurrets()) {
			foreach(Turret t in this.turrets) {
				if (t != null) {
					if (t.isCanFire()) 
					{
						t.fire();
					}
				}
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
		if (this.hasTurrets()) {
			foreach (Turret t in this.turrets) {
				if (t != null) 
					t.setTarget(newTarget);
			}
		}
	}
	
	public Transform getTarget() {
		return this.target; 	
	}
	
	/**
	 * Returns true if attack order is successful 
	 */
	public bool attack (Transform target) {
		Propulsors prop = (Propulsors) this.GetComponent(typeof(Propulsors));
		prop.setTargetPos(target.position); //TODO CALCULATE RIGHT POSITION, SO THAT IT DOESNT GO ALL THE WAY
		this.setTarget(target);	
		return true; 
	}
	
	public bool canAttack() {
		return (this.hasTurrets()) ; 	
	}
	
	public bool isPlayer() {
		return this.tag == "Player"; 	
	}
	
	public int getMaximumRange() {
		int maxRange = 0 ; 
		if (this.hasTurrets()) {
			foreach(Turret t in this.turrets) {
				if (t != null) 
					if (t.range < maxRange)
						maxRange = t.range ; 
			}
		}
		return maxRange; 
	}
	
	public bool getAllWeaponsInRange() {
		foreach(Turret t in this.turrets) {
			if (t != null) 
				if (!t.checkTargetInRange())
					return false;	
		}
		return true; 
	}
	
	
}