using UnityEngine;
using System.Collections;

public class Destructible : Photon.MonoBehaviour {

	public int energy = 500 ; 
	public int level = 1 ; 
	
	public void receiveDamage(int power, GameObject attacker) {
		if (this.tag == "AI")
		{
			this.AIReact(attacker); 	
		}
		this.setEnergy(this.energy - power); 
		if (this.energy <= 0) {
			this.destroy() ; 	
			Level levelScript = (Level) attacker.GetComponent(typeof(Level));
			levelScript.doGainXP(this.level * 25) ; 
		}
	}
	
	public void setEnergy(int newEnergy) {
		this.energy = newEnergy ; 	
	}
	
	public void destroy() {
		GameObject explosion = (GameObject) Instantiate(Resources.Load ("Prefabs/FX/explosion"), this.transform.position, this.transform.rotation) ; 
	
		if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.Destroy(gameObject);
		}
	}
	
	private void AIReact(GameObject attacker) {
		botBehavior behavior = (botBehavior) this.GetComponent(typeof(botBehavior));
		behavior.isUnderAttackBy(attacker);
	}
}
