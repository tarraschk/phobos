using UnityEngine;
using System.Collections;
using System.IO; 
using SimpleJSON; 

public class RecipesImport : MonoBehaviour {
	
	//public string recipesData ; //Interpreted data, this is the most important attribute : contains all recipes. 
	
	public string recipesLocation = "C:/Users/Public/Documents/Unity Projects/Phobos/Assets/Resources/Crafting/Recipes/phoboscrafts.json"; //Location of the recipes file. 
	
	private string recipeText ; //Stocks the recipe raw text data, to be interpreted as JSON. 
	
	/**
	 * Constructor, executed at loading section. 
	 */
	public RecipesImport() {
		Debug.Log ("Instantiate");
		this.readRecipeJSON(); 
		//string JSONData = (string) Resources.Load(this.recipesLocation);
		var recipesData = JSON.Parse (this.recipeText); 
		Debug.Log(recipesData); 
		string test = (recipesData["recipes"]["buildings"]["cruiser"]["crystal"]);
		Debug.Log (test);
	}
	
	/**
	 * Read and parse the recipe file. It is located here, but soon will be located online. 
	 * File must be JSON. 
	 */
	public void readRecipeJSON() {
		var SR = new StreamReader(this.recipesLocation); 
		this.recipeText = SR.ReadToEnd(); 
		Debug.Log(this.recipeText); 
		SR.Close();
	}
}
