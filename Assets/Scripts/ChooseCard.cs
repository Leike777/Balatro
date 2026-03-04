using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CardData
{
    public Sprite sprite;
    public string name;
    public string description;
}
public class ChooseCard : MonoBehaviour
{
    public List<CardData> CardInfo;
    
    public Image cardImage;         // 卡牌图片（UI Image）
    public TMP_Text nameText;      // 卡牌名称（TextMeshPro）
    public TMP_Text descText;      // 卡牌描述（TextMeshPro）

    private int currentIndex = 0;   // 当前显示的卡牌索引
    
    void Start()
    {
        UpdateCardUI(); // 初始化显示第一张卡牌
    }

    // 更新UI显示当前卡牌
    private void UpdateCardUI()
    {
        //if (CardInfo.Count == 0) return;

        CardData currentCard = CardInfo[currentIndex];
        cardImage.sprite = currentCard.sprite;
        nameText.text = currentCard.name;
        descText.text = currentCard.description;
    }

    // 切换到下一张卡牌（循环）
    public void NextCard()
    {
        if (CardInfo.Count == 0) return;

        currentIndex = (currentIndex + 1) % CardInfo.Count; // 循环递增
        UpdateCardUI();
    }

    // 切换到上一张卡牌（循环）
    public void PreviousCard()
    {
        if (CardInfo.Count == 0) return;

        currentIndex = (currentIndex - 1 + CardInfo.Count) % CardInfo.Count; // 循环递减
        UpdateCardUI();
    }
}


