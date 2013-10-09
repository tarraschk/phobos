using UnityEngine;
using System.Collections;
using System.IO; 
using SimpleJSON; 

public class GameplayData : MonoBehaviour {

	public SimpleJSON.JSONNode gameplayData ; //Interpreted and parsed data with jsonclass, this is the most important attribute : contains all gameplaydata. 
	
	public string gameplayLocation = "C:/Users/Public/Documents/Unity Projects/Phobos/Assets/Resources/GameData/Global/Gameplay.json"; //Location of the recipes file. 
	private string gameplayText ; //Stocks the raw text data, to be interpreted as JSON. 
	
	/**
	 * Constructor, executed at loading section. 
	 */
	public GameplayData() {
		this.readGameplayJSON(); 
		Debug.Log (this.gameplayText);
		
		this.gameplayData = JSON.Parse (this.gameplayText); 
	}
	
	/**
	 * Read and parse the recipe file. It is located here, but soon will be located online. 
	 * File must be JSON. 
	 */
	public void readGameplayJSON() {
		var SR = new StreamReader(this.gameplayLocation); 
		this.gameplayText = SR.ReadToEnd(); 
		SR.Close();
	}
	
	public JSONNode getEquipments() {
		return this.gameplayData["equipment"]; 	
	}
	
	public JSONNode getTurrets() {
		var turretsParent = this.getEquipments(); 
		return turretsParent["turrets"] ; 
	}
	
	public JSONNode getTurret(string turretName) {
		var turretParent = this.getTurrets();
		if (turretParent[turretName] != null) {
			return turretParent[turretName];
		}
		else
			return null ; 
	}
	
	public string getStringNodeProprety(string proprety, JSONNode node) {
		Debug.Log (node); 
		if (node[proprety] != null) 
			return ((string) node[proprety]); 
		else return null ; 
	}
	
	public int getIntNodeProprety (string proprety, JSONNode node) {
		string temp = this.getStringNodeProprety(proprety, node); 	
		if (temp != null) 
			return int.Parse(temp); 
		else return 0; 
	}
	
	public float getFloatNodeProprety(string proprety, JSONNode node) {
		string temp = this.getStringNodeProprety(proprety, node); 
		if (temp != null) 
			return float.Parse(temp); 
		else return 0; 
	}
}
