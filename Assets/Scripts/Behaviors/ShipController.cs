using UnityEngine;
using System.Collections;

//Controls a ship object, which means
//- Propulsors
//- Turrets
//- Cargohold (?) 

public class ShipController : MonoBehaviour {
	
	public GameObject target ; 
	public BehaviorTypes behavior = BehaviorTypes.idle; 
	void Update () {
		switch (this.behavior) {
			case BehaviorTypes.idle: 
			
			break;
			case BehaviorTypes.collecting: 
				this.collectBehavior();
			break;
			case BehaviorTypes.moving:
			
			break; 
		}
	}
	
	private void collectBehavior() {
		GameObject currentShip = gameObject; 
		Transform cargoBay = transform.FindChild(Phobos.Vars.CARGOBAY); 
		if (this.target != null) {
			
			var remainingDistance = Vector3.Distance(this.target.transform.position, this.transform.position);
			if (remainingDistance < Phobos.Vars.COLLECT_DISTANCE) {
				Cargohold cargoPlayer = (Cargohold) this.GetComponent(typeof(Cargohold));
				Collectable collectableTarg = (Collectable) target.GetComponent(typeof(Collectable));
				if (cargoPlayer.addObjectAtCargo(target)) {
					collectableTarg.isCollected(cargoBay); 
					this.setBehavior (BehaviorTypes.idle);
					this.unsetTarget(); 
				}
			}
		}
		else {
			this.setBehavior (BehaviorTypes.idle);	
		}
	}
	
	public void setTarget(GameObject newTarget) {
		this.target =  newTarget; 
	}
	
	public void unsetTarget() {
		this.target = null; 
	}
	
	public void setBehavior(BehaviorTypes newBehavior) {
		switch (newBehavior) {
			case BehaviorTypes.idle:
				Propulsors prop = (Propulsors) this.GetComponent(typeof(Propulsors));
				prop.stop ();
			break; 
		}
		this.behavior = newBehavior; 
	}
	
}