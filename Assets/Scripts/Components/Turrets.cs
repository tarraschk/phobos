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
		this.theWeapon = (Turret) gameObject.AddComponent("Turret");
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
			this.theWeapon.fire();
		}
		/**this.turrets2[0].tryFire();
		foreach (Turret t in this.turrets2) {
			Debug.Log (t);
		}*/
	}
	
	public bool hasTarget() {
		return (this.target != null); 	
	}
	
	public void setTarget(Transform newTarget) {
		this.target = newTarget;	
		this.theWeapon.setTarget(newTarget);
	}
	
	public void attack (Transform target) {
		Propulsors prop = (Propulsors) this.GetComponent(typeof(Propulsors));
		prop.setTargetPos(target.position); //TODO CALCULATE RIGHT POSITION, SO THAT IT DOESNT GO ALL THE WAY
		this.setTarget(target);	
	}
	
	public bool isPlayer() {
		return this.tag == "Player"; 	
	}
	
	
}