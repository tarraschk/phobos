using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager5 : Photon.MonoBehaviour
{
    public static GameManager5 SP;

    public Transform playerPrefab;

    private List<PlayerInfo5> playerList;
    PlayerInfo5 localPlayer;
    private Transform myLocalTransform;
    private Camera mainCamera;


    class PlayerInfo5
    {
        public PhotonPlayer networkPlayer;
        public Transform transform;

        public string name
        {
            get
            {
                return networkPlayer.name;
            }
        }
        public bool isLocal
        {
            get
            {
                return networkPlayer.isLocal;
            }
        }

    }

    void Awake()
    {
        SP = this;
        playerList = new List<PlayerInfo5>();
        mainCamera = Camera.main;

        SpawnLocalPlayer();

        PhotonNetwork.autoCleanUpPlayerObjects = false;

        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("Example5.0");


        StartNewAutoMatchmaking();

    }



    public GUIStyle customGUIStyle;
    private string debugMatchmakingStatus = "";

    void OnGUI()
    {
        GUILayout.Label("PlayerLists=" + playerList.Count + " vs " + PhotonNetwork.playerList.Length);
        GUILayout.Label("Current matchmaking status=" + debugMatchmakingStatus);

        //On-Screen
        foreach (PlayerInfo5 node in playerList)
        {
            if (node.transform)
            {
                //GUI.color=Color.white;
                Vector3 screenPos = mainCamera.WorldToScreenPoint(node.transform.position + new Vector3(0, 2, 0));
                if (screenPos.z > 0 && Vector3.Distance(mainCamera.transform.position, node.transform.position) <= 450)
                {
                    GUI.Label(new Rect(screenPos.x - 45, Screen.height - screenPos.y, 90, 15), node.name, customGUIStyle);
                }
            }
        }

    }



    [RPC]
    void AddPlayer(PhotonPlayer networkPlayer)
    {
        if (GetPlayer(networkPlayer) != null)
        {
            Debug.LogError("AddPlayer: Player already exists!");
            return;
        }
        PlayerInfo5 pla = new PlayerInfo5();
        pla.networkPlayer = networkPlayer;
        playerList.Add(pla);

        if (networkPlayer.isLocal)
            localPlayer = pla;
    }

    void SetPlayerTransform(PhotonPlayer networkPlayer, Transform pTransform)
    {
        if (!pTransform)
        {
            Debug.LogError("SetPlayersTransform has a NULL playerTransform!");
        }
        PlayerInfo5 thePlayer = GetPlayer(networkPlayer);
        if (thePlayer == null)
        {
            Debug.LogError("SetPlayersPlayerTransform: No player found! " + networkPlayer + "  " + pTransform + " trans=" + playerList.Count);
        }
        thePlayer.transform = pTransform;
    }


    [RPC]
    void RemovePlayer(PhotonPlayer networkPlayer)
    {
        PlayerInfo5 thePlayer = GetPlayer(networkPlayer);

        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.DestroyPlayerObjects(networkPlayer);
        }
        if (thePlayer.transform)
        {
            Destroy(thePlayer.transform.gameObject);
        }
        playerList.Remove(thePlayer);
    }


    PlayerInfo5 GetPlayer(PhotonPlayer networkPlayer)
    {
        foreach (PlayerInfo5 pla in playerList)
        {
            if (pla.networkPlayer == networkPlayer)
            {
                return pla;
            }
        }
        return null;
    }


    void ServerStarted()
    {
        debugMatchmakingStatus = "ServerStarted";

        playerList = new List<PlayerInfo5>();//Clear list
        playerList.Add(localPlayer);

        int id1 = PhotonNetwork.AllocateViewID();
        SetNetworkViewIDs(myLocalTransform.gameObject, id1);
        SetPlayerTransform(PhotonNetwork.player, myLocalTransform);
    }




    void SpawnLocalPlayer()
    {
        //Spawn local player
        // Randomize starting location
        Vector3 pos = transform.position;
        Quaternion rot = Quaternion.identity;

        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
        GameObject theGO = spawnPoints[Random.Range(0, spawnPoints.Length)];
        pos = theGO.transform.position;
        rot = theGO.transform.rotation;

        int id1 = PhotonNetwork.AllocateViewID();

        AddPlayer(PhotonNetwork.player);
        SpawnOnNetwork(pos, rot, id1, true, PhotonNetwork.player);
        /*if (PhotonNetwork.isNonMasterClientInGame)
        {
            Debug.Log("AddPlayer  SpawnLocalPlayer");

            photonView.RPC("AddPlayer", PhotonTargets.OthersBuffered, PhotonNetwork.player, PlayerPrefs.GetString("playerName"));
            photonView.RPC("SpawnOnNetwork", PhotonTargets.OthersBuffered, pos, rot, id1, PlayerPrefs.GetString("playerName"), false, PhotonNetwork.player);
        }*/

    }


    [RPC]
    void SpawnOnNetwork(Vector3 pos, Quaternion rot, int id1, bool amOwner, PhotonPlayer np)
    {
        Transform newPlayer = Instantiate(playerPrefab, pos, rot) as Transform;
        SetPlayerTransform(np, newPlayer);

        SetNetworkViewIDs(newPlayer.gameObject, id1);

        if (amOwner)
        {
            myLocalTransform = newPlayer;
        }
        Player5 tmp = newPlayer.GetComponent<Player5>();
        tmp.SetOwner(amOwner);
    }


    void SetNetworkViewIDs(GameObject go, int id1)
    {
        Component[] nViews = go.GetComponentsInChildren<PhotonView>();
        (nViews[0] as PhotonView).viewID = id1;
    }


    void StartNewAutoMatchmaking()
    {
        if (PhotonNetwork.room != null)
        {
            if (PhotonNetwork.room.playerCount > 2)
            {
                Debug.LogError("StartNewAutoMatchmaking not required, we have more then 1 player!");
                return;
            }
        }

        if (!autoJoinRunning)
            StartCoroutine(AutoJoinFeature());
    }

    //On client: When just connected to a server
    IEnumerator OnJoinedRoom()
    {
        tryJoinRandom = false;
        debugMatchmakingStatus = "ConnectedToServer (client)";
        playerList = new List<PlayerInfo5>();
        playerList.Add(localPlayer);

        int id1 = PhotonNetwork.AllocateViewID();

        photonView.RPC("AddPlayer", PhotonTargets.OthersBuffered, PhotonNetwork.player);
        photonView.RPC("SpawnOnNetwork", PhotonTargets.OthersBuffered, myLocalTransform.position, myLocalTransform.rotation, id1, false, PhotonNetwork.player);
        yield return 0;
        SetPlayerTransform(PhotonNetwork.player, myLocalTransform);
        SetNetworkViewIDs(myLocalTransform.gameObject, id1);

    }

    void OnMasterClientSwitched(PhotonPlayer newMC)
    {
        Debug.Log(Time.frameCount+" NEW MasterClient: " + newMC);
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {      
        if (PhotonNetwork.isMasterClient)
        {
            PlayerInfo5 pNode = GetPlayer(player);
            if (pNode != null)
            {
                string playerNameLeft = pNode.name;
                Chat_example1.SP.AddGameChatMessage(playerNameLeft + " left the game", true);
            }
        }
        RemovePlayer(player);
     
        if (PhotonNetwork.playerList.Length <=1)
        {
            StartNewAutoMatchmaking();
        }
    }


    void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("OnPhotonPlayerConnected " + player);
        //Nothing
    }


    //On server: When this game just switched from non-networking to networked
    void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom ");
        ServerStarted();
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.Log("OnDisconnectedFromPhoton ");
        PhotonNetwork.offlineMode = true;
    }


    void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
        haveReceivedRoomList = false;


        for (int i = playerList.Count - 1; i >= 0; i--)
        {
            PlayerInfo5 pla = playerList[i];
            if (!pla.networkPlayer.isLocal)
            {
                RemovePlayer(pla.networkPlayer);
            }
        }
    }

    void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby" + PhotonNetwork.connectionStateDetailed);
    }

    private bool haveReceivedRoomList = false;

    void OnReceivedRoomList()
    {
        Debug.Log("OnReceivedRoomList ");
        haveReceivedRoomList = true;
    }
    void OnReceivedRoomListUpdate()
    {
        Debug.Log("OnReceivedRoomListUpdate ");
        haveReceivedRoomList = true;
    }

    void OnPhotonCreateRoomFailed()
    {
        StartNewAutoMatchmaking(); //Catch all errors, will abort if we're already in the AutoJoinFeature
    }

    void OnPhotonJoinRoomFailed()
    {
        StartNewAutoMatchmaking(); //Catch all errors, will abort if we're already in the AutoJoinFeature
    }

    private bool autoJoinRunning = false;

    /// <summary>
    /// Try to find a populated match: join if available, otherwise host a room ourselves and wait for a bit..
    /// </summary>
    /// <returns></returns>

    IEnumerator AutoJoinFeature()
    {
        autoJoinRunning = true;

        while (PhotonNetwork.playerList.Length <= 1)
        {

            if (PhotonNetwork.room == null)
            {
                yield return StartCoroutine(AutoJoinTryConnecting());
            }          
            if (PhotonNetwork.playerList.Length <= 1)
            {
                yield return StartCoroutine(AutoJoinTryCreating());
            }
            yield return 0;
        }


        autoJoinRunning = false;
    }


    //
    // JOIN RANDOM ROOM: Only if available..
    //

    private bool tryJoinRandom = false;

    IEnumerator AutoJoinTryConnecting()
    {
        while (!haveReceivedRoomList)
        {
            yield return 0;
        }

        tryJoinRandom = true;
        PhotonNetwork.JoinRandomRoom();
        while (tryJoinRandom)
        {
            yield return 0;
        }
    }
    
    void OnPhotonRandomJoinFailed()
    {
        tryJoinRandom = false;
    }

    //
    //CREATE A NEW ROOM: Wait for player to join you..
    //

    IEnumerator AutoJoinTryCreating()
    {
        if (PhotonNetwork.room == null)
            PhotonNetwork.CreateRoom("", true, true, 16);

        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(Random.Range(2, 5));
            if (PhotonNetwork.playerList.Length > 1)
            {
                yield break;
            }
        }
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.room != null || PhotonNetwork.connectionStateDetailed != PeerState.JoinedLobby)
        {
            yield return 0;
        }
    }


}