using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseDifficulty : MonoBehaviour
{
    public List<DifficultyData> difficulties;
    
    public Image image;
    public TMP_Text nameText;
    
    private int currentIndex = 0;

    void Start()
    {
        UpdateDifficulty();
    }
    
    private void UpdateDifficulty()
    {
        DifficultyData data = difficulties[currentIndex];
        image.sprite = data.sprite;
        nameText.text = data.name;
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
public class DifficultyData
{
    public Sprite sprite;
    public string name;
}