using UnityEngine;
using System.Collections;

public class SimpleRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(-1*Vector3.up * Time.deltaTime * 1);
	}
}
