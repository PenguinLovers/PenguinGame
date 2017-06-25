using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour {

    public GameObject prefab;
    public int numberOfObjects = 20;
    public float radius = 5f;

    // Use this for initialization
    void Start () {
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            GameObject tmpObject = Instantiate(prefab, pos, Quaternion.identity);
            tmpObject.name = "Mass" + i;    // CharacterControllerから取得できるようにリネーム
            MassController tmpMC = tmpObject.GetComponent<MassController>();
            tmpMC.SetChild((i == numberOfObjects - 1) ? 0 : i + 1);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
