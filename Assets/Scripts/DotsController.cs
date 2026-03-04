using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class DotsController : MonoBehaviour
{
    private int quantity;
    public int index;
    
    void Start()
    {
        quantity = transform.childCount;
        //开始的初始化更新 要由点点组件的控制者来给予初始index以更新。
    }
    
    public void LastDot()
    {
        index = (index - 1 + quantity) % quantity;
        UpdateDots(index);
    }
    
    public void NextDot()
    {
        index = (index + 1 + quantity) % quantity;
        UpdateDots(index);
    }

    public void UpdateDots(int whiteDotNum)
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            if (whiteDotNum == i)
            {
                child.GetComponent<Image>().color = Color.white;
            }
            else
            {
                child.GetComponent<Image>().color = Color.black;
            }

            i++;
        }
    }
}
