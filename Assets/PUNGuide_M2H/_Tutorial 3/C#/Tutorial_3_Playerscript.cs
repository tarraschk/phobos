using UnityEngine;
using System.Collections;

public class Tutorial_3_Playerscript : Photon.MonoBehaviour
{

    public PhotonPlayer owner;

    //Last input value, we're saving this to be able to save network messages/bandwidth.
    private float lastClientHInput = 0;
    private float lastClientVInput = 0;

    //The input values the server will execute on this object
    private float serverCurrentHInput = 0;
    private float serverCurrentVInput = 0;


    void Awake()
    {
        if (PhotonNetwork.isNonMasterClientInRoom)
        {
            // We are probably not the owner of this object: disable this script.
            // RPC's and OnPhotonSerializeView will STILL get trough!
            // The server ALWAYS run this script though
            enabled = false;	 // disable this script (this disables Update());	
        }
    }
    [RPC]
    void SetPlayer(PhotonPlayer player)
    {
        owner = player;
        if (player == PhotonNetwork.player)
        {
            //Hey thats us! We can control this player: enable this script (this enables Update());
            enabled = true;
        }
    }

    void Update()
    {

        //Client code
        if (PhotonNetwork.player == owner)
        {
            //Only the client that owns this object executes this code
            float HInput = Input.GetAxis("Horizontal");
            float VInput = Input.GetAxis("Vertical");

            //Is our input different? Do we need to update the server?
            if (lastClientHInput != HInput || lastClientVInput != VInput)
            {
                lastClientHInput = HInput;
                lastClientVInput = VInput;
                
                SendMovementInput(HInput, VInput); //Use this (and line 62) for simple "prediction"
                photonView.RPC("SendMovementInput", PhotonTargets.MasterClient, HInput, VInput);
                
            }
        }

        //MasterCLient movement code
        //To also enable this on the client itself, use: " if (PhotonNetwork.isMasterClient || PhotonNetwork.player==owner){  "
        if (PhotonNetwork.isMasterClient || PhotonNetwork.player==owner)
        {            
            //Actually move the player using his/her input
            Vector3 moveDirection = new Vector3(serverCurrentHInput, 0, serverCurrentVInput);
            float speed = 5;
            transform.Translate(speed * moveDirection * Time.deltaTime);
        }

        if (!PhotonNetwork.isMasterClient)
        {
			Debug.Log ("Coucou !");
            transform.position = Vector3.Lerp(transform.position, lastReceivedPosition, 0.75f); //"lerp" to the posReceive by 75%
        }

    }


    [RPC]
    void SendMovementInput(float HInput, float VInput)
    {
        //Called on the server
        serverCurrentHInput = HInput;
        serverCurrentVInput = VInput;
    }


    Vector3 lastReceivedPosition;

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //This is executed on the owner of the PhotonView
            //The owner sends it's position over the network
                  
            stream.SendNext(transform.position);//"Encode" it, and send it

        }
        else
        {
            //Executed on all non-owners
            //receive a position and set the object to it

            Vector3 lastReceivedPosition = (Vector3)stream.ReceiveNext();
            
            //We've just recieved the current servers position of this object in 'posReceive'.

            transform.position = lastReceivedPosition;
            //To reduce laggy movement a bit you could comment the line above and use position lerping below instead:	
            //It would be even better to save the last received server position and lerp to it in Update because it is executed more often than OnPhotonSerializeView

        }
    }
}