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
			this.destroy(false) ; 	
			PlayerStats levelScript = (PlayerStats) attacker.GetComponent(typeof(PlayerStats));
			ShipController attackerController = (ShipController) attacker.GetComponent(typeof(ShipController));
			attackerController.setBehavior(BehaviorTypes.idle); 
			if (levelScript != null) {	
				levelScript.doGainXP(this.level * 25) ; 
				levelScript.doGainMoney(this.level * 15) ; 
			}
		}
	}
	
	public void setEnergy(int newEnergy) {
		this.energy = newEnergy ; 	
	}
	
	public void destroy(bool isWreckage) {
		GameObject explosion = (GameObject) Instantiate(Resources.Load ("Prefabs/FX/explosion"), this.transform.position, this.transform.rotation) ; 
	
		if (PhotonNetwork.isMasterClient) {
			Debug.Log ("i am master and im dead");
			ContainLoot CL = (ContainLoot) this.GetComponent(typeof(ContainLoot));
			if (CL != null) {
				//Has loot, we spawn it !
				CL.activateLootSpawn(1); 
			}
			PhotonNetwork.AllocateViewID(); 
			PhotonNetwork.Destroy(gameObject);
		}
		
		if (isWreckage) {
				
		}
	}
	
	private void AIReact(GameObject attacker) {
		botBehavior behavior = (botBehavior) this.GetComponent(typeof(botBehavior));
		behavior.isUnderAttackBy(attacker);
	}
}
