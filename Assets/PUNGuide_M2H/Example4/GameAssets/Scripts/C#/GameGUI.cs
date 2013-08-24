using UnityEngine;
using System.Collections;

public class GameGUI : Photon.MonoBehaviour
{
  
    void Awake()
    {

    }
    void OnGUI()
    {

        if (PhotonNetwork.connectionState == ConnectionState.Disconnected)
        {
            //We are currently disconnected: Not a client or host
            GUILayout.Label("Connection status: We've (been) disconnected");
            if (GUILayout.Button("Back to main menu"))
            {
                Application.LoadLevel(Application.loadedLevel-1);
            }

        }
        else
        {
            //We've got a connection(s)!


            if (PhotonNetwork.connectionState == ConnectionState.Connecting)
            {

                GUILayout.Label("Connection status: Connecting");

            }
            else 
            {

                GUILayout.Label("Connection status: Connected");
                GUILayout.Label("Players: " + PhotonNetwork.playerList.Length);
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing());
                
            }

            if (GUILayout.Button("Disconnect"))
            {
                PhotonNetwork.Disconnect();
            }
        }


    }

    //CLient function
    void OnDisconnectedFromPhoton()
    {
        Debug.Log("This CLIENT has disconnected from a server");
    }
    //Server functions called by Unity
    void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("Player connected from: " + player);
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("Player disconnected from: " + player);

    }


}