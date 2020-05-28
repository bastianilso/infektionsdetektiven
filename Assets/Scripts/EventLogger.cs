using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;

public class EventLogger : MonoBehaviour
{

    private Dictionary<string, Dictionary<int, string>> metalogs = new Dictionary<string, Dictionary<int, string>>();
    private Dictionary<string, Dictionary<int, string>> logs = new Dictionary<string, Dictionary<int, string>>();

    private Dictionary<string, Dictionary<int, object>> gamelogs = new Dictionary<string, Dictionary<int, object>>();

    [SerializeField]
    private string savePath = "";

    [SerializeField]
    private string fileName = "log";

    [SerializeField]
    private bool saveLocally = true;

    private string filePath;
    private char fieldSeperator = ',';
    private string uid = "";
    private int logCount = 0;
    private int gamelogCount = 0;

    // Start is called before the first frame update
    void Awake()
    {
        logs.Add("GameID", new Dictionary<int, string>());
        logs.Add("TimeStamp", new Dictionary<int, string>());

        //logs.Add("GameTime", new Dictionary<int, string>());
        //logs.Add("GameDay", new Dictionary<int, string>());
        //logs.Add("NumberOfInfected", new Dictionary<int, string>());

        metalogs.Add("GameID", new Dictionary<int, string>());
        metalogs.Add("Timestamp", new Dictionary<int, string>());
        metalogs.Add("SamplingRate", new Dictionary<int, string>());
    }

    public void GenerateUID(int participantId, int testId) {
        uid = participantId.ToString() + testId.ToString() + System.DateTime.Now.ToString("yyyy:MM:dd:HH:mm:ss").Replace(" ", "").Replace("/", "").Replace(":", "");
    }

    public Dictionary<string, Dictionary<int, object>> GetGameLogs() {
        return gamelogs;
    }

    // Converts the values of the parameters (in a "object format") to a string, formatting them to the
    // correct format in the process.
    private string ConvertToString(object arg)
    {
        if (arg is float)
        {
            return ((float)arg).ToString("0.0000").Replace(",", ".");
        }
        else if (arg is Vector3)
        {
            return ((Vector3)arg).ToString("0.0000").Replace(",", ".");
        }
        else
        {
            return arg.ToString();
        }
    }

    // Returns a time stamp including the milliseconds.
    private string GetTimeStamp()
    {
        return System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff");
    }

    // Generates a "logs" row (see class description) from the given datas. Adds mandatory parameters and 
    // the PersistentEvents parameters to the row when generating it.
    public void GenerateLine(Dictionary<string, object> log) {
        logs["TimeStamp"].Add(logCount, GetTimeStamp());
        logs["GameID"].Add(logCount, uid);

        foreach (KeyValuePair<string, object> pair in log)
        {
            if (logs.ContainsKey(pair.Key))
            {
                logs[pair.Key].Add(logCount, ConvertToString(pair.Value));
            }
            else
            {
                logs.Add(pair.Key, new Dictionary<int, string>{{logCount, ConvertToString(pair.Value)}});
            }
        }
        logCount++;
    }

    public void AddToGameLog(Dictionary<string, object> log) {
        foreach (KeyValuePair<string, object> pair in log)
        {
            if (gamelogs.ContainsKey(pair.Key))
            {
                gamelogs[pair.Key].Add(gamelogCount, pair.Value);
            }
            else
            {
                gamelogs.Add(pair.Key, new Dictionary<int, object>{{gamelogCount, pair.Value}});
            }
        }
        gamelogCount++;
    }

    private void GenerateMetaLog() {
        metalogs["GameID"].Add(logCount, uid);
        metalogs["TimeStamp"].Add(logCount, GetTimeStamp());
        metalogs["SamplingRate"].Add(logCount, GetTimeStamp());
    }

    // Generates the headers in a CSV format and saves them to the CSV file
    private void GenerateHeaders(Dictionary<string, Dictionary<int, string>> logs)
    {
        string headers = "";
        foreach (string key in logs.Keys)
        {
            if (headers != "")
            {
                headers += fieldSeperator;
            }
            headers += key;
        }
        SaveToFile(headers);
    }

    // Saves the given CSV line to the CSV file.
    private void SaveToFile(string line, bool end = true)
    {
        string tempLine = line;

        if (end)
        {
            tempLine += Environment.NewLine;
        }
        File.AppendAllText(filePath, tempLine);
    }

}
