/**
 * REQUIREMENTS : A bot must have those components : 
 * -Propulsors.cs
 * -Turrets.cs
 * 
 * 
 */
using UnityEngine;
using System.Collections;

public class botBehavior : MonoBehaviour {
	
	public enum AITypes{idle, attack, returnToPos};
	
	private AITypes AI = AITypes.idle;
	private Transform initPosition  ;
	private float targetAttackRange = 50f; 
	private float targetReturnIdleRange = 150f; 
	
	void Start() {
		Vector3 initialVector = new Vector3(this.transform.position.x,this.transform.position.y, this.transform.position.z);
		Transform botInitPos = new GameObject(this.name+"InitPos").transform;
		botInitPos.position = initialVector;
		this.setInitPosition(botInitPos);	
	}
	
	void Update () {
		AIBehavior ();
	}
	
	public void setInitPosition(Transform newTransform) {
		this.initPosition = newTransform;
	}
	
	public void isUnderAttackBy(GameObject attacker) {
		Debug.Log ("I am un der attac !");
		if (this.AI == AITypes.idle || this.AI == AITypes.returnToPos) {
			this.setAttackOn (attacker);
		}
	}
	
	void AIBehavior() {
		switch(this.AI) {
			
			case AITypes.idle:
				this.idleBehavior();
			break;
			
			case AITypes.attack:
				this.attackBehavior();
			break;
		
			
			case AITypes.returnToPos:
				this.returnToPositionBehavior();
			break;
			
		}
	}
	
	private void idleBehavior() {
		var foundUni = GameObject.Find("Universe");
		
		Universe univ = (Universe) foundUni.GetComponent(typeof(Universe));
		var univPlayers = univ.getPlayers();
		for (var i = 0 ; i < univPlayers.Length ; i++) {
			
			var remainingDistance = Vector3.Distance(univPlayers[i].transform.position, this.transform.position);
			if (remainingDistance < this.targetAttackRange) 
			{
				this.setAttackOn (univPlayers[i]);
			}
		}
	}
	
	private void setAttackOn(GameObject target) {
		Turrets turrets = (Turrets) this.GetComponent(typeof(Turrets));
		turrets.attack(target); 
		this.setAI(AITypes.attack);
	}
	
	private void attackBehavior() {
		Propulsors prop = (Propulsors) this.GetComponent(typeof(Propulsors));
		Turrets turrets = (Turrets) this.GetComponent(typeof(Turrets));
		var currentTarget = prop.getTargetPos();
		var that = this;
		if (currentTarget) {
			var remainingDistance = Vector3.Distance(currentTarget.transform.position, this.transform.position);
			if (remainingDistance >= this.targetReturnIdleRange) 
			{
				prop.setTargetPos(this.initPosition);
				this.setAI (AITypes.returnToPos);
			}
			
				/*if (this.shared.hasTarget) {
						var currentTarget = this.getSectorShip(this.shared.targetId);
						if (currentTarget) {
							var targetRange = utils.distance(currentTarget.shared, this.shared);
							if (targetRange >= this.shared.AIStopRange || !utils.isSameZ(currentTarget,this)) {
								if (server) this.setBotBehavior("backToPosition");
							}
						}
						else this.setBotBehavior("backToPosition");
					}
					else this.setAI("backToPositionTrigger");*/
		}
		else this.setAI (AITypes.returnToPos);
	}
	
	private void returnToPositionBehavior() {
		this.setAI (AITypes.idle);
	}
	
	void setAI(AITypes newAI) {
		this.AI = newAI;
	}
}
