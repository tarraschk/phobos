using UnityEngine;
using System.Collections;

public class collectablesSpawnPoint : MonoBehaviour {
	
	public Cooldown cooldown ; 
	public float cdTime = 1.50f; 
	public int size = 250; 
	public int collectableMax = 25; 
	public int collectableCount = 0; 
	public GameObject collectable;
	public bool isReady = false; 
	
	// Use this for initialization
	void Start () {
		this.cooldown = new Cooldown(this.cdTime, false);
		this.collectable = (GameObject) Resources.Load ("Prefabs/Objects/Resources/Crystal"); 
	}
	
	
	// Update is called once per frame
	void Update () {
		this.cooldown.Update(); 
		if (this.canSpawn ()) {
			this.spawnCollectable(); 
			this.cooldown.cooldownTick(); 	
		}
	}
	
	public void spawnCollectable() {
		var current = gameObject; 
		this.collectableCount++; 
		GameObject spawned = (GameObject) Instantiate(this.collectable) ;
		spawned.name = this.collectable.name; 
		spawned.transform.parent = current.transform; 
		spawned.transform.localScale = this.randomScale (0.5f, 1.0f); 
		spawned.transform.localPosition = this.spawnPosition();
	}
	
	public Vector3 randomScale(float min, float max) {
		float result = Random.Range(min, max); 
		return new Vector3(result, result, result); 
	}
	
	public Vector3 spawnPosition() {
		float minX = 0 - this.size; 
		float minY = 0 - this.size; 
		float maxX = 0 + this.size; 
		float maxY = 0 + this.size; 
		return new Vector3(Random.Range(minX, maxX), 0, Random.Range(minY, maxY));
	}
	
	public bool canSpawn() {
		return (this.cooldown.isReady() && this.collectableCount < this.collectableMax); 	
	}
}
