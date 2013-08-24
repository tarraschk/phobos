
using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
	
	//Turret reload speed
	public float cooldownTime = 0.28f;
	public Cooldown cooldown ;
	
	//Turret energy damage
	public int power = 50;
	
	//How long can the turret shoot ? 
	public int range = 100 ; 
	
	//The display name
	public string wName = "Electrifier";
	
	//Current turret's target
	public Transform target = null;
	
	//Is it ready to shoot ? 
	public bool ready = true;
	
	//The turret's projectile prefab
	private GameObject projectile ;
	
	void Start() {
		cooldown = new Cooldown(this.cooldownTime, false); //Instantiate the cooldown. 
	}
	
	/**
	 * Main Update
	 */
	void Update () {
		this.cooldown.Update();
		if (this.cooldown.isReady()) {
			this.setReady(true);
		}
	}
	
	/**
	 * Fire ! Instantiate laser and tick the cooldown
	 */
	public void fire() {
		this.setReady (false);
		this.cooldown.cooldownTick(); 
		GameObject projectile = (GameObject) Instantiate(Resources.Load ("Prefabs/Weapons/Projectile/Laser1"), this.transform.position, this.transform.rotation) ; 

		moveTo moveScript = projectile.GetComponent<moveTo>();
		Laser laserScript = projectile.GetComponent<Laser>();
		moveScript.startMarker = this.transform;
		moveScript.endMarker = this.target.transform;
		laserScript.setTarget(this.target.transform);
		laserScript.setAttacker(gameObject);
	}
	
	public void setReady(bool newReady) {
		this.ready = newReady;	
	}
	
	public void setTarget(Transform newTarget) {
		this.target = (newTarget);	
	}
	
	/**
	 * Ready to shoot ? 
	 * Checks the possibility of fire : distance, cooldown and target. 
	 */
	public bool isCanFire() {
		if (target != null) {
			if (this.range >= Vector3.Distance(this.transform.position, target.transform.position) && this.isReady ()) {
				return true; 	
			}
			else return false; 
		}
		
		else return false; 
	}
	
	public bool isReady() {
		return this.ready;	
	}
}
