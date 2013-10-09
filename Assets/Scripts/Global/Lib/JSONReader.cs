using UnityEngine;
using System.Collections;
using System.IO; 
using SimpleJSON; 

namespace Phobos
{
	public class JSONReader : MonoBehaviour {
	
		public SimpleJSON.JSONClass readAndParseJSON(string location) {
			var dataContent = this.readJSON(location); 
			return((SimpleJSON.JSONClass) JSON.Parse (dataContent));	
		}
		
		/**
		 * Read and parse the recipe file. It is located here, but soon will be located online. 
		 * File must be JSON. 
		 */
		public string readJSON(string location) {
			string content ="" ; 
			var SR = new StreamReader(location); 
			content = SR.ReadToEnd(); 
			SR.Close();
			return content; 
		}
	}
}
