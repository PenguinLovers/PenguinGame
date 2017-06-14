using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    private int nowMass = 0;
    GameObject nowMassObj;
    GameObject childMassObj;

    Rigidbody character;

    private bool b_keyUp = false;
    private bool isMoving = false;

    // Use this for initialization
    void Start () {
        character = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            b_keyUp = true;
        }
    }

    void FixedUpdate()
    {
        if(!isMoving && b_keyUp)
        {
            StartCoroutine("GoToNext");
            isMoving = true;
            b_keyUp = false;
        }
    }

    IEnumerator GoToNext()
    {
        if (nowMassObj == null || childMassObj == null)
        {
            GetMassInfo();
        }

        // 次のマスの位置
        Vector3 dest = childMassObj.transform.position;
        Debug.Log(character.transform.position + "," + dest);
        dest.y = character.transform.position.y;    // キャラクターの高さは変えない

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

        StopCoroutine("GoToNext");
    }

    void GetMassInfo()
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

    void UpdateNowMass()
    {
        MassController mc = nowMassObj.GetComponent<MassController>();
        nowMass = mc.GetChild();
    }
}
