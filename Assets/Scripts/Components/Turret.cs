
using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
	
	//Turret reload speed
	public float cooldownTime = 0.28f;
	public Cooldown cooldown ;
	
	//Turret energy damage
	public int power = 50;
	
	//How long can the turret shoot ? 
	public int range = 40 ; 
	
	//The display name
	public string wName = "Electrifier";
	
	//Current turret's target
	public Transform target = null;
	
	//Is it ready to shoot ? 
	public bool ready = true;
	
	//Where the turret's projectile will spawn
	public Vector3 laserSpawnPosition ; 
	
	//The turret main model and prefab. 
	public GameObject prefab; 
	
	//The turret's projectile prefab
	private GameObject projectile ;
	
	
	public Turret(string nwName = "Electrifier", GameObject nprojectile = null, int npower = 50, int nrange = 40, float ncooldownTime = 0.28f) {
		this.cooldownTime = ncooldownTime; 
		this.wName = nwName; 
		this.projectile = nprojectile; 
		this.power = npower; 
		this.range = nrange; 
	}
	
	void Start() {
		cooldown = new Cooldown(this.cooldownTime, false); //Instantiate the cooldown. 
		this.createTurretModel(); 
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
		GameObject projectile = (GameObject) Instantiate(Resources.Load ("Prefabs/Weapons/Projectile/Laser1"), (this.transform.position + new Vector3(-2.5f, -1f, 0.5f)), this.transform.rotation) ; 

		moveTo moveScript = projectile.GetComponent<moveTo>();
		Laser laserScript = projectile.GetComponent<Laser>();
		moveScript.startMarker = this.transform;
			moveScript.endMarker = this.target.transform;
		laserScript.setTarget(this.target.transform);
		laserScript.setAttacker(gameObject);
	}
	
	public void setProjectile(GameObject newProjectile) {
		this.projectile = newProjectile; 	
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
			if (this.checkTargetInRange() && this.isReady ()) {
				return true; 	
			}
			else return false; 
		}
		
		else return false; 
	}
	
	public bool checkTargetInRange() {
		if (target != null) 
			return (this.range >= Vector3.Distance(this.transform.position, target.transform.position)); 
		else return false ;
	}
	
	public bool isReady() {
		return this.ready;	
	}
	
	private void createTurretModel() {
			
	}
}
