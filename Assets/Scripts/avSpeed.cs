using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class avSpeed : MonoBehaviour
{
    private GameObject[] cars;
    // Start is called before the first frame update
    public List<float> speed;
    public GameObject txt; 
    public void Start(){
        cars = GameObject.FindGameObjectsWithTag ("Car");
        Debug.Log(cars.Length);
        foreach(GameObject car in cars){
            CarEngine carEngine = car.GetComponent<CarEngine>();
            Debug.Log(car.name);
            // speed.Add(carEngine.GetComponent);
        }
        // txt.FindGameObjectsWithTag("avSpeed").text = speed.Average().toString();
    }
    public void FixedUpdate(){
        cars = GameObject.FindGameObjectsWithTag ("Car");
        foreach(GameObject car in cars){
            CarEngine carEngine = car.GetComponent<CarEngine>();
            // speed.Add(carEngine.curentSpeed);
        }
        // txt.FindGameObjectsWithTag ("avSpeed").text = speed.Average().toString();
    }
}