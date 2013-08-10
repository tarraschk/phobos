using UnityEngine;
using System.Collections;

public class projectile : MonoBehaviour {
	public Transform target = null;
	private Vector3 origin = new Vector3(0,0,0);
	public int speed = 1000; 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate(Vector3.right * Time.deltaTime * this.speed);
		if(this.transform.position.x > 300){
			this.transform.position = this.origin;
		}
	}
}
