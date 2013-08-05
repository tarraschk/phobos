using UnityEngine;
using System.Collections;

public class botBehavior : MonoBehaviour {
	
	public enum AITypes{idle, attack, returnToPos};
	
	private AITypes AI = AITypes.idle;
	
	private float targetAttackRange = 50f; 
	
	void Update () {
		AIBehavior ();
	}
	
	void AIBehavior() {
				Debug.Log (this.AI);
		switch(this.AI) {
			
			case AITypes.idle:
				this.idleBehavior();
			break;
			
			case AITypes.attack:
				this.attackBehavior();
			break;
		
			
			case AITypes.returnToPos:
			
			break;
			
		}
	}
	
	void idleBehavior() {
		var foundUni = GameObject.Find("Universe");
		
		Universe univ = (Universe) foundUni.GetComponent(typeof(Universe));
		var univPlayers = univ.getPlayers();
		for (var i = 0 ; i < univPlayers.Length ; i++) {
			
			var remainingDistance = Vector3.Distance(univPlayers[i].transform.position, this.transform.position);
				Debug.Log (remainingDistance);
			if (remainingDistance < this.targetAttackRange) 
			{
				Propulsors prop = (Propulsors) this.GetComponent(typeof(Propulsors));
				
				prop.setTargetPos(univPlayers[i].transform);	
				this.setAI(AITypes.attack);
			}
		}
	}
	
	void attackBehavior() {
			
	}
	
	void setAI(AITypes newAI) {
		this.AI = newAI;
	}
}
