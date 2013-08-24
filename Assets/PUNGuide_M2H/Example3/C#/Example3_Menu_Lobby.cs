

using UnityEngine;
using System.Collections;

public class Example3_Menu_Lobby : Photon.MonoBehaviour
{
    public static Example3_Menu_Lobby SP;

    private bool launchingGame = false;
    private bool showMenu = false;

    private int serverMaxPlayers = 4;
    private string serverTitle = "Loading..";


    void Awake()
    {
        SP = this;
        showMenu = false;
    }

 

    public void EnableLobby()
    {
        launchingGame = false;
        showMenu = true;
    }

    void OnGUI()
    {
        if (!showMenu)
        {
            return;
        }


        //Back to main menu
        if (GUI.Button(new Rect(40, 10, 150, 20), "Back to main menu"))
        {
            LeaveLobby();
        }

        if (launchingGame)
        {
            LaunchingGameGUI();

        }
        else if (!PhotonNetwork.isMasterClient && !PhotonNetwork.isNonMasterClientInRoom)
        {
            //First set player count, server name and password option			
            CreationSettings();

        }
        else
        {
            //Show the lobby		
            ShowLobby();
        }
    }

    void LeaveLobby()
    {
        //Disconnect fdrom host, or shutduwn host
        PhotonNetwork.LeaveRoom();
        

        Example3_Menu.SP.OpenMenu("multiplayer");
        showMenu = false;
    }

    private string hostSetting_title = "No server title";
    private int hostSetting_players = 4;

    void CreationSettings()
    {

        GUI.BeginGroup(new Rect(Screen.width / 2 - 175, Screen.height / 2 - 75 - 50, 350, 150));
        GUI.Box(new Rect(0, 0, 350, 150), "Server options");

        GUI.Label(new Rect(10, 20, 150, 20), "Server title");
        hostSetting_title = GUI.TextField(new Rect(175, 20, 160, 20), hostSetting_title);

        GUI.Label(new Rect(10, 40, 150, 20), "Max. players (2-64)");
        hostSetting_players = int.Parse(GUI.TextField(new Rect(175, 40, 160, 20), hostSetting_players + ""));
               

        if (GUI.Button(new Rect(100, 115, 150, 20), "Go to lobby"))
        {
            StartHost(hostSetting_players, hostSetting_title);
        }
        GUI.EndGroup();
    }

    void StartHost(int players, string serverName)
    {
        players = Mathf.Clamp(players, 1, 64);        
        serverTitle = serverName;
        PhotonNetwork.CreateRoom(serverName, true, true, players);
    }

    void ShowLobby()
    {
        string players = "";
        int currentPlayerCount = 0;
        foreach (PhotonPlayer playerInstance in PhotonNetwork.playerList)
        {
            players = playerInstance.name + "\n" + players;
            currentPlayerCount++;
        }

        GUI.BeginGroup(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 180));
        GUI.Box(new Rect(0, 0, 400, 200), "Game lobby");

        
        GUI.Label(new Rect(10, 40, 150, 20), "Server title");
        GUI.Label(new Rect(150, 40, 100, 100), serverTitle);

        GUI.Label(new Rect(10, 60, 150, 20), "Players");
        GUI.Label(new Rect(150, 60, 100, 100), currentPlayerCount + "/" + serverMaxPlayers);

        GUI.Label(new Rect(10, 80, 150, 20), "Current players");
        GUI.Label(new Rect(150, 80, 100, 100), players);


        if (PhotonNetwork.isMasterClient)
        {
            if (GUI.Button(new Rect(25, 140, 150, 20), "Start the game"))
            {
                HostLaunchGame();
            }
        }
        else
        {
            GUI.Label(new Rect(25, 140, 200, 40), "Waiting for the server to start the game..");
        }

        GUI.EndGroup();
    }



    void OnCreatedRoom()
    {
        //Called on masterclient
        photonView.RPC("SetServerSettings", PhotonTargets.OthersBuffered, hostSetting_title);
    }

    void OnMasterClientSwitched(PhotonPlayer newMaster)
    {
        Debug.Log("The old masterclient left, we have a new masterclient: " + newMaster); 
        if (PhotonNetwork.player == newMaster)
        {
            photonView.RPC("SetServerSettings", PhotonTargets.OthersBuffered, serverTitle);
        }       
    }


    [RPC]
    void SetServerSettings(string newSrverTitle)
    {
        serverTitle = newSrverTitle;
    }
  

    void HostLaunchGame()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }
        //Close the room for future connections
        PhotonNetwork.room.open = false;
        PhotonNetwork.room.visible = false;
    

        photonView.RPC("LaunchGame", PhotonTargets.All);
    }

    [RPC]
    void LaunchGame()
    {
        launchingGame = true;
    }

    void LaunchingGameGUI()
    {
        //Show loading progress, ADD LOADINGSCREEN?
        GUI.Box(new Rect(Screen.width / 4 + 180, Screen.height / 2 - 30, 280, 50), "");
        if (Application.CanStreamedLevelBeLoaded((Application.loadedLevel + 1)))
        {
            	GUI.Label(new Rect(Screen.width / 4 + 200, Screen.height / 2 - 25, 285, 150), "Loaded, starting the game!");
		PhotonNetwork.LoadLevel((Application.loadedLevel + 1));
        }
        else
        {
            GUI.Label(new Rect(Screen.width / 4 + 200, Screen.height / 2 - 25, 285, 150), "Starting..Loading the game: " + Mathf.Floor(Application.GetStreamProgressForLevel((Application.loadedLevel + 1)) * 100) + " %");
        }

    }

}