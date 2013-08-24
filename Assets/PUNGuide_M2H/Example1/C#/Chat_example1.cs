using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chat_example1 : Photon.MonoBehaviour
{
    public static Chat_example1 SP;
    public static bool usingChat = false;	//Can be used to determine if we need to stop player movement since we're chatting
    
    public bool showChat = false;			//Show/Hide the chat

    
    //Private vars used by the script
    private string inputField = "";

    private Vector2 scrollPosition;
    private int width = 500;
    private int height = 185;
    private Rect window;
    private float lastUnfocus = 0;


    private List<ChatEntry> chatEntries = new List<ChatEntry>();
    public class ChatEntry
    {
        public string name = "";
        public string text = "";

  
    }

    void Awake()
    {
        window = new Rect(Screen.width / 2 - width / 2, Screen.height - height + 5, width, height);
        SP = this;

        //We get the name from the masterserver example, if you entered your name there ;).
        string playerName = PlayerPrefs.GetString("playerName"+Application.platform, "");
        if (playerName == null || playerName == "")
        {
            playerName = "RandomName" + Random.Range(1, 999);
        }
        PhotonNetwork.playerName = playerName;

    }


    //Client function
    void OnJoinedRoom()
    {
        SetShowChatWindow(true);
        AddGameChatMessage(PhotonNetwork.player.name + " joined the chat", true);
    }



    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        if (PhotonNetwork.isMasterClient)
        {
            AddGameChatMessage("Player disconnected: " + player, true);
        }
    }

    void OnDisconnectedFromPhoton()
    {
        SetShowChatWindow(false);
    }


    public void SetShowChatWindow(bool show)
    {
        showChat = show;
        inputField = "";
        chatEntries = new List<ChatEntry>();
    }

    void OnGUI()
    {
        if (!showChat || PhotonNetwork.room == null)
        {
            return;
        }


        if (Event.current.type == EventType.keyDown && Event.current.character == '\n')//inputField.Length <= 0)
        {
            if (lastUnfocus + 0.25f < Time.realtimeSinceStartup)
            {
                if (!usingChat)
                    StartCoroutine(StartUsingChat());
                else
                    StopUsingChat();
            }
        }
        if (usingChat && Input.GetKey(KeyCode.Escape))
        {
            StopUsingChat();
        }
        
        window = GUI.Window(5, window, GlobalChatWindow, "");
    }

    IEnumerator StartUsingChat()
    {
        usingChat = true;
        yield return 0;
        //GUI.FocusWindow(1);
        //GUI.FocusControl("Chat input field");
    }
    void StopUsingChat()
    {
        inputField = "";
        GUI.UnfocusWindow();//Deselect chat
        lastUnfocus = Time.realtimeSinceStartup;
        usingChat = false;
    }


    void GlobalChatWindow(int id)
    {

        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.EndVertical();

        // Begin a scroll view. All rects are calculated automatically - 
        // it will use up any available screen space and make sure contents flow correctly.
        // This is kept small with the last two parameters to force scrollbars to appear.
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (var entry in chatEntries)
        {
            GUILayout.BeginHorizontal();
            if ((entry as ChatEntry).name == "")
            {//Game message
                GUILayout.Label((entry as ChatEntry).text);
            }
            else
            {
                GUILayout.Label((entry as ChatEntry).name + ": " + (entry as ChatEntry).text);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(3);

        }
        // End the scrollview we began above.
        GUILayout.EndScrollView();


        if (Event.current.type == EventType.keyDown && Event.current.character == '\n' && inputField.Length > 0)
        {
            HitEnter(inputField);
        }
        
        if (usingChat)
        {
            GUI.SetNextControlName("Chat input field");
            inputField = GUILayout.TextField(inputField, GUILayout.Height(24));

            if (GUI.GetNameOfFocusedControl() != "Chat input field")
            {
                //GUI.FocusWindow(1);
                GUI.FocusControl("Chat input field");
            }
        }
        else
        {
            GUILayout.Space(23 + 5);
        }
        //If we DO have typing focus, make sure Chat.usingchat is set!
        if (GUI.changed && !usingChat)
        {
            StartCoroutine(StartUsingChat());
        }

        //GUI.DragWindow ();

        if (Input.GetKeyDown("mouse 0"))
        {
            if (usingChat)
            {
                StopUsingChat();
            }
        }
    }

    void HitEnter(string msg)
    {
        msg = msg.Replace("\n", "");
        photonView.RPC("ApplyGlobalChatText", PhotonTargets.All, msg, false);
        inputField = ""; //Clear line
    }

    
    [RPC]
    public void ApplyGlobalChatText(string msg, bool systemMessage, PhotonMessageInfo info)
    {
        ChatEntry entry = new ChatEntry();
        if(!systemMessage) entry.name = info.sender.name;
        entry.text = msg;

        chatEntries.Add(entry);

        //Remove old entries
        if (chatEntries.Count > 4)
        {
            chatEntries.RemoveAt(0);
        }

        scrollPosition.y = 1000000;
    }

    //Add game messages etc
    public void AddGameChatMessage(string str, bool systemMessage)
    {
        photonView.RPC("ApplyGlobalChatText", PhotonTargets.All, str, systemMessage);
    }


  
}
