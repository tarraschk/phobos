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
	
	
	public Phobos.AITypes AI = Phobos.AITypes.idleAggressive;
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
		if (this.AI == Phobos.AITypes.idleAggressive || this.AI == Phobos.AITypes.returnToPos) {
			this.setAttackOn (attacker.transform);
		}
	}
	
	void AIBehavior() {
		switch(this.AI) {
			
			case Phobos.AITypes.idleAggressive:
				this.idleAggressiveBehavior();
			break;
			
			case Phobos.AITypes.attack:
				this.attackBehavior();
			break;
		
			
			case Phobos.AITypes.returnToPos:
				this.returnToPositionBehavior();
			break;
			
		}
	}
	
	private void idleAggressiveBehavior() {
		var univPlayers = GameController.getPlayers(); 
		for (var i = 0 ; i < univPlayers.Length ; i++) {
			
			float remainingDistance = Vector3.Distance(univPlayers[i].transform.position, this.transform.position);
			if (remainingDistance < this.targetAttackRange) 
			{
				this.setAttackOn (univPlayers[i].transform);
			}
		}
	}
	
	private void setAttackOn(Transform target) {
		ShipController SC = (ShipController) this.GetComponent(typeof(ShipController));
		if (SC.attack(target)) //If order attack is received and can be executed
			this.setAI(Phobos.AITypes.attack);
	}
	
	private void attackBehavior() {
		ShipController SC = (ShipController) this.GetComponent(typeof(ShipController));
		Propulsors prop = (Propulsors) this.GetComponent(typeof(Propulsors));
		Turrets turrets = (Turrets) this.GetComponent(typeof(Turrets));
		Transform currentTarget = turrets.getTarget();
		var botObject = this.gameObject;
		if (currentTarget.position != Vector3.zero) {
			float remainingDistance = Vector3.Distance(currentTarget.position, botObject.transform.position);
			if (remainingDistance >= this.targetReturnIdleRange) 
			{
				SC.moveTo(this.initPosition.transform.position);
				turrets.unsetTarget(); 
				this.setAI (Phobos.AITypes.returnToPos);
			}
		}
		else this.setAI (Phobos.AITypes.returnToPos);
	}
	
	private void returnToPositionBehavior() {
		this.setAI (Phobos.AITypes.idleAggressive);
	}
	
	void setAI(Phobos.AITypes newAI) {
		this.AI = newAI;
	}
}
