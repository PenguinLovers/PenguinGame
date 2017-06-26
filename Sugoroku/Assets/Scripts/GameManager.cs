using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    DestroyMap,
    CreateMap,
    CharaInit,
    DiceWait,
    Dice,
    CharaMove,
    MassEvent,
}

public class GameManager : MonoBehaviour {

    private static GameManager s_Instance;

    // 現在の状態
    private GameState currentGameState;
    private GameState prevGameState;

    void Awake()
    {
        s_Instance = this;
        SetPrevState(GameState.CreateMap);
        SetCurrentState(GameState.CreateMap);
    }

    static public GameManager GetInstance()
    {
        return s_Instance;
    }

    // 外からこのメソッドを使って状態を変更
    public void SetCurrentState(GameState state)
    {
        SetPrevState(currentGameState);
        currentGameState = state;
    }

    public GameState GetCurrentState()
    {
        return currentGameState;
    }

    private void SetPrevState(GameState state)
    {
        prevGameState = state;
    }

    public GameState GetPrevState()
    {
        return prevGameState;
    }
}
