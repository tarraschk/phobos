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
	
	public bool hasTarget() {
		return (this.target != null); 	
	}
	
	public void setTarget(GameObject newTarget) {
		this.target = newTarget;	
		this.theWeapon.setTarget(newTarget);
		if (this.isPlayer()) {
			this.setGUIAttackTarget(newTarget);
		}
	}
	
	public void attack (GameObject target) {
		this.setTarget(target);	
		Propulsors prop = (Propulsors) this.GetComponent(typeof(Propulsors));
		prop.setTargetPos(target.transform);
	}
	
	public bool isPlayer() {
		return this.tag == "Player"; 	
	}
	
	private void setGUIAttackTarget(GameObject newTarget) {
		var GUIContainer = GameObject.FindGameObjectWithTag("GUIContainer");
		if (GUIContainer) {
			var attackTargetGUI = GUIContainer.transform.FindChild("AttackTarget");
			LabelPositioning labelPos = (LabelPositioning) attackTargetGUI.GetComponent(typeof(LabelPositioning));
			labelPos.target = newTarget.transform;
			labelPos.enableTexture();
		}
	}
}