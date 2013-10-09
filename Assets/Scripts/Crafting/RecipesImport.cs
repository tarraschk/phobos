using UnityEngine;
using System.Collections;
using System.IO; 
using SimpleJSON; 

public class RecipesImport : MonoBehaviour {
	
	public SimpleJSON.JSONClass recipesData ; //Interpreted data, this is the most important attribute : contains all recipes. 
	
	public string recipesLocation = "C:/Users/Public/Documents/Unity Projects/Phobos/Assets/Resources/GameData/Crafting/Recipes/phoboscrafts.json"; //Location of the recipes file. 
	
	private string recipeText ; //Stocks the recipe raw text data, to be interpreted as JSON. 
	
	/**
	 * Constructor, executed at loading section. 
	 */
	public RecipesImport() {
		this.readRecipeJSON(); 
		//string JSONData = (string) Resources.Load(this.recipesLocation);
		var recipesData2 = JSON.Parse (this.recipeText); 
		string test = (recipesData2["recipes"]["buildings"]["cruiser"]["crystal"]);
		this.recipesData = (SimpleJSON.JSONClass) recipesData2; 
	}
	
	/**
	 * Read and parse the recipe file. It is located here, but soon will be located online. 
	 * File must be JSON. 
	 */
	public void readRecipeJSON() {
		var SR = new StreamReader(this.recipesLocation); 
		this.recipeText = SR.ReadToEnd(); 
		SR.Close();
	}
	
	/**
	 * Get a recype data, based on the recype's type and name
	 * Data must have been loaded before. 
	 * Returns null if nothing is found
	 */
	public SimpleJSON.JSONClass getRecipe(string recType, string recName) {
		if (this.recipesData[Phobos.Vars.RECIPES][recType][recName] != null) 
			return (SimpleJSON.JSONClass) this.recipesData[Phobos.Vars.RECIPES][recType][recName]; 
		else return null; 
	}
}
