using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSpawner : MonoBehaviour
{
    public Transform[] paths;
    public Transform getPath(){
        for(int i =0; i<paths.Length;i++){
            Debug.Log(i);
            Debug.Log(paths[i]);
        }
        var randomIndex = Random.Range(0, paths.Length);
        Debug.Log(paths[randomIndex]);
        return paths[randomIndex];
    }
}
