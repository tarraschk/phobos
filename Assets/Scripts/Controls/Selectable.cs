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
		var tooltip = transform.FindChild(Phobos.Vars.TOOLTIP);
		if (model != null && model.renderer != null) 
			model.renderer.material.color = Color.yellow;
		if (tooltip != null) {
			LabelPositioning tooltipScript = (LabelPositioning) tooltip.GetComponent(typeof(LabelPositioning));	
			tooltipScript.setTooltipToShow(); 
		}
	}
	
	void OnMouseExit() {
		var model = transform.FindChild(MODEL);
		var tooltip = transform.FindChild(Phobos.Vars.TOOLTIP);
		if (model != null && model.renderer != null) 
			model.renderer.material.color = Color.white;
		if (tooltip != null) {
			LabelPositioning tooltipScript = (LabelPositioning) tooltip.GetComponent(typeof(LabelPositioning));	
			if (!tooltipScript.locked) 
				tooltipScript.setTooltipToHide(); 
		}
	}
	
	void OnMouseDown() {
		if (Input.GetMouseButton(0)) 
		{
			string targetName = this.getTargetName();
			string targetDescr = this.getTargetDescr(); 
			GameObject GUIModelObj = (GameObject) GameController.getGUIModel(); 
			GUIModel GUIM = (GUIModel) GUIModelObj.GetComponent(typeof(GUIModel));	
			PhotonView PV = (PhotonView) this.GetComponent(typeof(PhotonView));
			GUIM.removeAllActionsButtons();
			GUIM.setTargetInfos(targetName, targetDescr); 
			GUIM.addActionButton(this.availableActions[0], PV.viewID);
			this.setItemSelected(gameObject.transform); 
			this.doLockTooltip(); 
		}
		else if (Input.GetMouseButton(1)) 
		{
			this.doAction(this.availableActions[0]);
		}
	}
	
	/**
	 * Sets a new item selected and lock the tooltip to it. 
	 **/
	public void setItemSelected(Transform newItemSelected) {
		Controls GC = GameController.getControls(); 
		//We first disable the tooltip on the current selected item
		this.unlockPreviousTooltip(); 
		
		this.doLockTooltip(); 
		GC.itemSelected = newItemSelected; 	
	}
	
	private void unlockPreviousTooltip() {
		Controls GC = GameController.getControls(); 
		Transform previousItemSelected = GC.itemSelected;
		if (previousItemSelected != null) {
			Selectable PISelect = (Selectable) previousItemSelected.GetComponent(typeof(Selectable));
			PISelect.doUnlockTooltip();
		}
	}
	
	private string getTargetName() {
		PlayerStats PS = (PlayerStats) this.GetComponent(typeof(PlayerStats));
		if (PS != null)
			return PS.nick; 
		ObjectStats OS = (ObjectStats) this.GetComponent(typeof(ObjectStats));
		if (OS != null)
			return OS.name; 
		return "Unnamed"; 
	}
	
	private string getTargetDescr() {
		PlayerStats PS = (PlayerStats) this.GetComponent(typeof(PlayerStats));
		if (PS != null)
			return "Level " + PS.level; 
		ObjectStats OS = (ObjectStats) this.GetComponent(typeof(ObjectStats));
		if (OS != null)
			return OS.descr; 
		
		return "-"; 
	}
	
	/**
	 * Lock the tooltip when item is selected ! 
	 **/
	private void doLockTooltip() {
		Component tooltipGet = GetComponentInChildren(typeof(LabelPositioning)); 
		LabelPositioning tooltipScript = (LabelPositioning) tooltipGet; 
		tooltipScript.lockTooltip(); 
		tooltipScript.setTooltipToShow(); 
	}
	
	private void doUnlockTooltip() {
		Component tooltipGet = GetComponentInChildren(typeof(LabelPositioning)); 
		LabelPositioning tooltipScript = (LabelPositioning) tooltipGet; 
		tooltipScript.unlockTooltip(); 
		tooltipScript.setTooltipToHide(); 
	}
	
	private void doAction(string action) {
		var univ = GameController.findUniverse(); 
		GameController univScript = (GameController) univ.GetComponent(typeof(GameController));
		var target = gameObject; //The target is the current selected object
		var player = univScript.getPlayer(); // TODO 
		Controls shipControls = (Controls) GameController.getControls(); 
		ShipController shipController = (ShipController) player.GetComponent(typeof(ShipController));
		if (player) {
			switch (action) {
				case ATTACK: 
					shipControls.attackTarget(target.transform); 
				break;
					
				case COLLECT:
					shipControls.collectTarget(target.transform); 
				break;
					
				case DOCK :
					shipControls.dockTo(target.transform); 
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
