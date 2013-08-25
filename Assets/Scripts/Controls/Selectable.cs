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
		if (model != null) 
			model.renderer.material.color = Color.yellow;
	}
	
	void OnMouseExit() {
		var model = transform.FindChild(MODEL);
		if (model != null) 
			model.renderer.material.color = Color.white;
	}
	
	void OnMouseDown() {
		//this.availableActions = new string[3]{"attack", "collectable", "dock"}; 
		this.doAction(this.availableActions[0]);
	}
	
	private void doAction(string action) {
		var univ = GameController.findUniverse(); 
		GameController univScript = (GameController) univ.GetComponent(typeof(GameController));
		var target = gameObject; //The target is the current selected object
		var player = univScript.getPlayer(); // TODO 
		ShipController shipController = (ShipController) player.GetComponent(typeof(ShipController));
		if (player) {
			switch (action) {
				case ATTACK: 
					shipController.attackOwn(target.transform); 
				break;
					
				case COLLECT:
					shipController.collectOwn(target.transform); 
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
