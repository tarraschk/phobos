using UnityEngine;
using System.Collections;

/*
 * Manages the particles of a thruster
 * Object must use this hierarchy : 
 * 
 * GameObject - Model - Thruster
 * 
 * Script CANT BE USED OTHERWISE
 * 
 */
public class ThrusterParticle : MonoBehaviour {
	
	public float thrusterPower ; 
	private float thrusterCoefficient = 0.1f; 
	private float thrusterMinSize = 3f; 
	private float thrusterMinSpeed = 10f; 
	private Propulsors referencePropulsors ; 
	void Awake() {
		this.findAndAllocateThrusters(); 
	}
	
	void Update () {
		if (this.referencePropulsors != null)
			thrusterPowerUpdate(); 
		else this.findAndAllocateThrusters(); 
	}
	
	private void findAndAllocateThrusters() {
		if (gameObject.transform.parent.parent) {
			var motherObject = gameObject.transform.parent.parent; 
			
			//mother Object must exist and have propulsors
			referencePropulsors = (Propulsors) motherObject.GetComponent(typeof(Propulsors));
			if (referencePropulsors != null)
				enabled = true ; 
			else enabled = false ; 
		}	
	}
	
	private void thrusterPowerUpdate() {
		this.thrusterPower = this.referencePropulsors.speed * this.thrusterCoefficient ; 
		this.allocateThrusterPowerToParticle(this.thrusterPower, this.referencePropulsors.speed); 
	}
	
	private void allocateThrusterPowerToParticle(float thrusterPower, float thrusterSpeed) {
		particleSystem.startSize = thrusterPower + this.thrusterMinSize; 
		particleSystem.startSpeed = thrusterSpeed * 2 + this.thrusterMinSpeed ;
	}
}
