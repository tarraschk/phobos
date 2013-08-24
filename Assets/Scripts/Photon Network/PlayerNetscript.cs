using UnityEngine;
using System.Collections.Generic;


/**
 * 
 * This script manages the network for this player
 * 
*/
public class PlayerNetscript : Photon.MonoBehaviour {
	
	public PhotonPlayer owner;
    Vector3 lastReceivedPosition;
	
	public Vector3 testDestination ;
	
	public Stack<Phobos.dataEntry> netStack = new Stack<Phobos.dataEntry>(); 
	
	public bool stackActive = false ; 
	
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
	
    public void SetPlayer(PhotonPlayer player)
    {
		Debug.Log ("SET PLAYER" + player);
        owner = player;
        if (player == PhotonNetwork.player)
        {
            //Hey thats us! We can control this player: enable this script (this enables Update());
            enabled = true;
			Debug.Log ("enable");
        }
		else {
			Debug.Log ("Disable");
			enabled = false ;
			stackActive = false ; 
		}
    }
	
	void Update()
    {
        //Client code
        if (PhotonNetwork.player == owner)
        {
            //Only the client that owns this object executes this code
			
			if (stackActive) {
				Phobos.dataEntry toDoEntry = this.netStack.Peek(); 
				this.treatDataEntry(toDoEntry); 
			}
        }

    }
	
	private void treatDataEntry(Phobos.dataEntry entry) {
		Debug.Log ("TRAITE ENTRY" + entry.dataCommand);
		switch(entry.dataCommand) {
			case Phobos.Commands.MOVE_TO:
				Debug.Log ("Sending RPC");
                photonView.RPC("netMoveTo", PhotonTargets.Others, entry.dataVector);
			break ;
			
			case Phobos.Commands.ATTACK:
				Debug.Log ("Sending RPC");
                photonView.RPC("netAttack", PhotonTargets.Others, entry.dataTransform);
			break ;
			
			case Phobos.Commands.COLLECT:
				Debug.Log ("Sending RPC");
                photonView.RPC("netCollect", PhotonTargets.Others, entry.dataTransform);
			break ;
		}
		this.stackActive = false ;
	}
	
	public void sendNetMoveTo(Vector3 destination) {
		Phobos.dataEntry newNetEntry = new Phobos.dataEntry(Phobos.Commands.MOVE_TO, null, destination); 
		netStack.Push(newNetEntry); 
		this.testDestination = destination ; 
		this.stackActive = true ; 
	}
	
	public void sendNetAttack(Transform target) {
		Debug.Log ("Net attack ");
		Phobos.dataEntry newNetEntry = new Phobos.dataEntry(Phobos.Commands.ATTACK, target.transform, target.transform.position); 
		netStack.Push(newNetEntry); 
		this.stackActive = true ; 
	}
	
	public void sendNetCollect(Transform target) {
		Debug.Log ("Net collect ");
		Phobos.dataEntry newNetEntry = new Phobos.dataEntry(Phobos.Commands.COLLECT, target.transform, target.transform.position); 
		netStack.Push(newNetEntry); 
		this.stackActive = true ; 
	}
	
	[RPC]
    void netMoveTo(Vector3 destination)
    {
		Debug.Log ("Net move to " + destination);
		ShipController shipController = (ShipController) this.GetComponent(typeof(ShipController));
		shipController.moveTo(destination); 
    }
	
	[RPC]
    void netAttack(GameObject target)
    {
		Debug.Log ("RECEIVED Attack " + target.name);
    }
	
	[RPC]
    void netCollect(GameObject target)
    {
		Debug.Log ("RECEIVED Attack " + target.name);
    }
	
    [RPC]
    void SetPosition(Vector3 newPos)
    {
        //In this case, this function is always ran on the Clients
        //The server requested all clients to run this function (line 25).

        transform.position = newPos;
    }
	
	
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
