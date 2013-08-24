using UnityEngine;
using System.Collections;

public class Example3_Menu : MonoBehaviour
{


    public static Example3_Menu SP;

    private Example3_Menu_Join joinMenuScript;
    private Example3_Menu_Lobby gameLobbyScript;
    private Example3_Menu_Multiplayer multiplayerScript;

    private bool requirePlayerName = false;
    private string playerNameInput = "";

    void Awake()
    {
        SP = this;

        playerNameInput = PlayerPrefs.GetString("playerName" + Application.platform, "");
        requirePlayerName = true;


        joinMenuScript = GetComponent<Example3_Menu_Join>();
        gameLobbyScript = GetComponent<Example3_Menu_Lobby>();
        multiplayerScript = GetComponent<Example3_Menu_Multiplayer>();

        OpenMenu("multiplayer");

        PhotonNetwork.ConnectUsingSettings("1.0");
    }
    void OnGUI()
    {
        if (requirePlayerName)
        {
            GUILayout.Window(9, new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 100), NameMenu, "Please enter a name:");
        }
    }

    public void OpenMenu(string newMenu)
    {
        if (requirePlayerName)
        {
            return;
        }

        if (newMenu == "multiplayer-host")
        {
            gameLobbyScript.EnableLobby();

        }
        else if (newMenu == "multiplayer-join")
        {
            joinMenuScript.EnableMenu();

        }
        else if (newMenu == "multiplayer")
        {
            multiplayerScript.EnableMenu();

        }
        else
        {
            Debug.LogError("Wrong menu:" + newMenu);

        }
    }


    void NameMenu(int id)
    {
        GUILayout.BeginVertical();
        GUILayout.Space(10);


        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.Label("Please enter your name");
        GUILayout.Space(10);
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        playerNameInput = GUILayout.TextField(playerNameInput);
        GUILayout.Space(10);
        GUILayout.EndHorizontal();



        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        if (playerNameInput.Length >= 1)
        {
            if (GUILayout.Button("Save"))
            {
                requirePlayerName = false;
                PlayerPrefs.SetString("playerName" + Application.platform, playerNameInput);
                PhotonNetwork.playerName = playerNameInput;
                OpenMenu("multiplayer");
            }
        }
        else
        {
            GUILayout.Label("Enter a name to continue...");
        }
        GUILayout.Space(10);
        GUILayout.EndHorizontal();


        GUILayout.EndVertical();
    }
}