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

    public Transform playerPrefab;

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
        SpawnLocalPlayer();        
    }
	
	/**
	 * We spawn a player prefab on a spawn location
	 * And most importantly, we send the message to the network
	 */
    void SpawnLocalPlayer()
    {
        Debug.Log("SpawnLocalPlayer ");

        //Get random spawnpoint
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
        GameObject theGO = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 pos = theGO.transform.position;
        Quaternion rot = theGO.transform.rotation;

        //Manually allocate PhotonViewID
        int id1 = PhotonNetwork.AllocateViewID();
        
        photonView.RPC("AddPlayer", PhotonTargets.AllBuffered, PhotonNetwork.player);
        photonView.RPC("SpawnOnNetwork", PhotonTargets.AllBuffered, pos, rot, id1, PhotonNetwork.player);
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
        Debug.Log("AddPlayer " + networkPlayer );
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
	 */
	[RPC]
    void SpawnOnNetwork(Vector3 pos, Quaternion rot, int id1, PhotonPlayer np)
    {
        Transform newPlayer = Instantiate(playerPrefab, pos, rot) as Transform;
		newPlayer.name = "Player#"+id1; 
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
			Controls controlScript = (Controls) this.GetComponent(typeof(Controls));
			controlScript.setPlayer(newPlayer); 
        } 
		
		
        //Maybe call some specific action on the instantiated object?
        //PLAYERSCRIPT tmp = newPlayer.GetComponent<PLAYERSCRIPT>();
        //tmp.SetPlayer(pNode.networkPlayer);
    }

	/**
	 * Player disconnected
	 * We destroy it's object
	 */
    void RemovePlayer(PhotonPlayer networkPlayer)
    {
        Debug.Log("RemovePlayer " + networkPlayer);
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
    

    //When a PhotonView instantiates it has viewID=0 and is unusable.
    //We need to assign the right viewID -on all players(!)- for it to work
    void SetPhotonViewIDs(GameObject go, int id1)
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