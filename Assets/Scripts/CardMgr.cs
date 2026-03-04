using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CardMgr : MonoBehaviour
{
    public List<Card> selectingCards;
    
    public event Action OnSelectingCardsChanged;
    public static CardMgr Instance;
    public List<Sprite> cardSprites;
    public HorizontalCardHolder horizontalCardHolder;
    public int nowSortType = 0;
    
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        OnSelectingCardsChanged += JudgeCardsType;
    }

    public void UpdateSelectingCards(bool value,GameObject obj)
    {
        if (value)
        {
            AddSelectedCard(obj);
            OnSelectingCardsChanged?.Invoke();
        }
        else
        {
            RemoveSelectedCard(obj);
            OnSelectingCardsChanged?.Invoke();
        }
    }

    public void AddSelectedCard(GameObject obj)
    {
        selectingCards.Add(obj.GetComponent<Card>());
    }

    public void RemoveSelectedCard(GameObject obj)
    {
        selectingCards.Remove(obj.GetComponent<Card>());
    }

    public void SortByPoints()
    {
        // 1. 提取带分数的卡片列表
        List<(int point, GameObject cardObj, CardVisual cardVisual)> points = horizontalCardHolder.cards
            .Select(card => (card.point, card.gameObject.transform.parent.gameObject, card.cardVisual))
            .ToList();

        // 2. 按分数从大到小排序
        var sortedCards = points.OrderByDescending(x => x.point).ToList();
        
        
        int i = 0;
        // 3. 调整场景中的层级顺序（越晚渲染的越在上层）
        foreach (var card in sortedCards)
        {
            // 通过设置同级顺序（SiblingIndex），越大的数字越后渲染（显示在上层）
            card.cardObj.transform.SetSiblingIndex(i);
            card.cardVisual.transform.SetSiblingIndex(i);
            i++;

        }
    }

    public void SortByColor()
    {
        // 1. 提取带分数的卡片列表
        List<(int color, GameObject cardObj, CardVisual cardVisual)> points = horizontalCardHolder.cards
            .Select(card => (card.color, card.gameObject.transform.parent.gameObject, card.cardVisual))
            .ToList();

        // 2. 按分数从大到小排序
        var sortedCards = points.OrderByDescending(x => x.color).ToList();
        
        
        int i = 0;
        // 3. 调整场景中的层级顺序（越晚渲染的越在上层）
        foreach (var card in sortedCards)
        {
            // 通过设置同级顺序（SiblingIndex），越大的数字越后渲染（显示在上层）
            card.cardObj.transform.SetSiblingIndex(i);
            card.cardVisual.transform.SetSiblingIndex(i);
            i++;

        }
    }


    public GameObject playedCards;
    public float moveDistance = 2f;  // 向下移动的距离
    public float duration = 1f;      // 移动持续时间
    public Transform targetTransform;
    public void PlayCard()
    {
        PlayInfo.Instance.CardPlay -= 1;
        
        List<Card> cardsPlayed = new List<Card>();
        foreach (var card in selectingCards)
        {
            card.gameObject.transform.parent.transform.SetParent(playedCards.transform);
            cardsPlayed.Add(card);
        }
        
        
        // 计算目标位置（当前Y坐标 - 移动距离）
        Vector3 targetPosition = targetTransform.position + Vector3.down * moveDistance;

        // 使用 DOTween 移动
        targetTransform.DOMove(targetPosition, duration)
            .SetEase(Ease.OutQuad); // 可选：设置缓动曲线

        StartCoroutine(CalculateScore(cardsPlayed));
    }
    
    public GameObject abandonedCards;
    public void AbandonCards()
    {
        PlayInfo.Instance.CardAbandon -= 1;
        
        List<Card> destroyedCards = new List<Card>();
        foreach (var card in selectingCards)
        {
            card.gameObject.transform.parent.transform.SetParent(abandonedCards.transform);
            destroyedCards.Add(card);
            card.enabled = false;
        }

        StartCoroutine(DestryAnbandonedCards());
    }

    IEnumerator DestryAnbandonedCards()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (var card in selectingCards)
        {
            Destroy(card.transform.parent.gameObject);
        }
        selectingCards.Clear();
        horizontalCardHolder.DrawCards();
        
        yield return new WaitForSeconds(0.01f);
        if (nowSortType == 1)
        {
            SortByPoints();
        }
        else if (nowSortType == 2)
        {
            SortByColor();
        }
        OnSelectingCardsChanged?.Invoke();
    }

    public void SetSortType(int sortType)
    {
        nowSortType = sortType;
    }


    public GameObject scoreSquare;
    public Vector3 squareOffset;
    IEnumerator CalculateScore(List<Card> cardsPlayed)
    {
        if (cardsType == CardsType.同花)
        {
            yield return new WaitForSeconds(0.7f);
            foreach (var card in cardsPlayed)
            {
                //分数图标
                PlayInfo.Instance.chipsText.transform.DOScale(new Vector3(1.8f, 1.8f, 1.8f), 0.1f)
                    .SetEase(Ease.OutBack);  // 弹性效果（可选）
            
                float randomZRotation = UnityEngine.Random.Range(0f, 360f);
                Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomZRotation);
                GameObject square = Instantiate(scoreSquare, card.gameObject.transform.position + squareOffset, Quaternion.identity,card.gameObject.transform );
                Transform squareTrans = square.transform.Find("image");
                squareTrans.rotation = randomRotation;
                squareTrans.DOScale(new Vector3(1f, 1f, 1f), 0.2f)
                    .SetEase(Ease.OutBack);
                TextMeshProUGUI textMesh = square.transform.Find("num").GetComponent<TextMeshProUGUI>();
                int point;
                if (card.point == 14)
                {
                    point = 11;
                }
                else if (card.point <= 10)
                {
                    point = card.point;
                }
                else
                {
                    point = 10;
                }
                PlayInfo.Instance.Chips += point;
                textMesh.text = "+" + point.ToString();
                
                yield return new WaitForSeconds(0.15f);
                PlayInfo.Instance.chipsText.transform.DOScale(Vector3.one, 0.05f)
                    .SetEase(Ease.OutBack);  // 弹性效果（可选）
                //方块持续时间
                yield return new WaitForSeconds(0.25f);
                squareTrans.DOScale(Vector3.zero, 0.15f)
                    .SetEase(Ease.OutBack);
                textMesh.gameObject.transform.DOScale(Vector3.zero, 0.15f)
                    .SetEase(Ease.OutBack);
                
            
                yield return new WaitForSeconds(0.15f);
                //图标消失
               
                Destroy(square);
            }
            
            PlayInfo.Instance.Score += PlayInfo.Instance.Chips * PlayInfo.Instance.Multiple;
            yield return new WaitForSeconds(0.4f);
            
            if (PlayInfo.Instance.Score >= 450)
            {
                EnterSettlement();
                AbandonCards1(horizontalCardHolder.cards);
            }
            else AbandonCards(cardsPlayed);
        }

        else
        {
            int maxPoint = 0;
            GameObject maxCard = new GameObject();
            foreach (var card in cardsPlayed)
            {
                if (card.point > maxPoint)
                {
                    maxPoint = card.point;
                    maxCard = card.gameObject;
                }
            }
            yield return new WaitForSeconds(0.7f);
            //分数图标
            PlayInfo.Instance.chipsText.transform.DOScale(new Vector3(1.8f, 1.8f, 1.8f), 0.1f)
                .SetEase(Ease.OutBack);  // 弹性效果（可选）
            
            float randomZRotation = UnityEngine.Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomZRotation);
            GameObject square = Instantiate(scoreSquare, maxCard.transform.position + squareOffset, Quaternion.identity,maxCard.transform );
            Transform squareTrans = square.transform.Find("image");
            squareTrans.rotation = randomRotation;
            squareTrans.DOScale(new Vector3(1f, 1f, 1f), 0.2f)
                .SetEase(Ease.OutBack);
            TextMeshProUGUI textMesh = square.transform.Find("num").GetComponent<TextMeshProUGUI>();
            int point;
            if (maxPoint == 14)
            {
                point = 11;
            }
            else if (maxPoint <= 10)
            {
                point = maxCard.GetComponent<Card>().point;
            }
            else
            {
                point = 10;
            }
            PlayInfo.Instance.Chips += point;
            textMesh.text = "+" + point.ToString();
                
            //方块持续时间
            yield return new WaitForSeconds(0.15f);
            PlayInfo.Instance.chipsText.transform.DOScale(Vector3.one, 0.05f)
                .SetEase(Ease.OutBack);  // 弹性效果（可选）
            yield return new WaitForSeconds(0.25f);
            squareTrans.DOScale(Vector3.zero, 0.15f)
                .SetEase(Ease.OutBack);
            textMesh.gameObject.transform.DOScale(Vector3.zero, 0.15f)
                .SetEase(Ease.OutBack);
            
            //图标消失
            yield return new WaitForSeconds(0.15f);
            Destroy(square);
            
            PlayInfo.Instance.Score += PlayInfo.Instance.Chips * PlayInfo.Instance.Multiple;
            yield return new WaitForSeconds(0.4f);
            
            if (PlayInfo.Instance.Score >= 450)
            {
                EnterSettlement();
                AbandonCards1(horizontalCardHolder.cards);
            }
            else AbandonCards(cardsPlayed);
        }
        
    }
    public void AbandonCards(List<Card> cardsAbandoned)
    {
        
        List<Card> destroyedCards = new List<Card>();
        foreach (var card in cardsAbandoned)
        {
            card.gameObject.transform.parent.transform.SetParent(abandonedCards.transform);
            destroyedCards.Add(card);
            card.enabled = false;
        }

        StartCoroutine(DestryAnbandonedCards(cardsAbandoned));
    }
    
    public RectTransform AreCanDon;
    IEnumerator DestryAnbandonedCards(List<Card> cardsAbandoned)
    {
        yield return new WaitForSeconds(0.1f);
        foreach (var card in cardsAbandoned)
        {
            Destroy(card.transform.parent.gameObject);
        }
        selectingCards.Clear();
        horizontalCardHolder.DrawCards();
        
        yield return new WaitForSeconds(0.01f);
        if (nowSortType == 1)
        {
            SortByPoints();
        }
        else if (nowSortType == 2)
        {
            SortByColor();
        }
        OnSelectingCardsChanged?.Invoke();

        // 使用 DOTween 移动
        AreCanDon.DOAnchorPos(Vector2.zero, duration)
            .SetEase(Ease.OutQuad); // 可选：设置缓动曲线
    }
    
    public void AbandonCards1(List<Card> cardsAbandoned)
    {
        
        List<Card> destroyedCards = new List<Card>();
        foreach (var card in cardsAbandoned)
        {
            card.gameObject.transform.parent.transform.SetParent(abandonedCards.transform);
            destroyedCards.Add(card);
            card.enabled = false;
        }

        StartCoroutine(DestryAnbandonedCards1(cardsAbandoned));
    }
    IEnumerator DestryAnbandonedCards1(List<Card> cardsAbandoned)
    {
        yield return new WaitForSeconds(0.1f);
        foreach (var card in cardsAbandoned)
        {
            Destroy(card.transform.parent.gameObject);
        }
        selectingCards.Clear();
        
        yield return new WaitForSeconds(0.01f);
        

        // 使用 DOTween 移动
        AreCanDon.DOAnchorPos(Vector2.zero, duration)
            .SetEase(Ease.OutQuad); // 可选：设置缓动曲线
    }
    
    public CardsType cardsType;
    private void JudgeCardsType()
    {
        if (selectingCards.Count == 0)
        {
            cardsType = CardsType.none;
            PlayInfo.Instance.cardsTypeText.text = "";

            PlayInfo.Instance.Chips = 0;
            PlayInfo.Instance.Multiple = 0;
        }
        else if (selectingCards.Count < 5)//5x1
        {
            cardsType = CardsType.高牌;
            PlayInfo.Instance.cardsTypeText.text = cardsType.ToString();
            
            PlayInfo.Instance.Chips = 5;
            PlayInfo.Instance.Multiple = 1;

        }
        else//35x4
        {
            bool isSameColor = true;
            foreach (var card in selectingCards)
            {
                if (card.color == selectingCards[0].color)
                {
                    continue;
                }
                isSameColor = false;
            }
            
            if (isSameColor)
            {
                cardsType = CardsType.同花;
                PlayInfo.Instance.cardsTypeText.text = cardsType.ToString();
                
                PlayInfo.Instance.Chips = 35;
                PlayInfo.Instance.Multiple = 4;

            }
            else
            {
                cardsType = CardsType.高牌;
                PlayInfo.Instance.cardsTypeText.text = cardsType.ToString();
                
                PlayInfo.Instance.Chips = 5;
                PlayInfo.Instance.Multiple = 1;

            }
            
        }
    }
    
    public void EnterSettlement()
    {
        AreCanDon.gameObject.SetActive(false);
        
        PlayInfo.Instance.Chips = 0;
        PlayInfo.Instance.Multiple = 0;
        StartCoroutine(ScoreBackZero());
        StartCoroutine(ShoppingSign());

    }

    IEnumerator ScoreBackZero()
    {
        yield return new WaitForSeconds(1f);
        PlayInfo.Instance.Score = 0;
    }

    public float distance1;
    public float distance2;
    public float distance3;
    public float distance4;
    public RectTransform red;
    public RectTransform shop;
    public RectTransform shopPanel;
    IEnumerator ShoppingSign()
    {
        red.DOAnchorPos(red.position + new Vector3(0, distance2, 0), 0.3f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(0.7f);
        shop.DOAnchorPos(shop.position - new Vector3(0, distance1, 0), 0.3f).SetEase(Ease.OutQuad);
        shop.gameObject.SetActive(true);
        shopPanel.DOAnchorPos(shopPanel.position + new Vector3(distance4, distance3, 0), 0.3f).SetEase(Ease.OutQuad);
    }
}

public enum CardsType
{
    none,
    高牌,
    同花
}