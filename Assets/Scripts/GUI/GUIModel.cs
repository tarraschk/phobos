using UnityEngine;
using System.Collections;
namespace PhobosGUI {
public class ActionButton : EZData.Context 
{
	public GUIModel LinkToGame ; 
	
	#region Property TargetID
	private readonly EZData.Property<int> _privateTargetIDProperty = new EZData.Property<int>();
	public EZData.Property<int> TargetIDProperty { get { return _privateTargetIDProperty; } }
	public int TargetID
	{
	get    { return TargetIDProperty.GetValue();    }
	set    { TargetIDProperty.SetValue(value); }
	}
	#endregion
	
	#region Property ActionName
	private readonly EZData.Property<string> _privateActionNameProperty = new EZData.Property<string>();
	public EZData.Property<string> ActionNameProperty { get { return _privateActionNameProperty; } }
	public string ActionName
	{
	get    { return ActionNameProperty.GetValue();    }
	set    { ActionNameProperty.SetValue(value); }
	}
	#endregion
	
	public void actionActivate() {
		Debug.Log ("do action");
		Debug.Log (this.ActionName); 
		this.LinkToGame.doAction(this.ActionName, this.TargetID); 	
	}
}

public class Cargocontent : EZData.Context 
{
	public GUIModel LinkToGame ; 
	
	#region Property ContentName
	private readonly EZData.Property<string> _privateContentNameProperty = new EZData.Property<string>();
	public EZData.Property<string> ContentNameProperty { get { return _privateContentNameProperty; } }
	public string ContentName
	{
	get    { return ContentNameProperty.GetValue();    }
	set    { ContentNameProperty.SetValue(value); }
	}
	#endregion
	
	#region Property ContentQuantity
	private readonly EZData.Property<int> _privateContentQuantityProperty = new EZData.Property<int>();
	public EZData.Property<int> ContentQuantityProperty { get { return _privateContentQuantityProperty; } }
	public int ContentQuantity
	{
	get    { return ContentQuantityProperty.GetValue();    }
	set    { ContentQuantityProperty.SetValue(value); }
	}
	#endregion
		
	#region Property ContentID
	private readonly EZData.Property<int> _privateContentIDProperty = new EZData.Property<int>();
	public EZData.Property<int> ContentIDProperty { get { return _privateContentIDProperty; } }
	public int ContentID
	{
	get    { return ContentIDProperty.GetValue();    }
	set    { ContentIDProperty.SetValue(value); }
	}
	#endregion
		
	#region Property ContentTexture
	private readonly EZData.Property<Texture> _privateContentTextureProperty = new EZData.Property<Texture>();
	public EZData.Property<Texture> ContentTextureProperty { get { return _privateContentTextureProperty; } }
	public Texture ContentTexture
	{
	get    { return ContentTextureProperty.GetValue();    }
	set    { ContentTextureProperty.SetValue(value); }
	}
	#endregion
		
	#region Property ContentSpritePath
	private readonly EZData.Property<string> _privateContentSpritePathProperty = new EZData.Property<string>();
	public EZData.Property<string> ContentSpritePathProperty { get { return _privateContentSpritePathProperty; } }
	public string ContentSpritePath
	{
	get    { return ContentSpritePathProperty.GetValue();    }
	set    { ContentSpritePathProperty.SetValue(value); }
	}
	#endregion
		
	
}


public class UI : EZData.Context
{
	#region Property Name
	private readonly EZData.Property<string> _privateNameProperty = new EZData.Property<string>();
	public EZData.Property<string> NameProperty { get { return _privateNameProperty; } }
	public string Name
	{
	get    { return NameProperty.GetValue();    }
	set    { NameProperty.SetValue(value); }
	}
	#endregion
	
	#region Property Energy
	private readonly EZData.Property<int> _privateEnergyProperty = new EZData.Property<int>();
	public EZData.Property<int> EnergyProperty { get { return _privateEnergyProperty; } }
	public int Energy
	{
	get    { return EnergyProperty.GetValue();    }
	set    { EnergyProperty.SetValue(value); }
	}
	#endregion
	
	#region Property Money
	private readonly EZData.Property<int> _privateMoneyProperty = new EZData.Property<int>();
	public EZData.Property<int> MoneyProperty { get { return _privateMoneyProperty; } }
	public int Money
	{
	get    { return MoneyProperty.GetValue();    }
	set    { MoneyProperty.SetValue(value); }
	}
	#endregion
	
	#region Property Level
	private readonly EZData.Property<int> _privateLevelProperty = new EZData.Property<int>();
	public EZData.Property<int> LevelProperty { get { return _privateLevelProperty; } }
	public int Level
	{
	get    { return LevelProperty.GetValue();    }
	set    { LevelProperty.SetValue(value); }
	}
	#endregion
	
	#region Property XP
	private readonly EZData.Property<int> _privateXPProperty = new EZData.Property<int>();
	public EZData.Property<int> XPProperty { get { return _privateXPProperty; } }
	public int XP
	{
	get    { return XPProperty.GetValue();    }
	set    { XPProperty.SetValue(value); }
	}
	#endregion
	
	#region Property RequiredXP
	private readonly EZData.Property<int> _privateRequiredXPProperty = new EZData.Property<int>();
	public EZData.Property<int> RequiredXPProperty { get { return _privateRequiredXPProperty; } }
	public int RequiredXP
	{
	get    { return RequiredXPProperty.GetValue();    }
	set    { RequiredXPProperty.SetValue(value); }
	}
	#endregion
	
	#region Property PreviousRequiredXP
	private readonly EZData.Property<int> _privatePreviousRequiredXPProperty = new EZData.Property<int>();
	public EZData.Property<int> PreviousRequiredXPProperty { get { return _privatePreviousRequiredXPProperty; } }
	public int PreviousRequiredXP
	{
	get    { return PreviousRequiredXPProperty.GetValue();    }
	set    { PreviousRequiredXPProperty.SetValue(value); }
	}
	#endregion
	
	#region Property Oxygen
	private readonly EZData.Property<int> _privateOxygenProperty = new EZData.Property<int>();
	public EZData.Property<int> OxygenProperty { get { return _privateOxygenProperty; } }
	public int Oxygen
	{
	get    { return OxygenProperty.GetValue();    }
	set    { OxygenProperty.SetValue(value); }
	}
	#endregion
	
	#region Property OxygenMin
	private readonly EZData.Property<float> _privateOxygenMinProperty = new EZData.Property<float>();
	public EZData.Property<float> OxygenMinProperty { get { return _privateOxygenMinProperty; } }
	public float OxygenMin
	{
	get    { return OxygenMinProperty.GetValue();    }
	set    { OxygenMinProperty.SetValue(value); }
	}
	#endregion
	
	#region Property OxygenMax
	private readonly EZData.Property<float> _privateOxygenMaxProperty = new EZData.Property<float>();
	public EZData.Property<float> OxygenMaxProperty { get { return _privateOxygenMaxProperty; } }
	public float OxygenMax
	{
	get    { return OxygenMaxProperty.GetValue();    }
	set    { OxygenMaxProperty.SetValue(value); }
	}
	#endregion
	
	#region Property ActionsMenu
	private readonly EZData.Collection<ActionButton> _privateActionsMenu = new EZData.Collection<ActionButton>(false);
	public EZData.Collection<ActionButton> ActionsMenu { get { return _privateActionsMenu; } }
	#endregion
	
	#region Property TargetName
	private readonly EZData.Property<string> _privateTargetNameProperty = new EZData.Property<string>();
	public EZData.Property<string> TargetNameProperty { get { return _privateTargetNameProperty; } }
	public string TargetName
	{
	get    { return TargetNameProperty.GetValue();    }
	set    { TargetNameProperty.SetValue(value); }
	}
	#endregion
	
	#region Property TargetDescr
	private readonly EZData.Property<string> _privateTargetDescrProperty = new EZData.Property<string>();
	public EZData.Property<string> TargetDescrProperty { get { return _privateTargetDescrProperty; } }
	public string TargetDescr
	{
	get    { return TargetDescrProperty.GetValue();    }
	set    { TargetDescrProperty.SetValue(value); }
	}
	#endregion
	
	#region Property Cargohold
	private readonly EZData.Collection<Cargocontent> _privateCargohold = new EZData.Collection<Cargocontent>(false);
	public EZData.Collection<Cargocontent> Cargohold { get { return _privateCargohold; } }
	#endregion
}
	
}
public class GUIModel : MonoBehaviour
{
	public const string ATTACK = "attack";
	public const string COLLECT = "collect";
	public const string DOCK = "dock";
	public Transform GUITarget ; 
	public bool active = false ; 
	
	public GameController gameController ; 
	public Destructible TargetDestr ; 
	public Crafter TargetCraft ; 
	public Cargohold TargetCargo ; 
	public PlayerStats TargetStats ; 
	public ShipController TargetController ; 
	public OxygenTank TargetOxygen ; 
	public Controls playerControls ; 
	
	public Texture inventorySlot ; 
	
	public bool showBuildMenu = false ; 
	public bool showDockMenu = false ; 
	
	public NguiRootContext View;
	public PhobosGUI.UI Context;
	
	void Awake()
	{
		Context = new PhobosGUI.UI();
		View.SetContext(Context);
		this.active = false; 
	}
	
	void Update() {
		if (this.active)
			this.GUIUpdate(); 
	}
	
	
	
	public void setGUITarget(Transform newPlayer) {
		this.GUITarget = newPlayer;
		gameController = (GameController) GameController.getUniverseGameController(); 
		playerControls = (Controls) GameController.getControls(); 
		TargetCraft = (Crafter) GameController.getControls().getPlayer().GetComponent(typeof(Crafter)); 
		TargetController = (ShipController) this.GUITarget.GetComponent(typeof(ShipController));
		TargetOxygen = (OxygenTank) this.GUITarget.GetComponent(typeof(OxygenTank));
		TargetDestr = (Destructible) this.GUITarget.GetComponent(typeof(Destructible));
		TargetCargo = (Cargohold) this.GUITarget.GetComponent(typeof(Cargohold));	 
		TargetStats = (PlayerStats) this.GUITarget.GetComponent(typeof(PlayerStats));
		this.bootStats() ;
	}
	
	public void setActive(bool newActive) {
		this.active = newActive ; 
	}
	
	private void bootStats() {
		this.setName(TargetStats.nick); 	
		this.setEnergy(TargetDestr.energy); 	
	}
	
	private void GUIUpdate() {	
		this.setEnergy(TargetDestr.energy); 
		this.setMoney(TargetStats.money); 
		this.setOxygen(TargetOxygen.oxygenLevel); 
		this.setOxygenMin(TargetOxygen.oxygenMin); 
		this.setOxygenMax(TargetOxygen.oxygenMax); 
		this.setXP(TargetStats.currentXP); 
		this.setPreviousRequiredXP(TargetStats.previousRequiredXP); 
		this.setRequiredXP(TargetStats.requiredXP); 
		this.setLevel(TargetStats.level); 
	}
	
	public void setName(string newName) {
		Context.Name = newName; 	
	}
	
	public void setEnergy(int newEnergy) {
		Context.Energy = newEnergy; 	
	}
	
	public void setMoney(int newMoney) {
		Context.Money = newMoney; 	
	}
	
	public void setLevel(int newLevel) {
		Context.Level = newLevel; 	
	}
	
	public void setOxygen(float newOxygen) {
		Context.Oxygen = (int) newOxygen; 	
	}
	
	public void setOxygenMin(float newOxygenMin) {
		Context.OxygenMin = (int) newOxygenMin; 	
	}
	
	public void setOxygenMax(float newOxygenMax) {
		Context.OxygenMax = (int) newOxygenMax; 	
	}
	
	public void setXP(int newXP) {
		Context.XP = newXP; 	
	}
	
	public void setPreviousRequiredXP(int newPreviousRequiredXP) {
		Context.PreviousRequiredXP = newPreviousRequiredXP; 	
	}
	
	public void setRequiredXP(int newRequiredXP) {
		Context.RequiredXP = newRequiredXP; 	
	}
	
	public void addActionButton(string action, int targetId) {
		Context.ActionsMenu.Add(new PhobosGUI.ActionButton{ TargetID=targetId, ActionName = action, LinkToGame = this } );
	}
	
	/**
	 * Add a cargo in the GUI of the cargo
	 **/
	public void addCargoContent(string name, Texture icon, int quantity, int id) {	
		PhobosGUI.Cargocontent alreadyCargoContent = this.getGUICargoContentAlreadyExists(id); 
		if (alreadyCargoContent != null) {
			alreadyCargoContent.ContentQuantity += quantity ; 	
		}
		else {
			Context.Cargohold.Add(new PhobosGUI.Cargocontent{ ContentID=id, ContentName=name, ContentQuantity=quantity, ContentTexture=icon, ContentSpritePath=name }) ;	
		}
	}
	
	/**
	 * Remove cargo, affect the GUI. If quantity is below or equal to zero, we destroy the prefab. 
	 * */
	public void removeCargoContent(int id, int quantity) {
		PhobosGUI.Cargocontent toRemoveContent = this.getGUICargoContentAlreadyExists(id); 
		if ((toRemoveContent.ContentQuantity - quantity) <= 0) {
			int toRemoveArrayID = this.getGUICargoArrayIndex(id); 
			Context.Cargohold.Remove(toRemoveArrayID); 
		}
		else {
			toRemoveContent.ContentQuantity = toRemoveContent.ContentID - quantity ; 
		}
	}
	
	/*
	 * Returns the array index of a GUI element, identified by it's ID.
	 * Return -1 if it wasn't found.
	 **/
	private int getGUICargoArrayIndex(int id) {
		for (var i = 0 ; i < Context.Cargohold.ItemsCount ; i++) {
			if (Context.Cargohold.GetItem(i).ContentID == id) {
				return i ; 
			}	
		}
		return -1 ; 
	}
	
	/**
	 * Check if in the cargo GUI, we already have this item, 
	 * based on the item ID. If we do, return it.
	 * If not, return null */
	private PhobosGUI.Cargocontent getGUICargoContentAlreadyExists(int id) {
		for (var i = 0 ; i < Context.Cargohold.ItemsCount ; i++) {
			if (Context.Cargohold.GetItem(i).ContentID == id) {
				return Context.Cargohold.GetItem(i); 
			}
		}
		return null ;
	}
	
	public void removeAllActionsButtons() {
		Context.ActionsMenu.Clear();	
	}
	
	public void removeActionButton(int targetID) {
		Context.ActionsMenu.Remove (targetID); 
	}
	
	public void setTargetInfos(string targetName, string targetDescr) {
		this.clearTargetInfos(); 
		Context.TargetName = targetName; 
		Context.TargetDescr = targetDescr; 
	}
	
	private void clearTargetInfos() {
		Context.TargetName = string.Empty; 
		Context.TargetDescr = string.Empty; 
	}
	
	public void doAction(string action, int targetID) {
		var univ = GameController.findUniverse(); 
		GameController univScript = (GameController) univ.GetComponent(typeof(GameController));
		DataManager DM = (DataManager) univ.GetComponent(typeof(DataManager));
		Debug.Log (targetID);
		Transform target = DM.getPlayerOrObject(targetID); //The target is the current selected object
		Debug.Log (target);
		
		var player = univScript.getPlayer(); // TODO 
		Controls shipControls = (Controls) GameController.getControls(); 
		ShipController shipController = (ShipController) player.GetComponent(typeof(ShipController));
		Debug.Log (shipControls);
		if (player) {
			switch (action) {
				case ATTACK: 
					shipControls.attackTarget(target); 
				break;
					
				case COLLECT:
					shipControls.collectTarget(target); 
				break;
					
				case DOCK :
					shipControls.dockTo(target); 
				break;
			}
		}
	}
}
