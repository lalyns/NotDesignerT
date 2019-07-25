using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMesh : MonoBehaviour
{
    Transform _LightSource;
    Mesh mesh;
    


    private void Awake()
    {
        _LightSource = GameObject.FindGameObjectWithTag("LightSource").GetComponent<Transform>();
        mesh = GetComponent<MeshFilter>().mesh;

        SelectVertices();
    }

    void SelectVertices()
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        int i = 0;
        foreach(Vector3 vertex in vertices)
        {
            Debug.Log(i++ + " : " + vertex);
        }

        i = 0;
        foreach(int triangle in triangles)
        {
            Debug.Log(i++ + " : " + triangle);
        }

    }





}
