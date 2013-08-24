using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawn : Photon.MonoBehaviour
{
   

    public Transform playerPrefab;
    private List<PlayerNetscript> playerScripts = new List<PlayerNetscript>();

    void OnCreatedRoom()
    {
        //Spawn a player for the server itself
        Spawnplayer(PhotonNetwork.player);
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        
        if (PhotonNetwork.isMasterClient)
        {
            //A player connected to me(the MasterClient), spawn a player for it:
            Spawnplayer(newPlayer);
        }
    }


    void Spawnplayer(PhotonPlayer newPlayer)
    {
        //Called on the MasterClient only

        //Instantiate a new object for this player, remember; the server is therefore the owner.
        Transform myNewTrans = PhotonNetwork.Instantiate(playerPrefab.name, transform.position, transform.rotation, 0).transform;

        //Get the networkview of this new transform
        PhotonView newObjectsview = myNewTrans.GetComponent<PhotonView>();

        PlayerNetscript playerScript = myNewTrans.GetComponent<PlayerNetscript>();
        //Keep track of this new player so we can properly destroy it when required.
        
        playerScript.owner = newPlayer;
        playerScripts.Add(playerScript);
        
        //Call an RPC on this new PhotonView, set the PhotonPlayer who controls this new player
        newObjectsview.RPC("SetPlayer", PhotonTargets.AllBuffered, newPlayer);//Set it on the owner
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("Clean up after player " + player);

        for(int i =playerScripts.Count-1;i>=0;i--){
            PlayerNetscript script = playerScripts[i];        
            if (player == script.owner)
            {
                //We found the players object
                if (PhotonNetwork.isMasterClient)
                {
                    PhotonNetwork.Destroy(script.gameObject);
                }
                playerScripts.Remove(script);//Remove this player from the list                
                break;
            }
        }
    }
      

    void OnMasterClientSwitched(PhotonPlayer newMaster)
    {
        /* We have a design problem in this tutorial: ONLY the masterclient maintains the list of which 
         * PlayerScripts belongs to which PhotonPlayer, thus if the MasterClient leaves we cannot recover this.
         * This problem would be easy enough to solve by having everyone maintain this list. However I wanted to
         * keep this tutorial clear and to the point.
         */

        //Abort, abort...
        if(!newMaster.isLocal)
            PhotonNetwork.Disconnect();

        //After disconnection the scene will reload via OnDisconnectedFromPhoton.
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.Log("Resetting the scene the easy way.");
        Application.LoadLevel(Application.loadedLevel);
    }

}