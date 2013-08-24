using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Example3_Menu_Join : MonoBehaviour
{
   

    public GUISkin skin;
    private Rect windowRect1;
    private Rect windowRect2;

    private string errorMessage = "";
    public  GUIStyle customButton;

    private bool showMenu = false;



    void Awake()
    {
        windowRect1 = new Rect(Screen.width / 2 - 305, Screen.height / 2 - 140, 380, 280);
    }



    public void EnableMenu()
    {
        showMenu = true;
    }

    void OnJoinedRoom()
    {
        Debug.Log("Connected to lobby!");
        showMenu = false;
        Example3_Menu_Lobby.SP.EnableLobby();

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
            showMenu = false;
            Example3_Menu.SP.OpenMenu("multiplayer");
        }


        if (errorMessage != null && errorMessage != "")
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 30, 200, 60), "Error");
            GUI.Label(new Rect(Screen.width / 2 - 90, Screen.height / 2 - 15, 180, 50), errorMessage);
            if (GUI.Button(new Rect(Screen.width / 2 + 40, Screen.height / 2 + 5, 50, 20), "Close"))
            {
                errorMessage = "";
            }
        }


        if (errorMessage == "")
        { //Hide windows on error
            windowRect1 = GUILayout.Window(0, windowRect1, listGUI, "Join a game via the list");
         }



    }


    private Vector2 joinScrollPosition;

    void listGUI(int id)
    {

        GUILayout.BeginVertical();
        GUILayout.Space(6);
        GUILayout.EndVertical();

        //Masterlist
       GUILayout.Label("Game list:");

 
        GUILayout.Space(2);
        GUILayout.BeginHorizontal();
        GUILayout.Space(24);

        GUILayout.Label("Title", GUILayout.Width(200));
        GUILayout.Label("Players", GUILayout.Width(55));
        GUILayout.EndHorizontal();


        joinScrollPosition = GUILayout.BeginScrollView(joinScrollPosition);
        foreach (RoomInfo room in PhotonNetwork.GetRoomList())
        {
            if (!room.open) continue;

            GUILayout.BeginHorizontal();
            if (  (room.playerCount<room.maxPlayers || room.maxPlayers == 0) &&
                GUILayout.Button("" + room.name, GUILayout.Width(200)))
            {
                PhotonNetwork.JoinRoom(room.name);    
            }
            GUILayout.Label(room.playerCount + "/" + room.maxPlayers, GUILayout.Width(55));
            
            GUILayout.EndHorizontal();
        }
        if (PhotonNetwork.GetRoomList().Length == 0)
        {
            GUILayout.Label("No servers running right now");
        }
        GUILayout.EndScrollView();

        GUILayout.Label( PhotonNetwork.GetRoomList().Length + " total servers" );


    }
   




}