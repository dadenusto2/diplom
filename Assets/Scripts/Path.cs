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

        for(int i = 0; i< nodes.Count;i++){
            Vector3 curentNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero;
            if(i>0){
                previousNode = nodes[i-1].position;
            }
            Gizmos.DrawLine(previousNode, curentNode);
            Gizmos.DrawSphere(curentNode, 0.3f);
        }
    }

}
