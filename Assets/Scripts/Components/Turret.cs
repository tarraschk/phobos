
using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	public float cooldown = .25f;
	public int power = 50;
	public int range = 100 ; 
	public string wName = "Electrifier";
	public GameObject target = null;
	
	public bool ready = true;
	private float currentCooldown ;
	private float lastCooldown ; 
	private GameObject projectile ;
	
	void Start() {
		Debug.Log ("TURRET");
	}
	
	void Update () {
		this.cooldownUpdate();
	}
	void cooldownUpdate() {
		this.currentCooldown = Time.time;
		if (!this.isReady()) {
			if ((this.currentCooldown - this.lastCooldown) >= cooldown) {
				this.setReady(true);	
			}
		}
	}
	
	
	public void fire() {
		this.setReady (false);
		this.lastCooldown = Time.time;
		Debug.Log("Feu !");
		GameObject projectile = (GameObject) Instantiate(Resources.Load ("Prefabs/Weapons/Projectile/Laser1"), this.transform.position, this.transform.rotation) ; 

		moveTo moveScript = projectile.GetComponent<moveTo>();
		Laser laserScript = projectile.GetComponent<Laser>();
		moveScript.startMarker = this.transform;
		moveScript.endMarker = this.target.transform;
		laserScript.setTarget(this.target);
	}
	
	public void setReady(bool newReady) {
		this.ready = newReady;	
	}
	
	public void setTarget(GameObject newTarget) {
		this.target = (newTarget);	
	}
	
	public bool isCanFire() {
		if (this.range >= Vector3.Distance(this.transform.position, target.transform.position) && this.isReady ()) {
			return true; 	
		}
		else return false; 
	}
	
	public bool isReady() {
		return this.ready;	
	}
}
