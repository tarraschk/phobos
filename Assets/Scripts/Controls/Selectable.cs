using UnityEngine;
using System.Collections;


public class Selectable : MonoBehaviour {
	
public static readonly string MODEL = "Model";
	
	public const string ATTACK = "attack";
	public const string COLLECT = "collect";
	public const string DOCK = "dock";
	
public string[] availableActions = new string[3]{"attack", "collectable", "dock"}; 
	
	void Start() {
		this.availableActions = this.getObjectAction();
	}
	
	void OnMouseEnter() {
		var model = transform.FindChild(MODEL);
		model.renderer.material.color = Color.yellow;
	}
	
	void OnMouseExit() {
		var model = transform.FindChild(MODEL);
		model.renderer.material.color = Color.white;
	}
	
	void OnMouseDown() {
		//this.availableActions = new string[3]{"attack", "collectable", "dock"}; 
		this.doAction(this.availableActions[0]);
	}
	
	private void doAction(string action) {
		var target = gameObject; 
		var player = GameObject.Find("Player");
		if (player) {
			switch (action) {
				case ATTACK: 
					Turrets turr = (Turrets) player.GetComponent(typeof(Turrets));
					turr.attack(target);
				break;
					
				case COLLECT:
					Cargohold cargoPlayer = (Cargohold) player.GetComponent(typeof(Cargohold));
					cargoPlayer.addObjectAtCargo(target); 
				break;
					
				case DOCK :
				
				break;
			}
		}
	}
	
	private string[] getObjectAction() {
		string[] objectActions = {null, null, null, null, null}; //Todo : dynamic size array
		var j = 0; 
		if (this.GetComponent ("Destructible")) {
			objectActions[j] = ATTACK; 
			j++;
		}
		if (this.GetComponent("Collectable")) {
			objectActions[j] = COLLECT;	
			j++;
		}
		if (this.GetComponent("Dockable")) {
			objectActions[j] = DOCK;	
			j++;
		}
		return objectActions;
	}
}
