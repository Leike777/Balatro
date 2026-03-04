using DG.Tweening;
using UnityEngine;

public class UIMover : MonoBehaviour
{
    public float 弹性;
    public float time;
    void OnEnable()
    {
        // 获取UI元素的RectTransform
        RectTransform rectTransform = GetComponent<RectTransform>();
        
        // 存储原始位置
        Vector2 originalPosition = rectTransform.anchoredPosition;
        
        // 先将UI移动到屏幕下方（起始位置）
        rectTransform.anchoredPosition = new Vector2(originalPosition.x, -Screen.height);
        
        // 使用DOTween动画移动到原始位置
        rectTransform.DOAnchorPos(originalPosition, time) // 1秒动画时间
            .SetEase(Ease.OutBack, 弹性); // 使用弹性效果
    }
}