
using UnityEngine;
using System.Collections;

public class Example2_Gamescript : Photon.MonoBehaviour
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
                Application.LoadLevel(Application.loadedLevel - 1);
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

                GUILayout.Label("Conected.");
                GUILayout.Label("Players: " + PhotonNetwork.playerList.Length);
                GUILayout.Label("Ping to server " + PhotonNetwork.GetPing());

            }

            if (GUILayout.Button("Disconnect"))
            {
                PhotonNetwork.Disconnect();
            }
        }


    }


    void OnDisconnectedFromPhoton()
    {
        Debug.Log("Lost connection to the server");

    }



    void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("Player connected: " + player);
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("Player disconnected: " + player);

    }




}