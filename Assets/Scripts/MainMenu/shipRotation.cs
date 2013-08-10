using UnityEngine;
using System.Collections;

public class shipRotation : MonoBehaviour {
	private float increment = 0;
	private int h = 0;
	private bool sens = true;
	private int max = 300;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var direction = 1;
		var vitesseRotationTonneau = 3;
		if(this.sens){
			if(this.increment < this.max){
				if(this.increment > this.max - 50){
					this.increment += 0.3f;
				}
				else{
					this.increment += 1;
				}
				direction = 1;
			}
			else{
				this.sens = false;
			}
		}
		if(!this.sens){
			if(this.increment >= 0){
				if(this.increment < 50){
					this.increment -= 0.3f;
				}
				else{
					this.increment -= 1;
				}
				direction = -1;
			}
			else{
				this.sens = true;
			}
		}
		this.transform.Translate(Vector3.up * Time.deltaTime * direction);
		this.transform.Rotate(Vector3.forward * Time.deltaTime * vitesseRotationTonneau * direction);
		this.transform.RotateAround (Vector3.zero, Vector3.up, 10 * Time.deltaTime);
	}
}
