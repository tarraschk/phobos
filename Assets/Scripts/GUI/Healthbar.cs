using UnityEngine;
using System.Collections;

public class Healthbar : MonoBehaviour {
	
	public GUITexture healthBar ; 
	public float healthBarWMax ; 
	public float healthBarHMax ;
	private Destructible destrComponent ;
	// Use this for initialization
	void Awake () {
	
		this.healthBar = (GUITexture) this.GetComponent(typeof(GUITexture));	
		this.destrComponent = (Destructible) transform.parent.GetComponent(typeof(Destructible));	
		if (this.healthBar != null) {
			this.healthBarWMax = this.healthBar.pixelInset.width; 	
			this.healthBarHMax = this.healthBar.pixelInset.height; 	
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (this.healthBar != null) {
			this.updateHealthBar(); 	
		}
	}
	
	private void updateHealthBar() {
		float healthNewWidth = ((float) ((float)this.destrComponent.energy / (float) this.destrComponent.energyMax)) * this.healthBarWMax; 
		this.healthBar.pixelInset = new Rect(this.healthBar.pixelInset.x,this.healthBar.pixelInset.y, healthNewWidth,this.healthBar.pixelInset.height);
			
		this.healthBar.pixelInset.Set(-500f,-4654, this.destrComponent.energy, this.healthBar.pixelInset.height); 
	}
}
