using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassController : MonoBehaviour {
    [SerializeField]
    private int childMass;

    //取得用関数
    public int GetChild()
    {
        return childMass;
    }

    public void SetChild(int childId)
    {
        childMass = childId;
    }

    // Use this for initialization
    void Start()
    {
    }


// Update is called once per frame
    void Update () {
		
	}
}
