using UnityEngine;
using System.Collections;

public class PlayerStats : Photon.MonoBehaviour {
	
	public string nick = "Kevin"; 
	public int money ;
	public int level ;
	public int currentXP ; 
	public int requiredXP ; 
	public int previousRequiredXP; 
	
	void Awake() {
	
		this.level = 1 ; 
		this.money = 500 ; 
		this.currentXP = 0 ;
		this.requiredXP = 200 ; 
		this.previousRequiredXP = 0 ; 
	}
	
	public void doGainXP(int GainXP) {
		int newXP = this.currentXP + GainXP ; 
		while (this.currentXP + GainXP >= this.requiredXP) {
			this.doLevelUp(); 
		}
		
		this.currentXP = newXP ; 
	}
	
	public void doGainMoney(int GainMoney) {
		this.money += GainMoney  ; 
	}
	
	public void doLevelUp() {
		this.level += 1 ; 
		this.previousRequiredXP = this.requiredXP ; 
		this.requiredXP += 50 ; 
	}
}