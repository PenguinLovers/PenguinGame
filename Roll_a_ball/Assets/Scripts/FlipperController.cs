using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperController : MonoBehaviour {

    public float spring = 40000; // 弾性係数
    public float damper = 1000; // 
    public float openAngle = 45; 
    public float closeAngle = -45;

    private HingeJoint hj;
    private Rigidbody rb;
    public bool b_keyDown;
    public bool b_keyUp;

	// Use this for initialization
	void Start () {
        b_keyDown = false;
        b_keyUp = false;
        hj = gameObject.GetComponent<HingeJoint>();
        rb = gameObject.GetComponent<Rigidbody>();
        openFlipper();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            b_keyUp = true;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            b_keyDown = true;
        }
	}

    void FixedUpdate()
    {
        if (b_keyUp)
        {
            openFlipper();
            b_keyUp = false;
        }
        if (b_keyDown)
        {
            closeFlipper();
            b_keyDown = false;
        }
    }

    public void openFlipper()
    {
        JointSpring spr = hj.spring;
        spr.spring = spring;
        spr.damper = damper;
        spr.targetPosition = openAngle;
        hj.spring = spr;
    }

    public void closeFlipper()
    {
        JointSpring spr = hj.spring;
        spr.spring = spring;
        spr.damper = damper;
        spr.targetPosition = closeAngle;
        hj.spring = spr;
    }
}
