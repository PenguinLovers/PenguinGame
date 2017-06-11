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

	// Use this for initialization
	void Start () {
        DiceText.text = "Dice: X";
        dice = GetComponent<Rigidbody>();

        // 原点から-5～5ずらした位置に向かって投げる
        float directionX = Random.Range(-5.0f, 5.0f);
        float directionZ = Random.Range(-5.0f, 5.0f);
        Vector3 v = new Vector3(directionX, 0.0f, directionZ) - dice.transform.position;
        v.y = vy / power; // yはあとで*powerするので固定値になる

        // 力を加える位置
        Vector3 forcePosition = dice.transform.position - new Vector3(0.5f, 0.0f, -0.5f);

        dice.AddForceAtPosition(v * power, forcePosition);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 check_1 = transform.TransformDirection(Vector3.forward);
        Vector3 check_5 = transform.TransformDirection(Vector3.right);
        Vector3 check_4 = transform.TransformDirection(Vector3.left);
        int result = 0;

        if (Mathf.Abs(Mathf.Round(check_1.y)) != 1)
        {
            if (Mathf.Abs(Mathf.Round(check_4.y)) != 1)
            {
                if (Mathf.Round(check_5.y) == 1)
                {
                    result = 5;
                }
                else
                {
                    result = 2;
                }
            }
            else
            {
                if (Mathf.Round(check_4.y) == 1)
                {
                    result = 4;
                }
                else
                {
                    result = 3;
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
}
