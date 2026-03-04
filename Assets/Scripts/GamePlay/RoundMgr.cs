using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundMgr : MonoBehaviour
{
    public static RoundMgr Instance;
    void Awake() => Instance = this;
    
    
    // 当前状态（私有字段+公有属性）
    private RoundState _currentState;
    public RoundState CurrentState
    {
        get => _currentState;
        set
        {
            if (_currentState != value)
            {
                var previousState = _currentState;
                _currentState = value;
                OnStateChanged?.Invoke(previousState, _currentState);
            }
        }
    }

    // 状态改变事件（参数：上一个状态，新状态）
    public event Action<RoundState, RoundState> OnStateChanged;

    void Start()
    {
        // 初始化状态
        SetState(RoundState.ChooseBlind);
    }

    // 公开方法用于改变状态
    public void SetState(RoundState newState)
    {
        CurrentState = newState;
    }

    // 状态改变时的处理（也可以监听OnStateChanged事件）
    private void HandleStateChange(RoundState previousState, RoundState newState)
    {
        // Debug.Log($"状态改变: {previousState} -> {newState}");

        switch (newState)
        {
            case RoundState.ChooseBlind:
                OnEnterChooseBlind();
                break;
            case RoundState.Battle:
                OnEnterBattle();
                break;
            case RoundState.Settlement:
                OnEnterSettlement();
                break;
            case RoundState.Shopping:
                OnEnterShopping();
                break;
        }

        // 退出上一个状态的处理
        switch (previousState)
        {
            case RoundState.ChooseBlind:
                OnExitChooseBlind();
                break;
            // 其他状态退出处理...
        }
    }

    #region 状态具体实现
    private void OnEnterChooseBlind()
    {
        // Debug.Log("进入选择盲注阶段");
        // 实现具体逻辑...
    }

    private void OnExitChooseBlind()
    {
        // Debug.Log("退出选择盲注阶段");
        // 实现具体逻辑...
    }

    private void OnEnterBattle()
    {
        // Debug.Log("进入战斗阶段");
        // 实现具体逻辑...
    }

    private void OnEnterSettlement()
    {
        // Debug.Log("进入结算阶段");
        // 实现具体逻辑...
    }

    private void OnEnterShopping()
    {
        // Debug.Log("进入购物阶段");
        // 实现具体逻辑...
    }
    #endregion

    // 订阅/取消订阅事件
    private void OnEnable()
    {
        OnStateChanged += HandleStateChange;
    }

    private void OnDisable()
    {
        OnStateChanged -= HandleStateChange;
    }
}

public enum RoundState
{
    ChooseBlind,
    Battle,
    Settlement,
    Shopping,
}