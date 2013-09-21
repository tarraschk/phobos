using UnityEngine;
using System.Collections;

[RequireComponent (typeof (moveTo))]

public class Laser : MonoBehaviour {

	public Transform Target ; 
	public GameObject attacker ; 
	public float speed = 20f;  
	public int power = 25; 
	public double energy = 10f ; 
	
	private double energyFade = 7.5f;
	
	// Update is called once per frame
	void Update () {
		if (Target != null) {
			this.energyUpdate ();
			if (this.energy <= 0 || this.Target == null) 
				this.laserDestroy(); 
		}
	}
	void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.name == Target.name) {
			this.laserDestroy();
			Destructible destr = (Destructible) Target.GetComponent(typeof(Destructible));
			destr.receiveDamage(this.power, this.attacker);
		
			
		}
    }
	
	public void setTarget(Transform newTarget) {
		this.Target = newTarget;	
	}
	
	public void setAttacker(GameObject newAttacker) {
		this.attacker = newAttacker; 	
	}
	
	private void energyUpdate() {
		this.energy -= this.energyFade * Time.deltaTime ;
	}
	
	private void laserDestroy() {
		Destroy (gameObject);
	}
}
