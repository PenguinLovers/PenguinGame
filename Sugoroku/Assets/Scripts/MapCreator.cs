using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType
{
    Circle,
    Straight,
    // あとあと南極とかそれ以外を追加
}

public enum MapStep
{
    None,
    Create,
    Destroy,
}

public class MapCreator : MonoBehaviour {

    public GameObject prefab;
    public int numberOfObjects = 20;
    public float radius = 5f;
    private MapType mapType = MapType.Circle;
    private MapStep mapStep = MapStep.None;
    private bool isMapChange = false;
    // Use this for initialization
    void Start () {
        mapStep = MapStep.Create;
    }

    // Update is called once per frame
    void Update () {
        MapStep nextStep = mapStep;
		switch(mapStep)
        {
            case MapStep.Create:
                nextStep = CreateMap();
                break;
            case MapStep.Destroy:
                nextStep = DestroyMap();
                break;
            default: break;
        }
        mapStep = nextStep;
    }

    public MapType GetType()
    {
        return mapType;
    }

    // 外部からマップ変更
    public void ChangeMap(MapType type)
    {
        mapType = type;
        isMapChange = true;
        mapStep = MapStep.Destroy;
    }

    private MapStep DestroyMap()
    {
        GameManager.GetInstance().SetCurrentState(GameState.DestroyMap);
        var clones = GameObject.FindGameObjectsWithTag("Mass");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }
        return isMapChange ? MapStep.Create : MapStep.None;
    }

    private MapStep CreateMap()
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
                    tmpMC.SetParent((i == 0) ? numberOfObjects - 1 : i - 1);
                    tmpMC.SetChild((i == numberOfObjects - 1) ? 0 : i + 1);
                    tmpMC.SetMassEvent(MassEvent.None);
                    //tmpMC.SetMassEvent((i%2==0) ? MassEvent.Ahead : MassEvent.Back);
                }
                break;
            case MapType.Straight:
                for (int i = 0; i < numberOfObjects; i++)
                {
                    Vector3 pos = new Vector3(i*2, 0, 0);
                    GameObject tmpObject = Instantiate(prefab, pos, Quaternion.identity);
                    tmpObject.name = "Mass" + i;    // CharacterControllerから取得できるようにリネーム
                    MassController tmpMC = tmpObject.GetComponent<MassController>();
                    tmpMC.SetParent((i == 0) ? numberOfObjects - 1 : i - 1);
                    tmpMC.SetChild((i == numberOfObjects - 1) ? 0 : i + 1);
                    tmpMC.SetMassEvent(MassEvent.None);
                    //tmpMC.SetMassEvent((i%2==0) ? MassEvent.Ahead : MassEvent.Back);
                }
                break;
        }
        // キャラの初期位置設定へ
        GameManager.GetInstance().SetCurrentState(GameState.CharaInit);
        isMapChange = false;

        return MapStep.None;
    }

}
