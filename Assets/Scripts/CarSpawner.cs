using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public void spawnCar() {
        GameObject go = SelectACarPrefab();
        go.transform.position = GameObject.Find("Spawner").transform.position;
        Instantiate(go);
    }
    private GameObject SelectACarPrefab(){
        var randomIndex = Random.Range(0, carPrefabs.Length);
        return carPrefabs[randomIndex];
    }
}
