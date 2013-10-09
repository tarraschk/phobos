using UnityEngine;
using System.Collections;

public class Destructible : Photon.MonoBehaviour {

	public int energy = 500 ; 
	public int energyMax = 500 ; 
	public int level = 1 ; 
	
	public void receiveDamage(int power, GameObject attacker) {
		if (this.tag == "AI")
		{
			this.AIReact(attacker); 	
		}
		this.setEnergy(this.energy - power); 
		if (this.energy <= 0) {
			this.destroy() ; 	
			PlayerStats levelScript = (PlayerStats) attacker.GetComponent(typeof(PlayerStats));
			levelScript.doGainXP(this.level * 25) ; 
			levelScript.doGainMoney(this.level * 15) ; 
		}
	}
	
	public void setEnergy(int newEnergy) {
		this.energy = newEnergy ; 	
	}
	
	public void destroy() {
		GameObject explosion = (GameObject) Instantiate(Resources.Load ("Prefabs/FX/explosion"), this.transform.position, this.transform.rotation) ; 
	
		if (PhotonNetwork.isMasterClient) {
			ContainLoot CL = (ContainLoot) this.GetComponent(typeof(ContainLoot));
			if (CL != null) {
				//Has loot, we spawn it !
				CL.activateLootSpawn(1); 
			}
			PhotonNetwork.Destroy(gameObject);
		}
	}
	
	private void AIReact(GameObject attacker) {
		botBehavior behavior = (botBehavior) this.GetComponent(typeof(botBehavior));
		behavior.isUnderAttackBy(attacker);
	}
}
