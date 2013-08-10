using UnityEngine;
using System.Collections;

//Turret required, turrets is the turret manager
[RequireComponent (typeof (Turret))]

public class Turrets : MonoBehaviour {
	
	public GameObject target = null;
	
	public ArrayList turrets;
	public Turret[] turrets2;
	
	public Turret theWeapon;
	
	// Use this for initialization
	void Start () {
		this.theWeapon = (Turret) gameObject.AddComponent("Turret");
		this.setTarget(GameObject.Find("Bot"));
	}
	
	// Update is called once per frame
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
	
	private void fireWith() {
			
	}
	
	public bool hasTarget() {
		return (this.target != null); 	
	}
	
	public void setTarget(GameObject newTarget) {
		this.target = newTarget;	
		this.theWeapon.setTarget(newTarget);
	}
}