using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEditor.Playables;
using UnityEngine;


public enum JsonType
{
    JsonUtility,
    LitJson,
}
public class JsonMgr
{
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance => instance;
    private JsonMgr() { }
    
    public void SaveData(object data, string fileName, JsonType jsonType = JsonType.JsonUtility)
    {
        //string path = Application.persistentDataPath + "/" + fileName + ".json";
        string path = "E:/TmpData/" + fileName + ".json";
        string jsonStr = "";
        switch (jsonType)
        {
            case JsonType.JsonUtility:
                jsonStr = JsonUtility.ToJson(data);
                break;
            case JsonType.LitJson:
                jsonStr = JsonUtility.ToJson(data);
                break;
        }
        File.WriteAllText(path, jsonStr);
    }

    public T LoadData<T>(string fileName, JsonType jsonType = JsonType.JsonUtility)
    {
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        if (!File.Exists(path))
        {
            //path = Application.persistentDataPath + "/" + fileName + ".json";
            path = "E:\\TmpData\\" + fileName + ".json";
        }
        
        string jsonStr = File.ReadAllText(path);
        
        switch (jsonType)
        {
            // case JsonType.JsonUtility:
            //     return JsonUtility.FromJson<T>(jsonStr);
            //     break;
            // case JsonType.LitJson:
            //     return JsonUtility.FromJson<T>(jsonStr);
            //     break;
            default:
                return default(T);
        }
    }
}



