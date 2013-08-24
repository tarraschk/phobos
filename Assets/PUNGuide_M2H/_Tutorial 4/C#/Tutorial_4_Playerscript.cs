using UnityEngine;
using System.Collections;

public class Tutorial_4_Playerscript : Photon.MonoBehaviour {


//This is mostly copied from tut 2B 

void Update (){
	
	if(photonView.isMine){
		//Only the owner can move the cube!	
		//(Ok this check is a bit overkill since we did already disable the script in Awake)		
		Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		float speed = 5;
		transform.Translate(speed * moveDirection * Time.deltaTime);
	}
	
}
void OnPhotonSerializeView ( PhotonStream stream ,   PhotonMessageInfo info  ){
	if (stream.isWriting){
        //Executed on the owner of this PhotonView; 
		//The server sends it's position over the network
		
		stream.SendNext(transform.position);
				
	}else{
		//Executed on the others; 
		//receive a position and set the object to it
		
        transform.position = (Vector3)stream.ReceiveNext();
		
	}
}
}