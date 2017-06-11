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

	// Use this for initialization
	void Start () {
        hj = gameObject.GetComponent<HingeJoint>();
        rb = gameObject.GetComponent<Rigidbody>();
        openFlipper();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            openFlipper();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            closeFlipper();
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
