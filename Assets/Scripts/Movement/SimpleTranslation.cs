using UnityEngine;
using System.Collections;

public class SimpleTranslation : MonoBehaviour {

	public float moveSpeed = 1f; 
	
	// Update is called once per frame
	void Update () {
		transform.Translate(-1  * Vector3.left * Time.deltaTime * this.moveSpeed); 
		//this.transform.position.x += moveSpeed; 
	}
}
