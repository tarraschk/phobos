
using UnityEngine;
using System.Collections;

public class Cooldown {
	public float cooldown = .25f;
	public bool ready = true;
	private float currentCooldown ;
	private float lastCooldown ; 
	public Cooldown (float cooldown, bool isReady) {
		this.cooldown = cooldown;
		this.ready = isReady; 
	}

	public void Update() {
		this.cooldownUpdate(); 
	}

	void cooldownUpdate() {
		this.currentCooldown = Time.time;
		if (!this.isReady()) {
			if ((this.currentCooldown - this.lastCooldown) >= cooldown) {
					this.setReady(true);	
				}
			}
	}
	
	public void cooldownTick() {
		this.setReady (false);
		this.lastCooldown = Time.time;
	}
	
	
	public void setReady(bool newReady) {
		this.ready = newReady;	
	}
	
	public bool isReady() {
		return this.ready;	
	}	
}

