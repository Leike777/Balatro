using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayInfo : MonoBehaviour
{
    public static PlayInfo Instance;

    void Awake()
    {
        Instance = this;
    }
    private int _score = 0;
    private int _chips = 0;
    private int _multiple = 0;
    private int _cardPlay = 4;
    private int _cardAbandon = 3;
    private int _money = 14;
    public TextMeshProUGUI cardsTypeText;
    
    public int Score 
    { 
        get => _score; 
        set
        {
            if (value != _score)
            {
                UpdateNumText(0, value);
            }
            _score = value;
        }  
    }

    public int Chips
    {
        get => _chips; 
        set
        {
            if (value != _chips)
            {
                UpdateNumText(1, value);
            }
            _chips = value;
        } 
    }

    public int Multiple
    {
        get => _multiple; 
        set
        {
            if (value != _multiple)
            {
                UpdateNumText(2, value);
            }
            _multiple = value;
        }
    }

    public int CardPlay
    {
        get => _cardPlay; 
        set
        {
            if (value != _cardPlay)
            {
                UpdateNumText(3, value);
            }
            _cardPlay = value;
        }
    }

    public int CardAbandon
    {
        get => _cardAbandon;
        set
        {
            if (value != _cardAbandon)
            {
                UpdateNumText(4, value);
            }
            _cardAbandon = value;
        }
    }

    public int Money
    {
        get => _money; 
        set
        {
            if (value != _money)
            {
                UpdateNumText(5, value);
            }
            _money = value;
        }
    }
    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI chipsText;
    public TextMeshProUGUI multipleText;
    public TextMeshProUGUI cardPlayText;
    public TextMeshProUGUI cardAbandon;
    public TextMeshProUGUI moneyeText;
    
    private void UpdateNumText(int index, int value)
    {
        switch (index)
        {
            case 0:
                StartCoroutine(UpdateScore(value));
                break;
            case 1:
                chipsText.text = value.ToString();
                break;
            case 2:
                multipleText.text = value.ToString();
                break;
            case 3:
                cardPlayText.text = value.ToString();
                break;
            case 4:
                cardAbandon.text = value.ToString();
                break;
            case 5:
                moneyeText.text = value.ToString();
                break;
            default:
                break;
                
        }
    }


    public float scaleSize = 1.5f; // 放大倍数
    public float scaleDuration = 0.3f; // 缩放动画时间

    private IEnumerator UpdateScore(int targetValue)
    {
        int currentValue = int.Parse(scoreText.text);
        Transform textTransform = scoreText.transform;
        Vector3 originalScale = textTransform.localScale;
        
        // 创建动画序列
        Sequence sequence = DOTween.Sequence();
        
        // 1. 数字变化动画
        sequence.Append(
            DOTween.To(
                () => currentValue,
                x => {
                    currentValue = x;
                    scoreText.text = x.ToString();
                },
                targetValue,
                0.5f
            ).SetEase(Ease.OutQuad)
        );
        
        // 2. 同时进行的放大动画
        sequence.Join(textTransform.DOScale(originalScale * scaleSize, scaleDuration));
        
        // 3. 数字变化完成后缩回原大小
        sequence.Append(textTransform.DOScale(originalScale, scaleDuration));
        
        yield return sequence.WaitForCompletion();
    }
}
