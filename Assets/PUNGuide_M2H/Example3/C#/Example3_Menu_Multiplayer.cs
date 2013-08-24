using UnityEngine;
using System.Collections;

public class Example3_Menu_Multiplayer : MonoBehaviour
{
   

    private bool showMenu = false;
    private Rect myWindowRect;

    void Awake()
    {
        myWindowRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 200);
    }

    public void EnableMenu()
    {
        showMenu = true;
    }

    void OnGUI()
    {
        if (!showMenu)
        {
            return;
        }
        myWindowRect = GUILayout.Window(0, myWindowRect, windowGUI, "Multiplayer");
    }
    void windowGUI(int id)
    {

        GUILayout.Space(10);

        if (!PhotonNetwork.connected)
        {
            GUILayout.Label("Not connected");
            if (GUI.Button(new Rect(50, 60, 200, 20), "Retry"))
            {
                PhotonNetwork.ConnectUsingSettings("1.0");
            }
        }
        else
        {

            if (GUI.Button(new Rect(50, 60, 200, 20), "Create a game"))
            {
                showMenu = false;
                Example3_Menu.SP.OpenMenu("multiplayer-host");
            }

            if (GUI.Button(new Rect(50, 90, 200, 20), "Select a game to join"))
            {
                showMenu = false;
                Example3_Menu.SP.OpenMenu("multiplayer-join");
            }
        }

    }
}
