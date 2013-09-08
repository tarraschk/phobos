using UnityEngine;
using System.Collections;

//Turret required, turrets is the turret manager
//Propulsors required (TODO)

[RequireComponent (typeof (Turret))]

public class Turrets : MonoBehaviour {
	
	public Transform target = null;
	
	public ArrayList turrets;
	public Turret[] turrets2;
	
	public Turret theWeapon;
	
	// Use this for initialization
	void Start () {
		turrets = new ArrayList(); 
		Turret test1 = (Turret) gameObject.AddComponent("Turret");
		Turret test2 = (Turret) gameObject.AddComponent("Turret"); 
		this.theWeapon = (Turret) gameObject.AddComponent("Turret");
		this.turrets.Add(test1); 
		this.turrets.Add(test2); 
	}
	
	/**
	 * Main Update
	 */
	void Update () {
		if (this.hasTarget()) {
			this.hasTargetBehavior();
		}
	}
	
	private void hasTargetBehavior() {
		if (this.theWeapon.isCanFire()) 
		{
			//this.theWeapon.fire();
		}
		foreach(Turret t in this.turrets) {
				Debug.Log (t.isCanFire());
			if (t.isCanFire()) 
			{
				Debug.Log ("FIIIIRE");
				t.fire();
			}
		}
		/**this.turrets2[0].tryFire();
		foreach (Turret t in this.turrets2) {
			Debug.Log (t);
		}*/
	}
	
	public bool hasTarget() {
		return (this.target != null); 	
	}
	
	public void unsetTarget() {
		this.target = null ; 	
	}
	
	public void setTarget(Transform newTarget) {
		this.target = newTarget;
		foreach(Turret t in this.turrets) {
			t.setTarget(newTarget); 	
		}
		this.theWeapon.setTarget(newTarget);
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
		return this.theWeapon.range ; 	
	}
	
	public bool getAllWeaponsInRange() {
		//foreach;  @TODO
		return (theWeapon.checkTargetInRange());
	}
	
	
}