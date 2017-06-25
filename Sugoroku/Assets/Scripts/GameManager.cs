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
}

public class GameManager : MonoBehaviour {

    private static GameManager s_Instance;

    // 現在の状態
    private GameState currentGameState;

    void Awake()
    {
        s_Instance = this;
        SetCurrentState(GameState.Dice);
    }

    static public GameManager GetInstance()
    {
        return s_Instance;
    }

    // 外からこのメソッドを使って状態を変更
    public void SetCurrentState(GameState state)
    {
        currentGameState = state;
    }

    public GameState GetCurrentState()
    {
        return currentGameState;
    }
}
