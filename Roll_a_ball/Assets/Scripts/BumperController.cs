using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperController : MonoBehaviour {

    public float sprint = 40000;
    public float damper = 1000;
    public float openAngle = 45.0f;
    public float closeAngle = -45.0f;

    private HingeJoint hj;
    private Rigidbody rb;

    void Start()
    {
        hj = gameObject.GetComponent<HingeJoint>();
        hj.useSpring = true;
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        openBumper();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            closeBumper();
        }
        if(  Input.GetKeyUp(KeyCode.Space))
        {
            openBumper();
        }
    }

    public void openBumper()
    {
        JointSpring spr = hj.spring;
        spr.spring = sprint;
        spr.damper = damper;
        spr.targetPosition = openAngle;
        hj.spring = spr;
    }

    public void closeBumper()
    {
        JointSpring spr = hj.spring;
        spr.spring = sprint;
        spr.damper = damper;
        spr.targetPosition = closeAngle;
        hj.spring = spr;
    }
}
