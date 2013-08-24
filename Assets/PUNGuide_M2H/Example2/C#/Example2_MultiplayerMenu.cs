
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Example2_MultiplayerMenu : Photon.MonoBehaviour
{

    //GUI vars

    private string currentGUIMethod = "join";

    private Vector2 JoinScrollPosition;

    private string joinRoomName;

    private string failConnectMesage = "";
    bool isConnectingToRoom = false;



    void Awake()
    {

        //Default join values
        joinRoomName = "";

        //Default host values
        hostTitle = PlayerPrefs.GetString("hostTitle", "Guests server");
        hostMaxPlayers = PlayerPrefs.GetInt("hostPlayers", 8);

   

        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("1.0");
    }


    void OnConnectedToPhoton()
    {
        Debug.Log("This client has connected to a server");
        failConnectMesage = "";
    }
    void OnDisconnectedFromPhoton()
    {
        Debug.Log("This client has disconnected from the server");
        failConnectMesage = "Disconnected from Photon";
    }

    void OnFailedToConnectToPhoton(ExitGames.Client.Photon.StatusCode status)
    {
        Debug.Log("Failed to connect to Photon: " + status);
        failConnectMesage = "Failed to connect to Photon: " + status;
    }


    void OnGUI()
    {
        if (!PhotonNetwork.connected)
        {
            GUILayout.Label("Connecting..");
            if (failConnectMesage != "")
            {
                GUILayout.Label("Error message: " + failConnectMesage);
                if (GUILayout.Button("Retry"))
                {
                    failConnectMesage = "";
                    PhotonNetwork.ConnectUsingSettings("1.0");
                }
            }
        }
        else
        {
            GUILayout.Window(2, new Rect(Screen.width / 2 - 600 / 2, Screen.height / 2 - 550 / 2, 600, 550), WindowGUI, "");
        }
    }





    void WindowGUI(int wID)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Multiplayer menu");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (currentGUIMethod == "join")
        {
            GUILayout.Label("Join");
        }
        else
        {
            if (GUILayout.Button("Join"))
            {
                currentGUIMethod = "join";
            }
        }
        if (currentGUIMethod == "create")
        {
            GUILayout.Label("Create");
        }
        else
        {
            if (GUILayout.Button("Create"))
            {
                currentGUIMethod = "create";
            }
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(25);


        if (currentGUIMethod == "join")
            JoinMenu();
        else
            HostMenu();

    }


    void JoinMenu()
    {

        if (isConnectingToRoom)
        {

            GUILayout.Label("Trying to connect to a room.");

        }
        else if (failConnectMesage != "")
        {
            GUILayout.Label("The game failed to connect:\n" + failConnectMesage);
            GUILayout.Space(10);
            if (GUILayout.Button("Cancel"))
            {
                failConnectMesage = "";
            }
        }
        else
        {

            //Masterlist
            GUILayout.BeginHorizontal();
            GUILayout.Label("Game list:");


            GUILayout.FlexibleSpace();
            if ( PhotonNetwork.GetRoomList().Length > 0 &&
                GUILayout.Button("Join random game"))
            {
                PhotonNetwork.JoinRandomRoom();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
            GUILayout.Space(24);

            GUILayout.Label("Title", GUILayout.Width(200));
            GUILayout.Label("Players", GUILayout.Width(55));
            GUILayout.EndHorizontal();


            JoinScrollPosition = GUILayout.BeginScrollView(JoinScrollPosition);
            foreach (RoomInfo room in PhotonNetwork.GetRoomList())
            {
                GUILayout.BeginHorizontal();


                if ((room.playerCount < room.maxPlayers || room.maxPlayers<=0) &&
                    GUILayout.Button("" + room.name, GUILayout.Width(200)))
                {
                    PhotonNetwork.JoinRoom(room.name);
                }
                GUILayout.Label(room.playerCount + "/" + room.maxPlayers, GUILayout.Width(55));
        



                GUILayout.EndHorizontal();
            }
            if (PhotonNetwork.GetRoomList().Length == 0)
            {
                GUILayout.Label("No games are running right now");
            }
            GUILayout.EndScrollView();

            string text = PhotonNetwork.GetRoomList().Length + " total rooms";
            GUILayout.Label(text);

            //DIRECT JOIN

            GUILayout.BeginHorizontal();
            GUILayout.Label("Join by name:");
            GUILayout.Space(5);
            GUILayout.Label("Room name");
            joinRoomName = (GUILayout.TextField(joinRoomName + "", GUILayout.Width(50)) + "");

            if (GUILayout.Button("Connect"))
            {
                PhotonNetwork.JoinRoom(joinRoomName);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(4);

        }
    }


    void OnPhotonCreateRoomFailed()
    {
        Debug.Log("A CreateRoom call failed, most likely the room name is already in use.");
        failConnectMesage = "Could not create new room, the name is already in use.";
    }

    void OnPhotonJoinRoomFailed()
    {
        Debug.Log("A JoinRoom call failed, most likely the room name does not exist or is full.");
        failConnectMesage = "Could not connect to the desired room, this room does no longer exist or all slots are full.";
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("A JoinRandom room call failed, most likely there are no rooms available.");
        failConnectMesage = "Could not connect to random room; no rooms were available.";
    }


    void OnJoinedRoom()
    {
        //Stop communication until in the game
	PhotonNetwork.LoadLevel(Application.loadedLevel + 1);
    }




    private string hostTitle;
    //private string hostDescription;
    private int hostMaxPlayers;
    

    void HostMenu()
    {


        GUILayout.BeginHorizontal();
        GUILayout.Label("Host a new game:");
        GUILayout.EndHorizontal();

   
        GUILayout.BeginHorizontal();
        GUILayout.Label("Title:");
        GUILayout.FlexibleSpace();
        hostTitle = GUILayout.TextField(hostTitle, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        /*GUILayout.BeginHorizontal();
        GUILayout.Label("Server description");
        GUILayout.FlexibleSpace();
        hostDescription = GUILayout.TextField(hostDescription, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        */

        GUILayout.BeginHorizontal();
        GUILayout.Label("Max players (1-32)");
        GUILayout.FlexibleSpace();
        hostMaxPlayers = int.Parse(GUILayout.TextField(hostMaxPlayers + "", GUILayout.Width(50)) + "");
        GUILayout.EndHorizontal();

        CheckHostVars();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Start server", GUILayout.Width(150)))
        {
            StartHostingGame(hostTitle, hostMaxPlayers);
        }
        GUILayout.EndHorizontal();
    }

    void CheckHostVars()
    {
        hostMaxPlayers = Mathf.Clamp(hostMaxPlayers, 1, 64);
    }


    void StartHostingGame(string hostSettingTitle, int hostPlayers)
    {
        if (hostSettingTitle == "")
        {
            hostSettingTitle = "NoTitle";
        }

        hostPlayers = Mathf.Clamp(hostPlayers, 0, 64);

        PhotonNetwork.CreateRoom(hostSettingTitle, true, true, hostPlayers);
    }




    //
    // CUSTOM HOST LIST
    //
    // You could use this to implement custom sorting, or adding custom fields.
    //

    /*
    private List<MyRoomData> hostDataList = new List<MyRoomData>();

     
    void OnReceivedRoomList()
    {
        Debug.Log("We received a new room list, total rooms: " + PhotonNetwork.GetRoomList().Length);
        ReloadHostList();
    }

    void OnReceivedRoomListUpdate()
    {
        Debug.Log("We received a room list update, total rooms now: " + PhotonNetwork.GetRoomList().Length);
        ReloadHostList();
    }



    void ReloadHostList()
    {        
        hostDataList =new List<MyRoomData>();
        foreach(RoomInfo room in PhotonNetwork.GetRoomList())
        {
            MyRoomData cHost= new MyRoomData();
            cHost.room = room;
      
            
            hostDataList.Add(cHost);
            
        }
    }



    public class MyRoomData
    {
        public Room room;

        public string title
        {
            get { return room.name; }
        }
        public int connectedPlayers
        {
            get { return room.playerCount; }
        }
        public int maxPlayers
        {
            get { return room.maxPlayers; }
        }

        //Example custom fields
        public int gameVersion; // You could 
    }
     */


}