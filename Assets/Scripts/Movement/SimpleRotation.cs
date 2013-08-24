using UnityEngine;
using System.Collections;

public class SimpleRotation : MonoBehaviour {
	
	public float rotationSpeed = 1f; 
	
	void Update () {
		this.transform.Rotate(-1*Vector3.up * Time.deltaTime * this.rotationSpeed);
	}
}
