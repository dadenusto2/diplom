using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeshFlip : EditorWindow
{
    [MenuItem("Tools/Mesh flip")]
    public static void Open()
    {
        GetWindow<MeshFlip>();
    }

    public Transform gameObject;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        //EditorGUILayout.PropertyField(obj.FindProperty("gameObject"));

        /*if (gameObject == null)
        {
            EditorGUILayout.HelpBox("Ёлемени не выбран!", MessageType.Warning);
        }
        else
        {*/
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        //}
        obj.ApplyModifiedProperties();
    }
    void DrawButtons()
    {
        if (Selection.activeGameObject != null)
        {
            if (GUILayout.Button("»зменить mesh"))
            {
                Mesh mesh = Selection.activeGameObject.GetComponent<MeshFilter>().mesh;
                Vector3[] normals = mesh.normals;
                for (int i = 0; i < normals.Length; i++)
                    normals[i] = -1 * normals[i];
                mesh.normals = normals;
                for (int i = 0; i < mesh.subMeshCount; i++)
                {
                    int[] tris = mesh.GetTriangles(i);
                    for (int j = 0; j < tris.Length; j += 3)
                    {
                        int temp = tris[j];
                        tris[j] = tris[j + 1];
                        tris[j + 1] = temp;
                    }
                    mesh.SetTriangles(tris, i);
                }
            }
        }
    }
}
