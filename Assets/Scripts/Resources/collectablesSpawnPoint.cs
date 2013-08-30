using UnityEngine;
using System.Collections;

public class collectablesSpawnPoint : Photon.MonoBehaviour {
	
	public Cooldown cooldown ; 
	public float cdTime = 5.50f; 
	public int size = 250; 
	public int collectableMax = 25; 
	public int collectableCount = 0; 
	public GameObject collectable;
	public bool isReady = false; 
	
	// Use this for initialization
	void Awake () {
        //We're connected!
		if (!PhotonNetwork.isMasterClient)
        {
			Debug.Log ("IS MASTER");
			this.enabled = true ; 
		}
		else this.enabled = false ; 
		this.cooldown = new Cooldown(this.cdTime, false);
		this.collectable = (GameObject) Resources.Load ("Prefabs/Objects/Resources/Crystal"); 
	}
	
	
	// Update is called once per frame
	void Update () {
        if (PhotonNetwork.room != null)
        {
			this.cooldown.Update(); 
			if (this.canSpawn ()) {
				this.spawnCollectable(); 
				this.cooldown.cooldownTick(); 	
			}
		}
	}
	
	public void spawnCollectable() {
		var current = gameObject; 
		this.collectableCount++; 
		/*GameObject spawned = (GameObject) Instantiate(this.collectable) ;
		spawned.name = this.collectable.name; 
		spawned.transform.parent = current.transform; 
		spawned.transform.localScale = this.randomScale (0.5f, 1.0f); 
		spawned.transform.localPosition = this.spawnPosition();*/
		
		Vector3 pos = this.spawnPosition(); 
		Quaternion rot = this.transform.rotation; 
        int id = PhotonNetwork.AllocateViewID();
		Debug.Log ("SEND RPC");
        photonView.RPC("SpawnCrystalOnNetwork", PhotonTargets.AllBuffered, "Crystal", spawnPosition(), rot, id);
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
	
	[RPC]
	void SpawnCrystalOnNetwork(string crystal, Vector3 pos, Quaternion rot, int id) {
		Debug.Log ("SPAWN CRYSTAL");
		//GameObject newCrysta = (GameObject) PhotonNetwork.InstantiateSceneObject("Prefabs/Objects/Resources/"+crystal, pos, rot, 0, null) ;
		GameObject newCrysta = PhotonNetwork.Instantiate("Prefabs/Objects/Resources/"+crystal, Vector3.zero, rot, 0) as GameObject;
		newCrysta.transform.parent = gameObject.transform;
		newCrysta.transform.localPosition = pos;   //GameObject.FindGameObjectWithTag(Phobos.Vars.OBJECTS_TAG).transform; 
		newCrysta.name = "Crystal#"+id; 
		
		var currentUniverse = GameController.findUniverse(); 
		DataManager dataScript = (DataManager) currentUniverse.GetComponent(typeof(DataManager));
		
		dataScript.SetPhotonViewIDs(newCrysta, id);
		
		dataScript.netObjects.Add(id, newCrysta.transform); 
		Debug.Log ("id " + id + "  contains " + dataScript.netObjects[id]);
	}
	
}
