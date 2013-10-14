using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager : Photon.MonoBehaviour
{
    class PlayerInfo4
    {
        public PhotonPlayer networkPlayer;        
        public Transform transform;

        public bool IsLocal()
        {
            return networkPlayer.isLocal;
        }
    }

    public static DataManager DM;
	public DBHandler DBH ; 

    public Transform playerPrefab;
    public Transform botPrefab;
	public Hashtable netObjects = new Hashtable();

    private List<PlayerInfo4> playerList = new List<PlayerInfo4>(); 
    private PlayerInfo4 localPlayerInfo;
	
	/**
	 * Spawn data manager
	 * 
	 */
    void Awake()
    {
        DM = this;
    }
	
	/**
	 * WHEN JOINED ROOM : 
	 * Spawn a local player, we own it
	 */
    
    void OnJoinedRoom()
    {
		if (PhotonNetwork.isMasterClient) {
			//We have to spawn the local data in the scene. 
			SpawnSceneData(); 
		}
        SpawnLocalPlayer();        
    }
	
	/**
	 * Spawn the objects in this scene. 
	 * Used by master 
	 * */
	void SpawnSceneData()
	{
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
        GameObject theGO = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 pos = theGO.transform.position;
        Quaternion rot = theGO.transform.rotation;
		
        int id1 = PhotonNetwork.AllocateViewID();
        int id2 = PhotonNetwork.AllocateViewID();
		
		
	}
	
	public void addObjectToScene(string obj, Vector3 pos, Quaternion rot, ObjectsSpawnTypes spawnType) {
        int id = PhotonNetwork.AllocateViewID();
        photonView.RPC("SpawnObjOnNetwork", PhotonTargets.AllBuffered, obj, pos, rot, id, spawnType);
	}
	
	public void addBuildingToScene(string building, Vector3 pos, Quaternion rot) {
        int id = PhotonNetwork.AllocateViewID();
        photonView.RPC("SpawnBuildingOnNetwork", PhotonTargets.AllBuffered, building, pos, rot, id);
	}
	
	/**
	 * We spawn a player prefab on a spawn location
	 * And most importantly, we send the message to the network
	 */
    void SpawnLocalPlayer()
    {

        //Get random spawnpoint
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
        GameObject theGO = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 pos = theGO.transform.position;
        Quaternion rot = theGO.transform.rotation;

        //Manually allocate PhotonViewID
        int id1 = PhotonNetwork.AllocateViewID();
        
        photonView.RPC("AddPlayer", PhotonTargets.AllBuffered, PhotonNetwork.player);
        photonView.RPC("SpawnPlOnNetwork", PhotonTargets.AllBuffered, pos, rot, id1, PhotonNetwork.player);
    }
	

    //////////////////////////////
    // Manage players
	
	/**
	 * Adds a player to the game's datalist. 
	 * If it's a local player, we own it
	 * Warning : we do not instantiate the gameobject here, but on SpawnOnNetwork() !
	 */
    [RPC]
    void AddPlayer(PhotonPlayer networkPlayer)
    {
        if (GetPlayer(networkPlayer) != null)
        {
            Debug.LogError("AddPlayer: Player already exists!");
            return;
        }

        PlayerInfo4 pla = new PlayerInfo4();
        pla.networkPlayer = networkPlayer;
        playerList.Add(pla);

        if (pla.IsLocal())
        {
            if (localPlayerInfo != null) { Debug.LogError("localPlayerInfo already set?"); }
            localPlayerInfo = pla;
        }
    }
	
	/**
	 * Spawn a player's gameobject to the game
	 * Also used when a new player joins the game and has to load players that are already there
	 */
	[RPC]
    void SpawnPlOnNetwork(Vector3 pos, Quaternion rot, int id1, PhotonPlayer np)
    {
        GameObject newPlayerObject = PhotonNetwork.Instantiate("Prefabs/Players/PlayerShip", pos, rot, 0) ;
		Transform newPlayer = (Transform) newPlayerObject.transform; 
		newPlayer.name = "Player#"+id1; 
		newPlayer.transform.parent = GameObject.FindGameObjectWithTag(Phobos.Vars.PLAYERS_TAG).transform; 
        //Set transform
        PlayerInfo4 pNode = GetPlayer(np);
        pNode.transform = newPlayer;
        //Set photonview ID everywhere!
        SetPhotonViewIDs(newPlayer.gameObject, id1);
		
		//Enable the netscript and set player status for this instantiated player
		PlayerNetscript netScript = (PlayerNetscript) newPlayer.GetComponent(typeof(PlayerNetscript));
		netScript.SetPlayer(np); 
		
		
        if (pNode.IsLocal())
        {
			//If it's local player, we own it. We can control it and send RPCs with it
            localPlayerInfo = pNode;
			GameController GC = (GameController) this.GetComponent(typeof(GameController));
			GC.setPlayer(newPlayer);
        } 
		
		
        //Maybe call some specific action on the instantiated object?
        //PLAYERSCRIPT tmp = newPlayer.GetComponent<PLAYERSCRIPT>();
        //tmp.SetPlayer(pNode.networkPlayer);
    }
	
	
	/**
	 * Spawn an object to the game
	 */
	[RPC]
    void SpawnObjOnNetwork(string prefab, Vector3 pos, Quaternion rot, int id1, ObjectsSpawnTypes spawnType)
    {
		GameObject newObject =null ; 
		object[] data =null ; 
		newObject = PhotonNetwork.InstantiateSceneObject("Prefabs/Objects/" + prefab, pos, rot, 0, data ) as GameObject;
		
		PhotonView PV = (PhotonView) newObject.GetComponent(typeof(PhotonView));
		//this.netObjects.Add(id1, newObject.transform); 
		
    }
	
	[RPC]
    void SpawnBuildingOnNetwork(string building, Vector3 pos, Quaternion rot, int id)
    {
        GameObject newBuilding = (GameObject) PhotonNetwork.InstantiateSceneObject("Prefabs/Objects/Building/"+building, pos, rot, 0, null) ;
		newBuilding.transform.parent = GameObject.FindGameObjectWithTag(Phobos.Vars.OBJECTS_TAG).transform; 
		newBuilding.name = "Building#"+id; 
		
        SetPhotonViewIDs(newBuilding.gameObject, id);
		
		this.netObjects.Add(id, newBuilding.transform); 
		
    }

	/**
	 * Player disconnected
	 * We destroy it's object
	 */
    void RemovePlayer(PhotonPlayer networkPlayer)
    {
        PlayerInfo4 thePlayer = GetPlayer(networkPlayer);

        if (thePlayer.transform)
        {
            Destroy(thePlayer.transform.gameObject);
        }
        playerList.Remove(thePlayer);
    }

	/**
	 * Get a player in the current player list of this room
	 * 
	 */
    PlayerInfo4 GetPlayer(PhotonPlayer networkPlayer)
    {
        foreach (PlayerInfo4 pla in playerList)
        {
            if (pla.networkPlayer == networkPlayer)
            {
                return pla;
            }
        }
        return null;
    }
	
	public GameObject getObjectsContainer() {
		return GameObject.FindGameObjectWithTag(Phobos.Vars.OBJECTS_TAG); 	
	}
	
	public GameObject getPlayersContainer() {
		return GameObject.FindGameObjectWithTag(Phobos.Vars.PLAYERS_TAG); 	
	}
	
	/**
	 * Get a player or object in the current room 
	 * 
	 */
    public Transform getPlayerOrObject(int PhotonID)
    {
		GameObject ObjectsList = this.getObjectsContainer(); 
		GameObject PlList = this.getPlayersContainer(); 
		PhotonView PV ;
		
        foreach (Transform obj in ObjectsList.transform)
        {
            PV = (PhotonView) obj.GetComponent(typeof(PhotonView)); 
			if (PV != null) {
				if (PV.viewID == PhotonID)
					return obj; 
			}
        }
		foreach (Transform pl in PlList.transform)
        {
            PV = (PhotonView) pl.GetComponent(typeof(PhotonView)); 
			if (PV != null) {
				if (PV.viewID == PhotonID)
					return pl; 
			}
        }
		
		
        return null;
    }
    

    //When a PhotonView instantiates it has viewID=0 and is unusable.
    //We need to assign the right viewID -on all players(!)- for it to work
    public void SetPhotonViewIDs(GameObject go, int id1)
    {
        PhotonView[] nViews = go.GetComponentsInChildren<PhotonView>();
        nViews[0].viewID = id1;
    }
    

    //On all clients: When a remote client disconnects, cleanup
    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        PlayerInfo4 pNode = GetPlayer(player);
        if (pNode != null)
        {
            //string playerNameLeft= pNode.name;
            //I.e.: Chat.SP.addGameChatMessage(playerNameLeft+" left the game");
        }
        RemovePlayer(player);
    }



}