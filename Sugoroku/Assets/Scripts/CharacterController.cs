using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    private int nowMass = 0;
    private GameObject nowMassObj;
    private GameObject childMassObj;

    private Rigidbody character;

    private bool b_keyUp = false;
    private bool isMoving = false;
    private bool enableMoving = false;
    private int moveCount = 0;

    public GameObject prefab;
    public int numberOfObjects = 20;
    public float radius = 5f;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<Rigidbody>();
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            GameObject tmpObject = Instantiate(prefab, pos, Quaternion.identity);
            tmpObject.name = "Mass"+i;
            MassController tmpMC = tmpObject.GetComponent<MassController>();
            tmpMC.SetChild( (i == numberOfObjects-1)? 0 : i + 1);
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            b_keyUp = true;
        }

        if (GameManager.GetInstance().GetCurrentState() == GameState.CharaMove)
        {
            moveCount = GetDiceScore();
        }
    }
 
    void FixedUpdate()
    {
        if(!isMoving && moveCount > 0)//b_keyUp)
        {
            StartCoroutine("GoToNext");
            isMoving = true;
            b_keyUp = false;
        }
    }

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

        // 目的地に到着したと思うのでマスの情報を取得しなおしておく
        UpdateNowMass();
        GetMassInfo();
        isMoving = false;
        --moveCount;
        Debug.Log("moveCount:" + moveCount);
        GameManager.GetInstance().SetCurrentState(GameState.DiceWait);  // ダイス投げ待ち状態へ

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

    private int GetDiceScore()
    {
        return BoardDiceController.diceScore;
    }
}
