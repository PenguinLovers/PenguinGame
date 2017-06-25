using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType
{
    Circle,
    Straight,
    // あとあと南極とかそれ以外を追加
}

public class MapCreator : MonoBehaviour {

    public GameObject prefab;
    public int numberOfObjects = 20;
    public float radius = 5f;
    private MapType mapType = MapType.Circle;

    // Use this for initialization
    void Start () {
        CreateMap();
    }

    // Update is called once per frame
    void Update () {
		
	}

    // 外部からマップ変更
    public void ChangeMap(MapType type)
    {
        mapType = type;
        DestroyMap();
        CreateMap();
    }

    private void DestroyMap()
    {
        // マスの名前から削除？
        //for (int i = 0; i < numberOfObjects; i++)
        //{
        //    Destroy(MassObject[i], .0f);
        //}
    }

    private void CreateMap()
    {
        GameManager.GetInstance().SetCurrentState(GameState.CreateMap);
        switch (mapType)
        {
            default: break;
            case MapType.Circle:
                for (int i = 0; i < numberOfObjects; i++)
                {
                    float angle = i * Mathf.PI * 2 / numberOfObjects;
                    Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                    GameObject tmpObject = Instantiate(prefab, pos, Quaternion.identity);
                    tmpObject.name = "Mass" + i;    // CharacterControllerから取得できるようにリネーム
                    MassController tmpMC = tmpObject.GetComponent<MassController>();
                    tmpMC.SetChild((i == numberOfObjects - 1) ? 0 : i + 1);
                }
                break;
            case MapType.Straight:
                for (int i = 0; i < numberOfObjects; i++)
                {
                    Vector3 pos = new Vector3(i*2, 0, 0);
                    GameObject tmpObject = Instantiate(prefab, pos, Quaternion.identity);
                    tmpObject.name = "Mass" + i;    // CharacterControllerから取得できるようにリネーム
                    MassController tmpMC = tmpObject.GetComponent<MassController>();
                    tmpMC.SetChild((i == numberOfObjects - 1) ? 0 : i + 1);
                }
                break;
        }
        // キャラの初期位置設定へ
        GameManager.GetInstance().SetCurrentState(GameState.CharaInit);
    }

}
