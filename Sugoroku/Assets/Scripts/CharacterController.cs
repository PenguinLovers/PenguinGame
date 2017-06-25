using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    private int nowMass = 0;
    private GameObject nowMassObj;
    private GameObject childMassObj;

    private Rigidbody character;

    private bool isMoving = false;
    private int moveCount = 0;

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
        if (GameManager.GetInstance().GetCurrentState() == GameState.CharaMove)
        {
            if (moveCount == 0)
            {
                moveCount = GetDiceScore();
            }
        }
    }
 
    void FixedUpdate()
    {
        if(!isMoving && moveCount > 0)
        {
            StartCoroutine("GoToNext");
            isMoving = true;
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
        UpdateNowMass();
        GetMassInfo();
        isMoving = false;
        --moveCount;
        Debug.Log("moveCount:" + moveCount);
        if (moveCount == 0)
        {
            GameManager.GetInstance().SetCurrentState(GameState.DiceWait);  // ダイス投げ待ち状態へ
        }
        StopCoroutine("GoToNext");
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
    }

    private void UpdateNowMass()
    {
        MassController mc = nowMassObj.GetComponent<MassController>();
        nowMass = mc.GetChild();
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
