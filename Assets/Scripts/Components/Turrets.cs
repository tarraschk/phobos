using UnityEngine;
using System.Collections;

//Turret required, turrets is the turret manager
//Propulsors required 

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
	
	public void setTarget(Transform newTarget) {
		this.target = newTarget;	
		this.theWeapon.setTarget(newTarget);
		if (this.isPlayer()) {
			this.setGUIAttackTarget(newTarget);
		}
	}
	
	public void attack (Transform target) {
		Debug.Log ("ATTAAACK");
		//var attackPosition =  Vector3.Lerp(this.transform.position, target.transform.position, 0.2f);
		Propulsors prop = (Propulsors) this.GetComponent(typeof(Propulsors));
		prop.setTargetPos(target.position); //TODO CALCULATE RIGHT POSITION, SO THAT IT DOESNT GO ALL THE WAY
		//prop.setTarget(target);
		this.setTarget(target);	
	}
	
	public bool isPlayer() {
		return this.tag == "Player"; 	
	}
	
	private void setGUIAttackTarget(Transform newTarget) {
		var GUIContainer = GameObject.FindGameObjectWithTag("GUIContainer");
		if (GUIContainer) {
			var attackTargetGUI = GUIContainer.transform.FindChild("AttackTarget");
			LabelPositioning labelPos = (LabelPositioning) attackTargetGUI.GetComponent(typeof(LabelPositioning));
			labelPos.target = newTarget.transform;
			labelPos.enableTexture();
		}
	}
}