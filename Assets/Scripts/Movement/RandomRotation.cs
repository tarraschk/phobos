using UnityEngine;
using System.Collections;

public class RandomRotation : MonoBehaviour {
	
	public int rotationSpeed = 50; 
	
	void Update () {
        transform.Rotate(Vector3.right * Time.deltaTime * this.rotationSpeed);
        transform.Rotate(Vector3.up * Time.deltaTime * this.rotationSpeed, Space.World);
	}
}
