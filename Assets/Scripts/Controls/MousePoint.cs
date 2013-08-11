using UnityEngine;
using System.Collections;

public class MousePoint : MonoBehaviour {

	RaycastHit hit;
	
	private float raycastLength = 5000; 
	
	public int  damping = 6;
	public GameObject mouseTarget; 
	
	void Update () {
		
		GameObject Target = GameObject.Find ("Target");
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
		if (Physics.Raycast(ray, out hit, raycastLength)) {
			if (hit.collider.name == "TerrainMain")
			{
				if (Input.GetMouseButton(1) || Input.GetMouseButton(0)) 
				{
					GameObject TargetObj = Instantiate(mouseTarget, hit.point, Quaternion.identity) as GameObject; 
					TargetObj.name = "targetInstanciated";
					TargetObj.transform.parent = GameObject.Find ("EmptyObjects").transform; 
				    var target = GameObject.Find("Player");
					if (target) {
						Propulsors prop = (Propulsors) target.GetComponent(typeof(Propulsors));
						prop.setTargetPos(TargetObj.transform);
					}
					
				    /*target.transform.LookAt(TargetObj.transform);
					
					
					var rotation = Quaternion.LookRotation(TargetObj.transform.position - target.transform.position);
					target.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * damping);*/
					
				}
			}
		}
		Debug.DrawRay(ray.origin, ray.direction * raycastLength, Color.yellow);
	}
}
