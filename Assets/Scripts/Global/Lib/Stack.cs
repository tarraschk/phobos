using UnityEngine;
using System.Collections;

namespace Phobos
{
	public class Stack : MonoBehaviour
	{
		public ArrayList inputStack = new ArrayList() ; 
		
		public void addEntry(dataEntry entry) 
		{
			inputStack.Add (entry); 
		}
		
		public dataEntry shiftEntry() {
			dataEntry firstEntry = (dataEntry) inputStack[0]; 
			inputStack.RemoveAt(0);  
			return firstEntry; 
		}
	}
	
	public class dataEntry 
	{
		public string dataCommand ; 
		public Transform dataTransform ; 
		public Vector3 dataVector ;
		
		public dataEntry(string newCommand, Transform newTransform, Vector3 newVector) {
			this.dataCommand = newCommand ; 
			this.dataTransform = newTransform;
			this.dataVector = newVector; 
		}
	}
}

