using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// Basic GUI to show traffic and health statistics of the connection to Photon, 
/// toggled by shift+tab.
/// </summary>
/// <remarks>
/// The shown health values can help identify problems with connection losses or performance.
/// Example: 
/// If the time delta between two consecutive SendOutgoingCommands calls is a second or more,
/// chances rise for a disconnect being caused by this (because acknowledgements to the server
/// need to be sent in due time).
/// </remarks>
/// \ingroup optionalGui
public class PhotonStatsTracker : MonoBehaviour
{
    public string version = "0.4";  // used as "format" value, too
    //0.3:
    // replaced "Total Command Bytes" with "Total Messages" (in and out)
    //0.4:  
    // resetting the "longest" and "max" game level stats, to get a max per interval
    // added "format" value (containing the version of this tracker)
    // added send/dispatch calls
    // changed format for saved session in PlayerPrefs
    // cleaned up some code of tracker. timing, saving prefs, etc.

    /// <summary>Option to turn collecting stats on or off (used in Update()).</summary>
    public bool TrackingEnabled = true;

    /// <summary>Interval for statistics tracking in Milliseconds.</summary>
    public int TrackingInterval = 2000;

    /// <summary>Limits the tracking history to a certain number of entries).</summary>
    public int TrackingQueueLimit = 150;

    /// <summary>Automatically send tracked stats on connection timeout.</summary>
    public bool SendStatisticsOnTimeout = true;

    /// <summary>Automatically send tracked stats on application quit.</summary>
    public bool SendStatisticsOnExit = true;

    /// <summary>Shows or hides GUI (does not affect if stats are collected).</summary>
    public bool StatisticsButton = false;

    /// <summary>Positioning rect for window.</summary>
    public Rect ButtonPosRect = new Rect(0, 100, 200, 50);

    /// <summary>Saves current session to PlayerPrefs every Nth interval. Use 0 to disable.</summary>
    public int SaveToPrefsIntervals = 5;
    
    public string outputFileName = "stats_output.txt";


    #region internal values


    private const string AnalyticsWebSubmitUrl = "http://dev-clientstats-service.exitgames.com/data/publish/";

    private const string AnalyticsWebViewUrl = "";

    private const string PlayerPrefsTrackerKey = "StatsTackerSession";

    Stopwatch stopwatch = new Stopwatch();

    private int nextTimeToTrack;

    /// <summary>Thread for statistics tracking</summary>
    private Thread trackingThread;

    /// <summary>Queues for storing statistics data</summary>
    private Queue<int[]> statsDataQueue;
    private Queue<int[]> incomingPacketsStatsQueue;
    private Queue<int[]> outgoingPacketsStatsQueue;
    private Dictionary<int, string> trackedEvents;

    /// <summary>Field names for statistics data</summary>
    private string[] statsParamsLabels = new string[]
                                             {
                                                 "Time",
                                                 "Ping",
                                                 "Send delta",
                                                 "Dispatch delta",
                                                 "Max Ev-Exec Time",
                                                 "Max Ev-Exec EvCode",
                                                 "Max Response-Exec Time",
                                                 "Max Response-Exec OpCode",
                                                 "Resent Count"
                                             };

    /// <summary>Field names for packets data</summary>
    private string[] packetStatsParamsLabels = new string[]
                                                   {
                                                       "Time",
                                                       "Total Packet Bytes",
                                                       "Total Messages",
                                                       "Total Packet Count",
                                                       "Total Commands in Packets"
                                                   };

    /// <summary>
    /// Time intervals for current stats measurement
    /// </summary>
    private DateTime trackingIntervalStartTime;

    private DateTime trackingIntervalEndTime;
    
    private bool saveStatsToPlayerPrefs = false;
    
    private int startTickCount; // checking if SupportClass.GetTickCount() does as it should

    private int unsavedTrackingsCount = 0;

    #endregion


    public void Start()
    {
        PhotonNetwork.NetworkStatisticsEnabled = true;
        this.startTickCount = SupportClass.GetTickCount();

        statsDataQueue = new Queue<int[]>();
        incomingPacketsStatsQueue = new Queue<int[]>();
        outgoingPacketsStatsQueue = new Queue<int[]>();
        trackedEvents = new Dictionary<int, string>();

        if (!Application.isEditor && PlayerPrefs.HasKey(PlayerPrefsTrackerKey))
        {
            Debug.Log("Found StatsTracker session in PlayerPrefs. Sending it now as 'crashed'.");

            string savedSession = PlayerPrefs.GetString(PlayerPrefsTrackerKey);
            SendStatsViaWWW(savedSession, true);
        }

        this.SaveStatsToPlayerPrefs(AllStatsToJavaScript());
    }

    /// <summary>Checks for left-shift+tab input combination (to toggle TrafficStatsEnabled).</summary>
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            this.StatisticsButton = !this.StatisticsButton;
            this.TrackingEnabled = true; // enable stats when showing the window
        }

        if (TrackingEnabled)
        {
            if (trackingThread == null)
            {
                trackingThread = new Thread(this.StatsThreadDelegate);
                trackingThread.IsBackground = true;
                trackingThread.Start();
            }
        }
        else
        {
            if (trackingThread != null)
            {
                trackingThread.Abort();
                trackingThread = null;
            }
        }
        
        if (PhotonNetwork.networkingPeer.TrafficStatsEnabled != TrackingEnabled)
        {
            PhotonNetwork.networkingPeer.TrafficStatsEnabled = this.TrackingEnabled;
        }

        if (SaveToPrefsIntervals != 0 && unsavedTrackingsCount >= SaveToPrefsIntervals)
        {
            unsavedTrackingsCount = 0;
            SaveStatsToPlayerPrefs(AllStatsToJavaScript());
        }
    }

    public void OnGUI()
    {
        if (!this.StatisticsButton)
        {
            return;
        }

        GUILayout.BeginArea(this.ButtonPosRect);
        GUILayout.Label("Tracking Stats");
        if (this.TrackingEnabled)
        {
            if (GUILayout.Button("Send Now"))
            {
                string stats = AllStatsToJavaScript();
                SendStatsViaWWW(stats, false);
            }

            if (GUILayout.Button("Disable"))
            {
                this.TrackingEnabled = false;
            }
        }
        else
        {
            if (GUILayout.Button("Enable"))
            {
                this.TrackingEnabled = true;
            }
        }
        GUILayout.EndArea();
    }

    /// <summary>
    /// Save statistics values after interval specified in TrackingInterval
    /// </summary>
    private void StatsThreadDelegate()
    {
        nextTimeToTrack = TrackingInterval;
        TrafficStatsGameLevel gls = PhotonNetwork.networkingPeer.TrafficStatsGameLevel;
        stopwatch.Start();

        while (true)
        {
            int elapsedMilliseconds = (int)stopwatch.ElapsedMilliseconds;
            int deltaToNextTimeToTrack = nextTimeToTrack - elapsedMilliseconds;

            // sleep only if next track should happen at least 15ms later
            if (deltaToNextTimeToTrack > 15)
            {
                Thread.Sleep(deltaToNextTimeToTrack);
                continue;
            }

            nextTimeToTrack = (int)elapsedMilliseconds + TrackingInterval;
            unsavedTrackingsCount++;

            while (statsDataQueue.Count > TrackingQueueLimit)
            {
                lock (statsDataQueue)
                {
                    statsDataQueue.Dequeue();
                }
                lock (incomingPacketsStatsQueue)
                {
                    incomingPacketsStatsQueue.Dequeue();
                }
                lock (outgoingPacketsStatsQueue)
                {
                    outgoingPacketsStatsQueue.Dequeue();
                }
            }

            statsDataQueue.Enqueue(new int[]
                    {
                        (int) elapsedMilliseconds,
                        PhotonNetwork.GetPing(),
                        gls.LongestDeltaBetweenSending,
                        gls.LongestDeltaBetweenDispatching,
                        gls.LongestEventCallback,
                        gls.LongestEventCallbackCode,
                        gls.LongestOpResponseCallback,
                        gls.LongestOpResponseCallbackOpCode,
                        PhotonNetwork.ResentReliableCommands
                    });

            incomingPacketsStatsQueue.Enqueue(new int[]
                    {
                        (int) elapsedMilliseconds,
                        PhotonNetwork.networkingPeer.TrafficStatsIncoming.TotalPacketBytes,
                        PhotonNetwork.networkingPeer.TrafficStatsGameLevel.TotalIncomingMessageCount,
                        PhotonNetwork.networkingPeer.TrafficStatsIncoming.TotalPacketCount,
                        PhotonNetwork.networkingPeer.TrafficStatsIncoming.TotalCommandsInPackets
                    });

            outgoingPacketsStatsQueue.Enqueue(new int[]
                    {
                        (int) elapsedMilliseconds,
                        PhotonNetwork.networkingPeer.TrafficStatsOutgoing.TotalPacketBytes,
                        PhotonNetwork.networkingPeer.TrafficStatsGameLevel.TotalOutgoingMessageCount,
                        PhotonNetwork.networkingPeer.TrafficStatsOutgoing.TotalPacketCount,
                        PhotonNetwork.networkingPeer.TrafficStatsOutgoing.TotalCommandsInPackets
                    });

            gls.ResetMaximumCounters(); // makes us look up "longest delta" values of the past interval (instead of absolute max)
            Debug.Log("nextTimeToTrack: " + nextTimeToTrack);
        }
    }

    public void TrackEvent(string eventTxt)
    {
        int id = (int) stopwatch.ElapsedMilliseconds;
        if (trackedEvents.ContainsKey(id))
        {
            trackedEvents[id] += "," + eventTxt;
        }
        else
        {
            trackedEvents.Add(id, eventTxt);
        }
    }


    // // format for output
    //{
    //    "appid": <appid>,
    //    "timeutc": (long)UtcUnixTimestamp(),
    //    "version": <game version>,
    //    "format": <tracker format/version>,
    //    "client": [["Time", "Send delta", "Dispatch delta", "Event callback", "Event callback code", "Event response callback", "Event response callback code"],
    //    [1000, 62, 32, 0, 0, 0, 0],
    //    [2000, 63, 32, 15, 255, 0, 0],
    //    [3000, 78, 32, 15, 255, 0, 0]],
    //    "in": ...
    //}

    // TODO: make this more dynamic. at the moment, ",\n" is manually formatted as postfix to each datapiece
    private string AllStatsToJavaScript()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{");
        sb.AppendFormat("\"appid\":\"{0}\",\n\"timeutc\":{1},\n\"version\":\"{2}\",\n\"format\":\"{3}\",\n", PhotonNetwork.PhotonServerSettings.AppID, UtcUnixTimestamp(), PhotonNetwork.networkingPeer.mAppVersion, this.version);
        sb.AppendFormat("\"client\":{0},\n", ConvertToJavaScript(statsDataQueue, statsParamsLabels));
        sb.AppendFormat("\"in\":{0},\n", ConvertToJavaScript(incomingPacketsStatsQueue, packetStatsParamsLabels));
        sb.AppendFormat("\"out\":{0},\n", ConvertToJavaScript(outgoingPacketsStatsQueue, packetStatsParamsLabels));
        sb.AppendFormat("\"events\":{0}", ConvertToJavaScript(trackedEvents));
        sb.Append("}");
        return sb.ToString();
    }

    // print out: [[timestamp, event-string], ...]
    private object ConvertToJavaScript(Dictionary<int, string> trackedEvents)
    {
        if (trackedEvents == null || trackedEvents.Count < 1)
        {
            return "[]";
        }

        StringBuilder output = new StringBuilder();

    
        output.Append("[");

        int i = 0;
        foreach (KeyValuePair<int, string> trackedEvent in trackedEvents)
        {
            output.AppendFormat("[{0},\"{1}\"]", trackedEvent.Key, trackedEvent.Value);
            if (i != trackedEvents.Count - 1)
            {
                output.Append(",");
            }
            i++;
        }

        output.Append("]");

        return output.ToString();
    }

    // print out: [[labels],[valueset],[valueset], ...]
    private string ConvertToJavaScript(Queue<int[]> packetsStatsQueue, string[] paramsLabels)
    {
        if (packetsStatsQueue == null || packetsStatsQueue.Count <= 0)
        {
            return string.Empty;
        }

        StringBuilder output = new StringBuilder();
        output.Append("[");
        lock (packetsStatsQueue)
        {
            output.Append(ToJsArray(paramsLabels));
            output.Append(",\n");
            
            int i = 0;
            foreach (int[] data in packetsStatsQueue)
            {
                output.Append(ToJsArray(data));
                if (i != packetsStatsQueue.Count - 1)
                {
                    output.Append(",\n");
                }
                i++;
            }
        }
        output.Append("]");
        return output.ToString();
    }


    public void OnApplicationQuit()
    {
        if (trackingThread != null)
        {
            try
            {
                trackingThread.Abort();
            }
            catch
            {
            }
        }

        if (SendStatisticsOnExit)
        {
            TrackEvent("OnApplicationQuit");
            if (startTickCount == SupportClass.GetTickCount())
            {
                TrackEvent("SupportClass.GetTickCount() not updated since start. Is: " + SupportClass.GetTickCount());
            }

            SendStatsViaWWW(AllStatsToJavaScript(), false);
        }

        PlayerPrefs.DeleteKey(PlayerPrefsTrackerKey);
    }

    public void OnDisconnectedFromPhoton()
    {
        TrackEvent("OnDisconnectedFromPhoton");
    }

    public void OnConnectedToPhoton()
    {
        TrackEvent("ConnectedToPhoton");
    }

    public void OnJoinedRoom()
    {
        TrackEvent("OnJoinedRoom");
    }

    public void OnConnectionFail(DisconnectCause disconnectCause)
    {
        TrackEvent(disconnectCause.ToString());
        if (disconnectCause == DisconnectCause.TimeoutDisconnect && SendStatisticsOnTimeout)
        {
            SendStatsViaWWW(AllStatsToJavaScript(), false);
        }
    }


    public static string ToJsArray(string[] txts)
    {
        return "[\"" + string.Join("\",\"", txts) + "\"]";
    }

    public static string ToJsArray(int[] values)
    {
        if (values == null || values.Length < 1)
        {
            return "[]";
        }

        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        for (int i = 0; i < values.Length - 1; i++)
        {
            sb.AppendFormat("{0},", values[i]);
        }
        sb.AppendFormat("{0}]", values[values.Length-1]);
        return sb.ToString();
    }

    public long UtcUnixTimestamp()
    {
        TimeSpan _UnixTimeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        return (long)_UnixTimeSpan.TotalSeconds;
    }

    /// <summary>
    /// Send statistics to server
    /// </summary>
    /// <param name="stringToSend">Statistics data</param>
    void SendStatsViaWWW(string stringToSend, bool crash)
    {
        // Create a POST request with statistics data
        WWWForm statsForm = new WWWForm();
        statsForm.AddField("applicationId", PhotonNetwork.PhotonServerSettings.AppID);
        statsForm.AddField("data", stringToSend);
        if (crash)
        {
            statsForm.AddField("crash", 1);
        }
        
        WWW www = new WWW(AnalyticsWebSubmitUrl, statsForm);
        
        Debug.Log("Sent tracked statistics:\n" + stringToSend);
    }

    private void SaveStatsToPlayerPrefs(string stringToSave)
    {
        PlayerPrefs.SetString(PlayerPrefsTrackerKey, stringToSave);
    }

    /// <summary>Writes data to text file for test purposes</summary>
    private void AddStringToOutputFile(string appendedString)
    {
        #if !UNITY_WEBPLAYER
        using (StreamWriter writer = File.AppendText(outputFileName))
        {
            writer.WriteLine(appendedString);
            writer.WriteLine("");
            writer.Close();
        }
        Debug.Log("Data was saved to the file.");
        #endif
    }
}
