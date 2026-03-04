using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance {get; private set;}
    
    public JsonSettings jsonSettings;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 可选：让它在场景切换时不被销毁
        }
        // 如果文件不存在，创建默认设置并保存
        //为了方便查看，我暂存到E盘上，正常应该在C盘中的那个默认文件夹
        if (File.Exists("E:\\TmpData\\jsonSettings.json")) 
        {
            jsonSettings = JsonMgr.Instance.LoadData<JsonSettings>("jsonSettings");
        }
        else
        {
            jsonSettings = InitializeSettings();
        }
        
        // 应用加载
        if(jsonSettings != null)ApplyAllSettings(jsonSettings);
    }

    private void ApplyAllSettings(JsonSettings jsonSettings)
    {
        SetGameSpeed(jsonSettings.gameSpeed);
        SetCRT(jsonSettings.CRT);
        SetResolution(jsonSettings.resolution);
        SetVolume(jsonSettings.volume);
        SetSoundEffectVolume(jsonSettings.soundEffectVolume);
    }
    
    public void SetGameSpeed(float speed)
    {
        Time.timeScale = speed;
        if (speed != jsonSettings.gameSpeed)
        {
            jsonSettings.gameSpeed = speed;
            JsonMgr.Instance.SaveData(jsonSettings, "jsonSettings");
            print("写入新的配置信息");
        }
        
    }

    public GameObject CRT;
    public void SetCRT(bool value)
    {
        CRT.SetActive(value);
        jsonSettings.CRT = value;
        JsonMgr.Instance.SaveData(jsonSettings, "jsonSettings");
    }

    public void SetResolution(string resolution)
    {
        // 1. 解析字符串（如 "1920×1080"）
        string[] dimensions = resolution.Split('×'); // 注意：这里的 '×' 是乘号（不是字母x）
        
        if (dimensions.Length == 2 && 
            int.TryParse(dimensions[0], out int width) && 
            int.TryParse(dimensions[1], out int height))
        {
            // 2. 设置分辨率（最后一个参数表示是否全屏）
            Screen.SetResolution(width, height, Screen.fullScreen);
            Debug.Log($"分辨率已设置为: {width} × {height}");
        }
        else
        {
            Debug.LogError("分辨率格式错误！请使用 '宽度×高度' 格式（例如 '1920×1080'）");
        }
        
        jsonSettings.resolution = resolution;
        JsonMgr.Instance.SaveData(jsonSettings, "jsonSettings");
    }

    public void SetVolume(int value)
    {
        jsonSettings.volume = value;
        JsonMgr.Instance.SaveData(jsonSettings, "jsonSettings");
        if(MusicMgr.Instance != null)MusicMgr.Instance.audioSource.volume = value / 100f;
    }

    public void SetSoundEffectVolume(int value)
    {
        jsonSettings.soundEffectVolume = value;
        JsonMgr.Instance.SaveData(jsonSettings, "jsonSettings");
    }

    
    
    private JsonSettings InitializeSettings()
    {
        JsonSettings jsonSettings = new JsonSettings()
        {
            // 设置默认值（示例）
            gameSpeed = 1f,
            CRT = true,
            resolution = "2560×1440",
            volume = 100,
            soundEffectVolume = 100
        };
        
        // 保存默认设置
        JsonMgr.Instance.SaveData(jsonSettings,"jsonSettings");
        Debug.Log("创建了默认 jsonSettings 文件");
        
        return jsonSettings;
    }
}


public class JsonSettings
{
    public float gameSpeed;
    public bool CRT;
    public string resolution;
    public int volume;
    public int soundEffectVolume;
}