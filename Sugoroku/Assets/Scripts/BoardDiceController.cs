using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardDiceController : MonoBehaviour
{

    public float power = 20.0f;
    public float vy = 500.0f;
    public Text DiceText;
    static public int diceScore = 0;

    public GameObject character;
    private Vector3 offset;


    private Rigidbody dice;

    private bool b_keyUp;
    private bool isStart; // 振り始めたか
    
    // 振り直し用
    private Vector3 dicePos;
    private Quaternion diceRot;

    // Use this for initialization
    void Start()
    {
        DiceText.text = "Dice: X";
        offset = transform.position - character.transform.position;
        dice = GetComponent<Rigidbody>();
        dicePos = dice.transform.position;
        diceRot = dice.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // ダイス投げ待ち
        if (GameManager.GetInstance().GetCurrentState() == GameState.DiceWait)
        {
            if (Application.platform == RuntimePlatform.Android
                || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Ended)
                    {
                        // タッチ終了
                        b_keyUp = true;
                    }
                }
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    b_keyUp = true;
                }
            }
        }

        // ダイスが止まっている状態
        if (dice.velocity.magnitude == 0.0f)
        {
            if (isStart)
            {
                // 振り終わって出目確定までいった
                if (GameManager.GetInstance().GetCurrentState() == GameState.Dice)
                {
                    GameManager.GetInstance().SetCurrentState(GameState.CharaMove);
                }
            }
        }

        diceScore = CheckScore();
        DiceText.text = "Dice:" + diceScore;

    }

    void FixedUpdate()
    {
        if (b_keyUp)
        {
            InitDicePosition();
            DiceRoll();
            b_keyUp = false;
        }
    }

    void LateUpdate()
    {
        if(!isStart)
        {
            transform.position = character.transform.position + offset;
        }
    }

    private void InitDicePosition()
    {
        isStart = false;
        dice.transform.position = dicePos;
        dice.transform.rotation = diceRot;
    }

    private void DiceRoll()
    {
        dice.velocity = new Vector3(0.0f, 0.0f, 0.0f);  // サイコロに重力がかかってる状態なのでリセットしておく

        isStart = true;
        GameManager.GetInstance().SetCurrentState(GameState.Dice);

        // キャラクターの位置から少しずらした位置に向かって投げる
        float directionX = character.transform.position.x + Random.Range(-1.0f, 1.0f);
        float directionZ = character.transform.position.z + Random.Range(-1.0f, 1.0f);
        Vector3 v = new Vector3(directionX, 0.0f, directionZ) - dice.transform.position;
        v.y = vy / power; // yはあとで*powerするので固定値になる

        // 力を加える位置(トップスピンになるように)
        Vector3 forcePosition;

        float radius = 0.5f;    // ダイスが1辺1mっぽいので、内部の球の半径は0.5
        float sqrExcludeRadius = dice.transform.position.sqrMagnitude + radius * radius;    // 原点中心の大きな球の半径の2乗を求める

        // 大きな球の範囲外かつダイス内の球の範囲内の点が出るまで繰り返す
        do
        {
            float forceRandX = Random.Range(-radius, radius);
            float forceRandY = Random.Range(-radius, radius);
            float forceRandZ = Random.Range(-radius, radius);
            forcePosition = dice.transform.position + new Vector3(forceRandX, forceRandY, forceRandZ);
        } while (sqrExcludeRadius > forcePosition.sqrMagnitude);

        dice.AddForceAtPosition(v * power, forcePosition);
    }

    private int CheckScore()
    {
        // 以下、出目判定
        Vector3 check_1 = transform.TransformDirection(Vector3.forward);
        Vector3 check_3 = transform.TransformDirection(Vector3.right);
        Vector3 check_2 = transform.TransformDirection(Vector3.up);
        int result = 0;

        if (Mathf.Abs(Mathf.Round(check_1.y)) != 1)
        {
            if (Mathf.Abs(Mathf.Round(check_2.y)) != 1)
            {
                if (Mathf.Round(check_3.y) == 1)
                {
                    result = 3;
                }
                else
                {
                    result = 4;
                }
            }
            else
            {
                if (Mathf.Round(check_2.y) == 1)
                {
                    result = 2;
                }
                else
                {
                    result = 5;
                }
            }
        }
        else
        {
            if (Mathf.Round(check_1.y) == 1)
            {
                result = 1;
            }
            else
            {
                result = 6;
            }
        }

        return result;

    }
}
