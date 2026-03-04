using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageCarousel : MonoBehaviour
{
    public Image[] images; // 拖入4个子Image对象
    public float displayTime = 1f; // 每张图片显示时间
    private int currentIndex = 0;

    void Start()
    {
        // 初始隐藏所有图片
        foreach (var img in images)
        {
            img.gameObject.SetActive(false);
        }
        
        // 开始轮播
        StartCoroutine(PlayCarousel());
    }

    IEnumerator PlayCarousel()
    {
        while (true)
        {
            // 隐藏当前图片
            images[currentIndex].gameObject.SetActive(false);
            
            // 计算下一张图片索引
            currentIndex = (currentIndex + 1) % images.Length;
            
            // 显示下一张图片
            images[currentIndex].gameObject.SetActive(true);
            
            // 等待指定时间
            yield return new WaitForSeconds(displayTime);
        }
    }
}