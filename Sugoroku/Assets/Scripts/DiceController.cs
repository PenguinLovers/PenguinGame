using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceController : MonoBehaviour {

    public float power = 20.0f;
    public float vy = 500.0f;
    public Text DiceText;
    private int diceScore = 0;

    private Rigidbody dice;

    private bool b_keyUp;

    // 振り直し用
    private Vector3 dicePos;
    private Quaternion diceRot;

    // Use this for initialization
    void Start () {
        DiceText.text = "Dice: X";
        dice = GetComponent<Rigidbody>();
        dicePos = dice.transform.position;
        diceRot = dice.transform.rotation;
        DiceRoll();
    }
	
	// Update is called once per frame
	void Update () {
        if (dice.velocity.magnitude == 0.0f)
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
        diceScore = CheckScore();
        DiceText.text = "Dice:" + diceScore;

	}

    void FixedUpdate()
    {
        if( b_keyUp )
        {
            InitDicePosition();
            DiceRoll();
            b_keyUp = false;
        }
    }

    void InitDicePosition()
    {
        dice.transform.position = dicePos;// new Vector3(6.0f, 6.0f, 6.0f);
        dice.transform.rotation = diceRot;// Quaternion.Euler(-55.0f, 108.0f, -135.0f);
    }

    void DiceRoll()
    {
        // 原点から少しずらした位置に向かって投げる
        float dx = dice.transform.position.x / 2.0f;
        float dz = dice.transform.position.z / 2.0f;
        float directionX = Random.Range(-Mathf.Abs(dx), Mathf.Abs(dx));
        float directionZ = Random.Range(-Mathf.Abs(dz), Mathf.Abs(dz));
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
