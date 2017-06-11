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

    // Use this for initialization
    void Start () {
        DiceText.text = "Dice: X";
        dice = GetComponent<Rigidbody>();
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

        DiceText.text = "Dice:" + result;

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
        dice.transform.position = new Vector3(6.0f, 6.0f, 6.0f);
        dice.transform.rotation = Quaternion.Euler(-55.0f, 108.0f, -135.0f);
    }

    void DiceRoll()
    {
        // 原点から-5～5ずらした位置に向かって投げる
        float directionX = Random.Range(-5.0f, 5.0f);
        float directionZ = Random.Range(-5.0f, 5.0f);
        Vector3 v = new Vector3(directionX, 0.0f, directionZ) - dice.transform.position;
        v.y = vy / power; // yはあとで*powerするので固定値になる

        // 力を加える位置
        Vector3 forcePosition = dice.transform.position - new Vector3(0.5f, 0.0f, -0.5f);

        dice.AddForceAtPosition(v * power, forcePosition);
    }
}
