using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour {

    public float power = 20.0f;
    public float vy = 500.0f;

    private Rigidbody dice;

	// Use this for initialization
	void Start () {
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
		
	}
}
