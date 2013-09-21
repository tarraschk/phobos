using UnityEngine;
using System.Collections;
/**
 * This components allows the owner to craft / build stuff around him. 
 * */
public class Crafter : MonoBehaviour {
	
	public GameController gameController ; 
	public ArrayList craftMaterial ; 
	
	void Awake() {
		gameController = (GameController) GameController.getUniverseGameController(); 
	}
	
	/**
	 * Build (if we can) this building preview. 
	 * */
	public void build(Transform buildingPreview) {
		Build buildObj = (Build) buildingPreview.GetComponent(typeof(Build)); 
		buildObj.build(buildingPreview.position, buildingPreview.rotation); 
	}
	
	/**
	 * Can this current player build the building ?
	 * 
	 * Check in the cargo. To be improved. 
	 * 
	 * */
	public bool canBuild(string building) {
		bool canBuild = true ;
		bool tempResourceFound = false; 
		var costData = gameController.getRecipesManager().getRecipeData(Phobos.recType.BUILDING, building);
		Cargohold cargo = (Cargohold) this.GetComponent(typeof(Cargohold));
		this.craftMaterial = cargo.getCargoContentVar(); 
		foreach (var t in costData) {
			tempResourceFound = false ;
			System.Collections.Generic.KeyValuePair<string, SimpleJSON.JSONNode> costNode = (System.Collections.Generic.KeyValuePair<string, SimpleJSON.JSONNode>) t;
			string requireResource = costNode.Key;
			int requireCost = int.Parse(costNode.Value); 
			foreach (var materialTemp in craftMaterial) {
				Transform material = (Transform) materialTemp; 
				Collectable collect = (Collectable) material.GetComponent(typeof(Collectable));
				if (collect.name == requireResource) {
				tempResourceFound = true ; 
					if (collect.quantity < requireCost) {
						canBuild = false ; 	
					}
				}
			}
			if (!tempResourceFound)
				canBuild = false; 

		}
		return canBuild; 	
	}
	
}
