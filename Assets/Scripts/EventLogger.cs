using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;

public enum EventType {
    GameEvent,
    SubjectEvent,
    PlayerEvent,
}

public class EventLogger : MonoBehaviour
{

    private Dictionary<string, string> statelogs = new Dictionary<string, string>();
    private Dictionary<string, Dictionary<int, string>> logs = new Dictionary<string, Dictionary<int, string>>();

    private Dictionary<string, Dictionary<int, object>> gamelogs = new Dictionary<string, Dictionary<int, object>>();

    [SerializeField]
    private string savePath = "";

    [SerializeField]
    private string fileName = "log";

    [SerializeField]
    private bool saveLocally = true;

    [SerializeField]
    private ConnectToMySQL connectToMySQL;

    private string filePath;
    private string completeFileName = "";
    private char fieldSeperator = ',';
    private string uid = "";
    private int logCount = 0;
    private int gamelogCount = 0;
    private string email = "anonymous";
    private bool saveHeaders = true;

    // Start is called before the first frame update
    void Awake()
    {
        initLog();
        statelogs.Add("PlayID", "-1");
        statelogs.Add("DeviceID", "-1");
        statelogs.Add("NumberOfHealthy", "-1");
        statelogs.Add("NumberOfIsolated", "-1");
        statelogs.Add("NumberOfInfected", "-1");
        statelogs.Add("NumberOfTested", "-1");
        statelogs.Add("GameState", "-1");
        statelogs.Add("SubjectsOnStart", "-1");
        statelogs.Add("InfectedOnStart", "-1");
        statelogs.Add("GameOverScore", "-1");
        statelogs.Add("GameWonScore", "-1");
        statelogs.Add("NewInfectionSeconds", "-1");
        statelogs.Add("LevelNo", "-1");
        GenerateUID();
        InitFile();

        statelogs["PlayID"] = uid;
        statelogs["DeviceID"] = SystemInfo.deviceUniqueIdentifier;
    }

    private void initLog() {
        logs.Add("PlayID", new Dictionary<int, string>());
        logs.Add("DeviceID", new Dictionary<int, string>());
        logs.Add("Timestamp", new Dictionary<int, string>());
        logs.Add("Event", new Dictionary<int, string>());
        logs.Add("EventType", new Dictionary<int, string>());
        logs.Add("GameTime", new Dictionary<int, string>());
        logs.Add("NumberOfHealthy", new Dictionary<int, string>());
        logs.Add("NumberOfIsolated", new Dictionary<int, string>());
        logs.Add("NumberOfInfected", new Dictionary<int, string>());
        logs.Add("NumberOfTested", new Dictionary<int, string>());
        logs.Add("GameState", new Dictionary<int, string>());
        logs.Add("SubjectsOnStart", new Dictionary<int, string>());
        logs.Add("InfectedOnStart", new Dictionary<int, string>());
        logs.Add("GameOverScore", new Dictionary<int, string>());
        logs.Add("GameWonScore", new Dictionary<int, string>());
        logs.Add("NewInfectionSeconds", new Dictionary<int, string>());
    }

    private void InitFile()
    {
        completeFileName = fileName + "_" + GetTimeStamp().Replace('/', '-').Replace(":", "-");
        filePath = savePath + "/" + completeFileName + ".csv";
    }

    public void GenerateUID() {
        uid = Md5Sum(System.DateTime.Now.ToString(SystemInfo.deviceUniqueIdentifier + "yyyy:MM:dd:HH:mm:ss.ffff").Replace(" ", "").Replace("/", "").Replace(":", ""));
    }

    public Dictionary<string, Dictionary<int, object>> GetGameLogs() {
        return gamelogs;
    }

    // Generates a "logs" row (see class description) from the given datas.
    public void GenerateLine(Dictionary<string, object> log = null, bool isEvent = false) {
        logs["Timestamp"].Add(logCount, GetTimeStamp());
        foreach (KeyValuePair<string, string> pair in statelogs)
        {
            if (logs.ContainsKey(pair.Key))
            {
                logs[pair.Key].Add(logCount, pair.Value);
            }
            else
            {
                logs.Add(pair.Key, new Dictionary<int, string>{{logCount, pair.Value}});
            }
        }
        if (!isEvent) {
            logs["Event"].Add(logCount, "Sample");
            logs["EventType"].Add(logCount, "PlayEvent");
        }
        /*if (isEvent) {
            //logs["Event"].Add(logCount, ConvertToString(log["Event"]));
            //logs["EventType"].Add(logCount, ConvertToString(log["EventType"]));
            logs["NumberOfHealthy"].Add(logCount, statelogs["NumberOfHealthy"]);
            logs["NumberOfIsolated"].Add(logCount, statelogs["NumberOfIsolated"]);
            logs["NumberOfTested"].Add(logCount, statelogs["NumberOfTested"]);
            logs["NumberOfInfected"].Add(logCount, statelogs["NumberOfInfected"]);
        } else {
            logs["Event"].Add(logCount, "Sample");
            logs["EventType"].Add(logCount, "PlayEvent");
        }
        */
        /*logs["GameState"].Add(logCount, statelogs["GameState"]);
        logs["PlayID"].Add(logCount, statelogs["PlayID"]);
        logs["DeviceID"].Add(logCount, statelogs["DeviceID"]);
        logs["SubjectsOnStart"].Add(logCount, statelogs["SubjectsOnStart"]);
        logs["InfectedOnStart"].Add(logCount, statelogs["InfectedOnStart"]);
        logs["GameOverScore"].Add(logCount, statelogs["GameOverScore"]);
        logs["GameWonScore"].Add(logCount, statelogs["GameWonScore"]);
        logs["NewInfectionSeconds"].Add(logCount, statelogs["NewInfectionSeconds"]);
        */

        if (log != null) {
            foreach (KeyValuePair<string, object> pair in log)
            {
                if (logs.ContainsKey(pair.Key))
                {
                    if (logs[pair.Key].ContainsKey(logCount)) {
                        logs[pair.Key][logCount] = ConvertToString(pair.Value);
                    } else {
                        logs[pair.Key].Add(logCount, ConvertToString(pair.Value));
                    }
                    
                }
                else
                {
                    logs.Add(pair.Key, new Dictionary<int, string>{{logCount, ConvertToString(pair.Value)}});
                }
            }
        }
        logCount++;
    }

    public void SaveLogs() {
        //SaveCsvLogs();
        SaveSqlLogs();
    }

    public void ClearLogs() {
        logs.Clear();
        logCount = 0;
        initLog();
    }

    public void ClearGameLogs() {
        gamelogs.Clear();
        gamelogCount = 0;
    }

    public void AddToEventLog(Dictionary<string, object> log) {
        GenerateLine(log, true);
    }

    public void UpdateStateLog(Dictionary<string, object> log) {
        foreach (KeyValuePair<string, object> pair in log)
        {
            if (statelogs.ContainsKey(pair.Key)) {
                statelogs[pair.Key] = ConvertToString(pair.Value);
            } else
            {
                statelogs.Add(pair.Key, ConvertToString(pair.Value));
            }
        }      
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
        //UpdateStateLog(log);
        GenerateLine(log);
        gamelogCount++;
    }

    // Formats the logs to a CSV row format and saves them. Calls the CSV headers generation beforehand.
    // If a parameter doesn't have a value for a given row, uses the given value given previously (see 
    // UpdateHeadersAndDefaults).
    private void SaveCsvLogs()
    {
        if(!saveLocally) return;
        if (saveHeaders) {
            GenerateHeaders();
            saveHeaders = false;
        }
        string temp;
        for (int i = 0; i < logCount; i++)
        {
            string line = "";
            foreach (KeyValuePair<string, Dictionary<int, string>> log in logs)
            {
                if (line != "")
                {
                    line += fieldSeperator;
                }

                if (log.Value.TryGetValue(i, out temp))
                {
                    line += temp;
                }
                else
                {
                    if(statelogs.TryGetValue(log.Key, out temp))
                    {
                        line += temp;
                    }
                    else
                    {
                        line += "NULL";
                    }
                }
            }
            SaveToFile(line);
        }
    }


    // Generates the headers in a CSV format and saves them to the CSV file
    private void GenerateHeaders()
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

    private void SaveSqlLogs()
    {
        Dictionary<string, List<string>> logCollection = new Dictionary<string, List<string>>();
        logCollection.Add("Email", new List<string>());
        for(int i = 0; i < logCount; i++)
        {
            logCollection["Email"].Add(email);
        }
        string temp;

        foreach(KeyValuePair<string, Dictionary<int, string>> pair in logs)
        {
            logCollection.Add(pair.Key, new List<string>());
            
            for(int i = 0; i < logCount; i++)
            {
                if (pair.Value.TryGetValue(i, out temp))
                {
                    logCollection[pair.Key].Add(temp);
                }
                else
                {
                    if(statelogs.TryGetValue(pair.Key, out temp))
                    {
                        logCollection[pair.Key].Add(temp);
                    }
                    else
                    {
                        logCollection[pair.Key].Add("NULL");
                    }
                }
            }
        }
        SendSqlLogs(logCollection);
    }

    private void SendSqlLogs(Dictionary<string, List<string>> logCollection)
    {
        connectToMySQL.AddToUploadQueue(logCollection);
        connectToMySQL.UploadNow();
    }

    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);
    
        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);
    
        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";
    
        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
    
        return hashString.PadLeft(32, '0');
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

}
