using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Color lineColor;

    private List<Transform> nodes = new List<Transform>();

    void OnDrawGizmosSelected() {
        Gizmos.color = lineColor;

        Transform[] pathTransform = GetComponentsInChildren<Transform>(); 
        nodes = new List<Transform>();

        for(int i = 0; i< pathTransform.Length;i++){
            if(pathTransform[i] != transform){
                nodes.Add(pathTransform[i]);
            }
        }
        int j=1;
        while(true){
            try{
                Vector3 curentNode = nodes[j-1].position;
                Vector3 previousNode = nodes[j].position;
                Gizmos.DrawLine(previousNode, curentNode);
                Gizmos.DrawSphere(curentNode, 0.3f);
                j++;
            }
            catch{
                break;
            }
        }
    }

}
