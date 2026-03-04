using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GamePlayUIMgr : MonoBehaviour
{
    public static GamePlayUIMgr Instance;
    
    public GameObject[] blinds;
    
    public float distance = 2f; // 向下移动的距离
    public float duration = 1f; // 动画持续时间
    public float elasticity = 0.5f; // 弹性强度(0-1)
    public int oscillations = 10; // 振荡次数
    
    void Awake() => Instance = this;

    void Start()
    {
        RoundMgr.Instance.OnStateChanged += StateUIChanged;
        //StateUIDown(BattleBlind);
    }

    private int index = 1;
    
    public void EnterBattle()
    {
        foreach (GameObject obj in blinds)
        {
            // 计算目标位置（当前Y坐标减去距离）
            Vector3 targetPosition = obj.transform.position + Vector3.down * distance;
        
            // 使用弹性缓动移动
            obj.transform.DOMove(targetPosition, duration)
                .SetEase(Ease.OutElastic, elasticity, oscillations);
        }

        if (index == 1)
        {
            RoundMgr.Instance.CurrentState = RoundState.Battle;
            index++;
        }
        else
        {
            
        }
    }

    public void BlindDown(GameObject obj)
    {
        // 计算目标位置（当前Y坐标减去距离）
        Vector3 targetPosition = obj.transform.position + Vector3.down * distance / 7f;
        
        // 使用弹性缓动移动
        obj.transform.DOMove(targetPosition, duration)
            .SetEase(Ease.OutElastic, elasticity, oscillations);
    }

    public void BlindUp(GameObject obj)
    {
        // 计算目标位置（当前Y坐标减去距离）
        Vector3 targetPosition = obj.transform.position + Vector3.up * distance / 6f;
        
        // 使用弹性缓动移动
        obj.transform.DOMove(targetPosition, duration)
            .SetEase(Ease.OutElastic, elasticity, oscillations);
    }
    
    public void DoubleGold()
    {
        PlayInfo.Instance.Money *= 2;
    }

    public GameObject ChooseBlind;
    public GameObject BattleBlind;
    public GameObject SettlementNone;
    public GameObject Shopping;
    public GameObject battle1;
    public GameObject battle2;
    public void StateUIChanged(RoundState oldState, RoundState newState)
    {
        switch (oldState)
        {
            case RoundState.ChooseBlind:
                StateUIUp(ChooseBlind);
                break;
            case RoundState.Battle:
                StateUIUp(BattleBlind);
                break;
            case RoundState.Settlement: 
                StateUIUp(SettlementNone);
                break;
            case RoundState.Shopping:
                StateUIUp(Shopping);
                break;
        }

        switch (newState)
        {
            case RoundState.ChooseBlind:
                StateUIDown(ChooseBlind);
                break;
            case RoundState.Battle:
                StateUIDown(BattleBlind);
                break;
            case RoundState.Settlement: 
                StateUIDown(SettlementNone);
                break;
            case RoundState.Shopping:
                battle1.SetActive(false);
                battle2.SetActive(true);
                StateUIDown(Shopping);
                break;
        }
    }

    public float stateUIMoveDistance;
    
    public void StateUIUp(GameObject obj)
    {
        // 计算目标位置（当前Y坐标减去距离）
        Vector3 targetPosition = obj.transform.position + Vector3.up * stateUIMoveDistance ;
        
        // 使用弹性缓动移动
        obj.transform.DOMove(targetPosition, duration)
            .SetEase(Ease.OutElastic, elasticity, oscillations);
    }

    public void StateUIDown(GameObject obj)
    {
        StartCoroutine(Delay(obj));
        
    }
    
    public TextMeshProUGUI numText;
    public void RoundNumber(int num)
    {
        numText.text = num.ToString();
    }

    public float delay;
    IEnumerator Delay(GameObject obj)
    {
        yield return new WaitForSeconds(delay);
        // 计算目标位置（当前Y坐标减去距离）
        Vector3 targetPosition = obj.transform.position + Vector3.down * stateUIMoveDistance ;
        
        // 使用弹性缓动移动
        obj.transform.DOMove(targetPosition, duration)
            .SetEase(Ease.OutElastic, elasticity, oscillations);
    }

    public void SetActiveTrue(GameObject obj)
    {
        obj.SetActive(true);
    }
}
