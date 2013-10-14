using UnityEngine;
using System.Collections;

public class OxygenTank : MonoBehaviour {
	
	public float oxygenLevel = 1000 ;
	public float oxygenMin = 0 ; 
	public float oxygenMax = 1000 ; 
	
	void Update () {
		if (this.sectorIsExplored()) {
			this.enabled = false ; 	
		}
		else {
			this.consumeOxygen(); 	
		}
	}
	
	private void consumeOxygen() {
		this.oxygenLevel -= 0.1f;	
	}
	
	public bool sectorIsExplored() {
		return (GameController.getSectorData().isExplored);	
	}
	
	public void restoreOxygen() {
		this.oxygenLevel = this.oxygenMax ; 	
	}
}
