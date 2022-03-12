using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class AutoSave : MonoBehaviour
{
    private GameObject[] cars;
    public void Save() {
        cars = GameObject.FindGameObjectsWithTag ("Car");
        List<Data> datas = new List<Data>();
        foreach(GameObject car in cars){
            Data data = new Data(car.name, car.transform.position.x, car.transform.position.y, car.transform.position.z, car.transform.rotation.x, car.transform.rotation.y, car.transform.rotation.z);
            // Debug.Log(car.name);
            // Debug.Log(data.name);
            datas.Add(data);
        }
        Debug.Log(datas.Count);
        string json = JsonUtility.ToJson(datas);
        Debug.Log(json);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/data.json", json);
    }
}

[System.Serializable]
public class Data{
    [SerializeField]
    public string name;
    [SerializeField]
    public float posX, posY, posZ, rotX, rotY, rotZ;
    public Data(string name, float posX, float posY, float  posZ, float  rotX, float rotY, float rotZ){
        this.name=name;
        this.posX=posX;
        this.posY=posY;
        this.posZ=posZ;
        this.rotX=rotX;
        this.rotY=rotY;
        this.rotZ=rotZ;
    }
}
