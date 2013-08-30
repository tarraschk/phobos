using UnityEngine;
using System.Collections;

public class ConnectAuto : MonoBehaviour {

    private bool receivedRoomList = false;
	
	 /*
     * We want this script to automatically connect to Photon and to enter a room.
     * This will help speed up debugging in the next tutorials.
     * 
     * In Awake we connect to the Photon server(/cloud).
     * Via OnConnectedToPhoton(); we will either join an existing room (if any), otherwise create one. 
     */

    void Awake()
    {
        if (PhotonNetwork.connectionState == ConnectionState.Disconnected)
        {
        	PhotonNetwork.ConnectUsingSettings("1.0");
		}
		else {
        	StartCoroutine(JoinOrCreateRoom());	
		}
    }
	
	void OnLevelWasLoaded(int level) {
	}
	
	void OnConnectedToPhoton()
    {
		Debug.Log ("Connected to Photon.");
        StartCoroutine(JoinOrCreateRoom());
    }
	
	void OnDisconnectedFromPhoton()
    {
        receivedRoomList = false;
    }
	
	
	void OnGUI()
    {
        //Check connection state..
        if (PhotonNetwork.connectionState == ConnectionState.Disconnected)
        {
            //We are currently disconnected
            GUILayout.Label("Connection status: Disconnected");

            GUILayout.BeginVertical();
            if (GUILayout.Button("Connect"))
            {
                //Connect using the PUN wizard settings (Self-hosted server or Photon cloud)
                PhotonNetwork.ConnectUsingSettings("1.0");
            }
            GUILayout.EndVertical();
        }
        else
        {
            //We're connected!
            if (PhotonNetwork.connectionState == ConnectionState.Connected)
            {
                GUILayout.Label("Connection status: Connected");
                if (PhotonNetwork.room != null)
                {
					if (PhotonNetwork.isMasterClient) {
						GUILayout.Label("You are the Master");
					}
                    GUILayout.Label("Room: " + PhotonNetwork.room.name);
                    GUILayout.Label("Players: " + PhotonNetwork.room.playerCount + "/" + PhotonNetwork.room.maxPlayers);

                }
                else
                {
                    GUILayout.Label("Not inside any room");
                }

                GUILayout.Label("Ping to server: " + PhotonNetwork.GetPing());
            }
            else
            {
                //Connecting...
                GUILayout.Label("Connection status: " + PhotonNetwork.connectionState);
            }
        }
    }
	
	
	/// <summary>
    /// Helper function to speed up our testing: 
    /// - after connecting to Photon, check for active rooms and join the first if possible
    /// - if no roomlist was found within 2 seconds: Create a room
    /// </summary>
    /// <returns></returns>
    IEnumerator JoinOrCreateRoom()
    {
		Debug.Log ("Trying to join or create a room.");
        float timeOut = Time.time + 2;
        while (Time.time < timeOut && !receivedRoomList)
        {
            yield return 0;
        }
        //We still didn't join any room: create one
        if (PhotonNetwork.room == null){
            string roomName = "TestRoom"+Application.loadedLevelName;
			Debug.Log ("Trying to create a room : " + roomName);
            PhotonNetwork.CreateRoom(roomName, true, true, 4);
        }
    }
	
	/// <summary>
    /// This is called when we are connect to Photon in the lobby state, upon receiving a new roomlist.
    /// </summary>
    void OnReceivedRoomList()
    {
		Debug.Log ("Received Room List.");
        string wantedRoomName = "TestRoom" + Application.loadedLevelName;
        foreach(RoomInfo room in PhotonNetwork.GetRoomList()){
            if (room.name == wantedRoomName)
            {
                PhotonNetwork.JoinRoom(room.name);
                break;
            }
        }
        receivedRoomList = true;
    }
	
	/// <summary>
    /// Not used in this script, just to show how list updates are handled.
    /// </summary>
    void OnReceivedRoomListUpdate()
    {
        Debug.Log("We received a room list update, total rooms now: " + PhotonNetwork.GetRoomList().Length);
		string wantedRoomName = "TestRoom" + Application.loadedLevelName;
        foreach(RoomInfo room in PhotonNetwork.GetRoomList()){
            if (room.name == wantedRoomName)
            {
                PhotonNetwork.JoinRoom(room.name);
                break;
            }
        }
    }
}
