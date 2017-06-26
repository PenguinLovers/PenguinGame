using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    private int nowMass = 0;
    private GameObject nowMassObj;
    private GameObject childMassObj;
    private GameObject parentMassObj;

    private Rigidbody character;

    private bool isMoving = false;
    private int moveCount = 0;
    private bool isFirstMoving = false;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        // マスの生成が終わってないとキャラの初期配置ができないので仕方なくここへ。
        // マップ生成→キャラの初期配置の順序がしっかりすればステータス無視でStartに移せる。
        if (GameManager.GetInstance().GetCurrentState() == GameState.CharaInit)
        {
            InitPosition();
            // キャラの初期位置設定が終わってからダイス振り待ちへ
            GameManager.GetInstance().SetCurrentState(GameState.DiceWait);
        }
        if (GameManager.GetInstance().GetCurrentState() == GameState.CharaMove
          && GameManager.GetInstance().GetPrevState() == GameState.Dice)
        {
            if (moveCount == 0)
            {
                SetMoveCount(GetDiceScore());
                isFirstMoving = true;
            }
        }
    }
 
    void FixedUpdate()
    {
        if(!isMoving && moveCount != 0)
        {
            isMoving = true;
            if (moveCount > 0)
            {
                StartCoroutine("GoToNext");
            }
            else if(moveCount < 0)
            {
                StartCoroutine("GoToPrev");
            }
        }

    }

    // 1マスずつ進める処理
    private IEnumerator GoToNext()
    {
        if (nowMassObj == null || childMassObj == null)
        {
            GetMassInfo();
        }

        // 次のマスの位置
        Vector3 dest = childMassObj.transform.position;
        Debug.Log(character.transform.position + "," + dest);
        dest.y = character.transform.position.y;    // キャラクターの高さは変えない

        character.transform.LookAt(dest);

        while ( Mathf.Abs(character.transform.position.x - dest.x) > 0.05f
            || Mathf.Abs(character.transform.position.z - dest.z) > 0.05f)
        {
            character.transform.position += (dest - character.transform.position) / 20.0f;
            yield return null;
        }

        // 目的地(1マス先)に到着したと思うのでマスの情報を取得しなおしておく
        UpdateNowMass(true);
        GetMassInfo();
        isMoving = false;
        SetMoveCount(moveCount - 1);
        Debug.Log("moveCount:" + moveCount);
        // マスイベント処理
        if (moveCount == 0)
        {
            StartCoroutine("ExecMassEvent");
        }

        
        StopCoroutine("GoToNext");
    }

    // 1マスずつ進める処理
    private IEnumerator GoToPrev()
    {
        if (nowMassObj == null || parentMassObj == null)
        {
            GetMassInfo();
        }

        // 前のマスの位置
        Vector3 dest = parentMassObj.transform.position;
        Debug.Log(character.transform.position + "," + dest);
        dest.y = character.transform.position.y;    // キャラクターの高さは変えない

        character.transform.LookAt(dest);

        while (Mathf.Abs(character.transform.position.x - dest.x) > 0.05f
            || Mathf.Abs(character.transform.position.z - dest.z) > 0.05f)
        {
            character.transform.position += (dest - character.transform.position) / 20.0f;
            yield return null;
        }

        // 目的地(1マス前)に到着したと思うのでマスの情報を取得しなおしておく
        UpdateNowMass(false);
        GetMassInfo();
        isMoving = false;
        SetMoveCount(moveCount + 1);
        Debug.Log("moveCount:" + moveCount);
        // マスイベント処理
        if (moveCount == 0)
        {
            StartCoroutine("ExecMassEvent");
        }

        StopCoroutine("GoToPrev");
    }

    private IEnumerator ExecMassEvent()
    {
        // イベントを何回も起こさないように。戻る進むマスがあるので初回移動時のみ判定。
        if (isFirstMoving)
        {
            GameManager.GetInstance().SetCurrentState(GameState.MassEvent);
            isFirstMoving = false; 
            switch (GetMassEvent())
            {
                case MassEvent.Ahead:
                    yield return new WaitForSeconds(1);
                    SetMoveCount(1);
                    break;
                case MassEvent.Back:
                    yield return new WaitForSeconds(1);
                    SetMoveCount(-1);
                    break;
                case MassEvent.None:
                    // 何も起こさずにダイス投げ待ちへ
                    GameManager.GetInstance().SetCurrentState(GameState.DiceWait);
                    break;
                default:
                    break;
            }
        }
        else
        {
            // すでに一回イベント起こしてるのでスルー
            GameManager.GetInstance().SetCurrentState(GameState.DiceWait);  // ダイス投げ待ち状態へ
        }

        // 進む戻る系ならキャラ移動へ
        if (moveCount != 0)
        {
            GameManager.GetInstance().SetCurrentState(GameState.CharaMove);
        }
    }

    private void SetMoveCount(int count)
    {
        moveCount = count;
    }

    private void GetMassInfo()
    {
        // 現在のマスを取得
        string massName = "Mass" + nowMass;
        nowMassObj = GameObject.Find(massName);
        MassController mc = nowMassObj.GetComponent<MassController>();

        // 現在のマスから次のマス（子）を取得
        string destMassName = "Mass" + mc.GetChild();
        Debug.Log(massName + "'s child is " + destMassName);
        childMassObj = GameObject.Find(destMassName);

        // 現在のマスから前のマス（親）を取得
        string prevMassName = "Mass" + mc.GetParent();
        Debug.Log(massName + "'s parent is " + prevMassName);
        parentMassObj = GameObject.Find(prevMassName);
    }

    private void UpdateNowMass(bool isForward)
    {
        MassController mc = nowMassObj.GetComponent<MassController>();
        if (isForward)
        {
            nowMass = mc.GetChild();
        }
        else
        {
            nowMass = mc.GetParent();
        }
    }

    private MassEvent GetMassEvent()
    {
        // 現在のマスを取得
        string massName = "Mass" + nowMass;
        nowMassObj = GameObject.Find(massName);
        MassController mc = nowMassObj.GetComponent<MassController>();

        Debug.Log(mc.GetMassEvent());
        return mc.GetMassEvent();
    }


    private void InitPosition()
    {
        // 初期マスの情報取得
        nowMass = 0;
        GetMassInfo();

        // 初期マスにキャラを置く
        Vector3 dest = nowMassObj.transform.position;
        Debug.Log(character.transform.position + "," + dest);
        dest.y = character.transform.position.y;    // キャラクターの高さは変えない
        character.transform.position = dest;

        // 次のマスの位置
        dest = childMassObj.transform.position;
        Debug.Log(character.transform.position + "," + dest);
        dest.y = character.transform.position.y;    // キャラクターの高さは変えない

        // 次のマスの方向へ向く
        character.transform.LookAt(dest);
    }

    private int GetDiceScore()
    {
        return BoardDiceController.diceScore;
    }
}
