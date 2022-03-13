using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    List<GameObject> prefabList = new List<GameObject>();
    public GameObject Prefab1;
    public GameObject Prefab2;
    public GameObject Prefab3;
    public GameObject Prefab4;
    public GameObject Prefab5;
    public GameObject Prefab6;
    public GameObject Prefab7;
    public GameObject Prefab8;
    public GameObject Prefab9;

    private PedestrianCharacterController pedestrianPrefab;
    public int pedestrianToSpawn;
    private void Awake()
    {
        prefabList.Add(Prefab1);
        prefabList.Add(Prefab2);
        prefabList.Add(Prefab3);
        prefabList.Add(Prefab4);
        prefabList.Add(Prefab5);
        prefabList.Add(Prefab6);
        prefabList.Add(Prefab7);
        prefabList.Add(Prefab8);
        prefabList.Add(Prefab9);
    }
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Spawn()
    {
        int prefabIndex = UnityEngine.Random.Range(0, prefabList.Count);
        int count = 0;
        while (count < pedestrianToSpawn)
        {
            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            GameObject obj = Instantiate(prefabList[prefabIndex], child.GetComponent<Waypoint>().transform.position, Quaternion.identity);
            obj.GetComponent<WaypointNavigator>().curentWaypoint = child.GetComponent<Waypoint>();
            count++;
            prefabIndex = UnityEngine.Random.Range(0, prefabList.Count);
        }

    }
}