using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	public int size = 10; 
	
	public void isCollected() {
		Destroy (gameObject);
	}
	
}
