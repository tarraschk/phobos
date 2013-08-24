using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FPSChat4 : Photon.MonoBehaviour
{
 
    public static bool usingChat = false;	//Can be used to determine if we need to stop player movement since we're chatting
    public GUISkin skin;						//Skin
    public bool showChat = false;			//Show/Hide the chat

    //Private vars used by the script
    private string inputField = "";

    private Vector2 scrollPosition;
    private int width = 500;
    private int height = 180;

    private float lastUnfocus = 0;
    private Rect window;

    private List<FPSChatEntry> chatEntries = new List<FPSChatEntry>();
    public class FPSChatEntry
    {
        public string name = "";
        public string text = "";
    }
    void Awake()
    {
        usingChat = false;

        window = new Rect(Screen.width / 2 - width / 2, Screen.height - height + 5, width, height);
    }

      public void SetShowChatWindow(bool show)
    {
        showChat = show;
        inputField = "";      
        chatEntries = new List<FPSChatEntry>();
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
        //GUI.UnfocusWindow();//Deselect chat
        lastUnfocus = Time.realtimeSinceStartup;
        usingChat = false;
    }




    void OnGUI()
    {
        if (!showChat)
        {
            return;
        }

        GUI.skin = skin;

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
    void GlobalChatWindow(int id)
    {

        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.EndVertical();

        // Begin a scroll view. All rects are calculated automatically - 
        // it will use up any available screen space and make sure contents flow correctly.
        // This is kept small with the last two parameters to force scrollbars to appear.
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (FPSChatEntry entry in chatEntries as List<FPSChatEntry>)
        {
            GUILayout.BeginHorizontal();
            if (entry.name == "")
            {//Game message
                GUILayout.Label(entry.text);
            }
            else
            {
                GUILayout.Label(entry.name + ": " + entry.text);
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
        photonView.RPC("ApplyGlobalChatText", PhotonTargets.All, PhotonNetwork.playerName, msg);
        inputField = ""; //Clear line
    }

    [RPC]
    public void ApplyGlobalChatText(string name, string msg)
    {
        FPSChatEntry entry = new FPSChatEntry();
        entry.name = name;
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
    public void addGameChatMessage(string str)
    {
        ApplyGlobalChatText("", str);
        if (PhotonNetwork.playerList.Length > 0)
        {
            photonView.RPC("ApplyGlobalChatText", PhotonTargets.Others, "", str);
        }
    }
}
