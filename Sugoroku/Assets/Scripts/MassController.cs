using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MassEvent
{
    None,
    Ahead,  // 1歩進む
    Back,   // 1歩戻る
}

public class MassController : MonoBehaviour {
    [SerializeField]
    private int parentMass;
    [SerializeField]
    private int childMass;
    [SerializeField]
    private MassEvent massEvent;

    //各種取得・設定用関数
    public int GetParent()
    {
        return parentMass;
    }
    public void SetParent(int parentid)
    {
        parentMass = parentid;
    }
    public int GetChild()
    {
        return childMass;
    }
    public void SetChild(int childId)
    {
        childMass = childId;
    }
    public MassEvent GetMassEvent()
    {
        return massEvent;
    }
    public void SetMassEvent(MassEvent type)
    {
        massEvent = type;
    }

    // Use this for initialization
    void Start()
    {
    }


// Update is called once per frame
    void Update () {
		
	}
}
