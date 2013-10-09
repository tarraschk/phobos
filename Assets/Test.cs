using UnityEngine;

[RequireComponent(typeof(HUDText))]
public class Test : MonoBehaviour {

	void Update () {
		HUDText ht = GetComponent<HUDText>(); 
		ht.Add(Time.deltaTime * 10f, Color.white, 0.5f); 
	}
}
