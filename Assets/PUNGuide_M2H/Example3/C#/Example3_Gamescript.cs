using UnityEngine;
using System.Collections;

public class Example3_Gamescript : Photon.MonoBehaviour
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
                Application.LoadLevel((Application.loadedLevel - 1));
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

                GUILayout.Label("Connection status: Server!");
                GUILayout.Label("Connections: " + PhotonNetwork.playerList.Length);
                if (PhotonNetwork.playerList.Length >= 1)
                {
                    GUILayout.Label("Ping to first player: " + PhotonNetwork.GetPing());
                }
            }

            if (GUILayout.Button("Disconnect"))
            {
                PhotonNetwork.Disconnect();
            }
        }

    }

    //Client&Server
    void OnDisconnectedFromPhoton()
    {
        Debug.Log("Lost connection to the server");

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