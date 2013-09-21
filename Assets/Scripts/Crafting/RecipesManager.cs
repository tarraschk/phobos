using UnityEngine;
using System.Collections;
using SimpleJSON; 

public class RecipesManager : MonoBehaviour {
	
	RecipesImport recipesData ; 
	
	public RecipesManager() {
		this.recipesData = new RecipesImport(); 
	}
	
	public SimpleJSON.JSONClass getRecipeData(string recType, string recName) {
		return this.recipesData.getRecipe(recType, recName);  	
	}
	
}
