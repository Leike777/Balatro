using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChooseOneThing : MonoBehaviour
{
    public List<OneThingData> difficulties;
    
    public TMP_Text nameText;
    
    public int currentIndex;

    public DotsController dotsController;
    void Start()
    {
        switch (name)
        {
            case "GameSpeed":
                float speed = Settings.Instance.jsonSettings.gameSpeed;
                if (speed == 0.5f)
                {
                    currentIndex = 0;
                    dotsController.index = 0;
                }
                else if (speed == 1f)
                {
                    currentIndex = 1;
                    dotsController.index = 1;
                }
                else if (speed == 2f) 
                {
                    currentIndex = 2;
                    dotsController.index = 2;
                }
                else 
                {
                    currentIndex = 3;
                    dotsController.index = 3;
                }
                break;
            case "CRT":
                bool index = Settings.Instance.jsonSettings.CRT;
                if (index)
                {
                    currentIndex = 0;
                    dotsController.index = 0;
                }
                else
                {
                    currentIndex = 1;
                    dotsController.index = 1;
                }
                break;
            case "Resolution":
                string resolution = Settings.Instance.jsonSettings.resolution;
                if (resolution == "3440×1440")
                {
                    currentIndex = 0;
                    dotsController.index = 0;
                }
                else if (resolution == "2560×1440")
                {
                    currentIndex = 1;
                    dotsController.index = 1;
                }
                else if (resolution == "1920×1080")
                {
                    currentIndex = 2;
                    dotsController.index = 2;
                }
                else if (resolution == "1920×1200")
                {
                    currentIndex = 3;
                    dotsController.index = 3;
                }
                else
                {
                    currentIndex = 4;
                    dotsController.index = 4;
                }
                break;
        }
        
        UpdateDifficulty();
        dotsController.UpdateDots(currentIndex);
    }
    
    private void UpdateDifficulty()
    {
        OneThingData data = difficulties[currentIndex];
        nameText.text = data.text;
        
        //写入Settings中
        switch (name)
        {
            case "GameSpeed":
                Settings.Instance.SetGameSpeed(float.Parse(nameText.text));
                break;
            case "CRT":
                if (nameText.text == "开启")
                {
                    Settings.Instance.SetCRT(true);
                }
                else
                {
                    Settings.Instance.SetCRT(false);
                }
                break;
            case "Resolution":
                Settings.Instance.SetResolution(nameText.text);
                break;
            
        }
    }
    
    public void NextCard()
    {
        if (difficulties.Count == 0) return;

        currentIndex = (currentIndex + 1) % difficulties.Count; // 循环递增
        UpdateDifficulty();
    }

    // 切换到上一张卡牌（循环）
    public void PreviousCard()
    {
        if (difficulties.Count == 0) return;

        currentIndex = (currentIndex - 1 + difficulties.Count) % difficulties.Count; // 循环递减
        UpdateDifficulty();
    }
}

[System.Serializable]
public class OneThingData
{
    public string text;
}