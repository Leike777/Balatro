using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerView : MonoBehaviour
{
    public int point;
    public int color;
    public Image  iamge;
    
    void Start()
    {
        transform.position = GameObject.Find("Deck").transform.position;//确定位置
        DeterminePointsAndColor();
        GiveInfoToParentCard();
    }

    private void DeterminePointsAndColor()
    {
        point = Random.Range(2, 15);
        color = Random.Range(1, 5);
        iamge.sprite = CardMgr.Instance.cardSprites[point - 1 + (color - 1) * 13 -1];
    }

    private void GiveInfoToParentCard()
    {
        CardVisual cardVisual = GetComponent<CardVisual>();
        Card parentCard = cardVisual.parentCard.GetComponent<Card>();
        
        parentCard.point = point;
        parentCard.color = color;
    }
}
