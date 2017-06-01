using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;

    private Rigidbody rb;
    private int count;

    static Quaternion changeAxis(Quaternion q)
    {
        // return new Quaternion(-q.x, -q.y, q.z, q.w);
        return new Quaternion(q.x, q.z, q.y, q.w);
    }

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android
          || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Input.gyro.enabled = true;
        }
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "";
    }

    void FixedUpdate()
    {
        //Quaternion quaternion = changeAxis(Input.gyro.attitude);
        if (Input.gyro.enabled)
        {
            Vector3 gravity = Input.gyro.gravity;
            Vector3 movement = new Vector3(gravity.x, 0.0f, gravity.y);

            rb.AddForce(movement * speed);
        }
        else
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            rb.AddForce(movement * speed * 0.5f);
        }
    }

    // OnTriggerEnterはIs Triggerにチェックが入っている場合有効。
    // ただしチェックを入れると衝突せず物体をすり抜けるようになる。
    // 衝突判定をしつつぶつかったときの挙動を書きたい場合はOnCollisionEnterを使う。
    //void OnTriggerEnter(Collider other)
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            //Destroy(other.gameObject);
            ++count;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            winText.text = "You Win!";
        }
    }

}