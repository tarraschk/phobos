using UnityEngine;
using System.Collections;

public class PlayerScript : Photon.MonoBehaviour
{   
    public PhotonView rigidBodyView;
    public int hp = 100;
    public ScoreBoard theScoreBoard;
    public bool localPlayer = false;

    public Material metalMaterial;
    private Material orgMaterial;
    private float coloredUntill;
    private bool invincible;


    void Awake()
    {
        orgMaterial = renderer.material;

        theScoreBoard = ScoreBoard.SP;
    }

    void OnPhotonInstantiate(PhotonMessageInfo msg)
    {
        // This is our own player
        if (photonView.isMine)
        {
            //camera.main.enabled=false;
            
            localPlayer = true;
         
            Destroy(GameObject.Find("LevelCamera"));

            Machinegun gun = transform.GetComponentInChildren < Machinegun>();
            gun.localPlayer = true;



        }
        // This is just some remote controlled player, don't execute direct
        // user input on this. DO enable multiplayer controll
        else
        {            
            name += msg.sender.name; 

            transform.Find("CrateCamera").gameObject.active = false;

            FPSWalker4 tmp2 = GetComponent<FPSWalker4>() as FPSWalker4;
            tmp2.enabled = false;
            MouseLook tmp5 = GetComponent<MouseLook>() as MouseLook;
            tmp5.enabled = false;
        }
    }

    void OnGUI()
    {
        if (localPlayer)
        {
            GUILayout.Label("HP: " + hp);
        }
    }


    [RPC]
    IEnumerator StartInvincibility()
    {
        invincible = true;
        renderer.material = metalMaterial;

        yield return new WaitForSeconds(10);

        renderer.material = orgMaterial;
        invincible = false;
    }


    void ApplyDamage(string[] info)
    {
        float damage = float.Parse(info[0]);
        //string killerName = info[1];

        if (invincible)
        {
            return;
        }

        hp -= (int)damage;
        if (hp < 0)
        {
            theScoreBoard.LocalPlayerHasKilled();
            photonView.RPC("Respawn", PhotonTargets.All);
        }
        else
        {
            photonView.RPC("setHP", PhotonTargets.Others, hp);
        }
    }


    [RPC]
    void setHP(int newHP)
    {
        hp = newHP;
    }

    [RPC]
    void Respawn()
    {
        if (photonView.isMine)
        {
            theScoreBoard.LocalPlayerDied();

            // Randomize starting location
            GameObject[] spawnpoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
            Transform spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Length)].transform;

            transform.position = spawnpoint.position;
            transform.rotation = spawnpoint.rotation;
        }
        hp = 100;
    }



}