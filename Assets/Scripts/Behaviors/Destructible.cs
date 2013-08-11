using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {

	public int energy = 500 ; 
	
	public void receiveDamage(int power, GameObject attacker) {
		if (this.tag == "AI")
		{
			this.AIReact(attacker); 	
		}
		this.setEnergy(this.energy - power); 
		if (this.energy <= 0) {
			this.destroy() ; 	
		}
	}
	
	public void setEnergy(int newEnergy) {
		this.energy = newEnergy ; 	
	}
	
	public void destroy() {
		GameObject explosion = (GameObject) Instantiate(Resources.Load ("Prefabs/FX/explosion"), this.transform.position, this.transform.rotation) ; 
		Destroy (gameObject);
	}
	
	private void AIReact(GameObject attacker) {
		botBehavior behavior = (botBehavior) this.GetComponent(typeof(botBehavior));
		behavior.isUnderAttackBy(attacker);
	}
}
