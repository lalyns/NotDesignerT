using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCastObject : MonoBehaviour
{
    public Transform _VertexParent;
    Transform[] _Vertices;

    GameObject _LightSource;
    GameObject _Ground;

    public GameObject shadowPosition;
    Transform[] _ShadowVertices;
    public GameObject shadowObject;
    public MeshFilter shadowMeshFilter;
    public MeshRenderer shadowMeshRenderer;
    
    public void Awake()
    {
        _LightSource = GameObject.FindGameObjectWithTag("LightSource");
        _Ground = GameObject.FindGameObjectWithTag("Ground");

        Transform[] temp = _VertexParent.GetComponentsInChildren<Transform>();

        _Vertices = new Transform[temp.Length - 1];
        int i = 0;

        foreach(Transform vertex in temp)
        {
            if (vertex == _VertexParent)
                continue;
            else
            {
                _Vertices[i++] = vertex;
                //Debug.Log(_Vertices[i++].name);
            }
                    
        }

        Vector3 lightDirection = GetLightDirection(_LightSource, _Ground);

        Transform[] shadowCastTransforms = DecideShadowCastDirection(_LightSource);

        GameObject Pos1 = Instantiate(shadowPosition, shadowObject.transform);
        Pos1.transform.position = shadowCastTransforms[0].position;
        Pos1.name = "Pos1";
        GameObject Pos2 = Instantiate(shadowPosition, shadowObject.transform);
        Pos2.transform.position = shadowCastTransforms[1].position;
        Pos2.name = "Pos2";
        GameObject Pos3 = Instantiate(shadowPosition, shadowObject.transform);
        Pos3.transform.position = shadowCastTransforms[2].position;
        Pos3.name = "Pos3";
        GameObject Pos4 = Instantiate(shadowPosition, shadowObject.transform);
        Pos4.transform.position = shadowCastTransforms[3].position;
        Pos4.name = "Pos4";
        GameObject Pos5 = Instantiate(shadowPosition, shadowObject.transform);
        Pos5.transform.position = GroundHitPoint(shadowCastTransforms[0], lightDirection);
        Pos5.name = "Pos5";
        GameObject Pos6 = Instantiate(shadowPosition, shadowObject.transform);
        Pos6.transform.position = GroundHitPoint(shadowCastTransforms[1], lightDirection);
        Pos6.name = "Pos6";

        _ShadowVertices = new Transform[6];
        _ShadowVertices[0] = Pos1.transform;
        _ShadowVertices[1] = Pos2.transform;
        _ShadowVertices[2] = Pos3.transform;
        _ShadowVertices[3] = Pos4.transform;
        _ShadowVertices[4] = Pos5.transform;
        _ShadowVertices[5] = Pos6.transform;

        Mesh shadowMesh = CreateShadowMesh(_ShadowVertices);
        shadowMeshFilter.sharedMesh = shadowMesh;
    }

    public void Update()
    {
        Vector3 lightDirection = GetLightDirection(_LightSource, _Ground);

        Transform[] shadowCastTransforms = DecideShadowCastDirection(_LightSource);

        Debug.DrawLine(shadowCastTransforms[0].position, GroundHitPoint(shadowCastTransforms[0], lightDirection), Color.red);
        Debug.DrawLine(shadowCastTransforms[1].position, GroundHitPoint(shadowCastTransforms[1], lightDirection), Color.red);

        //Mesh shadowMesh = CreateShadowMesh(shadowCastTransforms);
        //shadowMeshFilter.sharedMesh = shadowMesh;

    }

    public Vector3 GroundHitPoint(Transform start, Vector3 direction)
    {
        Vector3 hitPoint = Vector3.zero;

        Ray ray = new Ray();
        ray.origin = start.position;
        ray.direction = direction;

        RaycastHit hit;
        if(Physics.Raycast(ray.origin, ray.direction * 1000f, out hit, 1000f, 1<<10, QueryTriggerInteraction.Ignore))
        {
            hitPoint = hit.point;
        }

        return hitPoint;
    }

    public Vector3 GetLightDirection(GameObject LightSource, GameObject Ground)
    {
        Vector3 direction = Ground.transform.position - LightSource.transform.position ;
        direction = direction.normalized;

        return direction;
    }

    public Transform[] DecideShadowCastDirection(GameObject lightSource)
    {
        Transform[] transforms = new Transform[4];

        Vector3 lightPos = lightSource.transform.position;
        Vector3 thisPos = this.transform.position;

        // 광원이 오른쪽에 존재
        if(lightPos.x - thisPos.x > 0)
        {
            transforms[0] = _Vertices[0];
            transforms[1] = _Vertices[1];
            transforms[2] = _Vertices[4];
            transforms[3] = _Vertices[5];
        }
        // 광원이 왼쪽에 존재
        else if (lightPos.x - thisPos.x < 0)
        {
            transforms[0] = _Vertices[2];
            transforms[1] = _Vertices[3];
            transforms[2] = _Vertices[6];
            transforms[3] = _Vertices[7];
        }
        // 광원이 위쪽에 존재
        else if (lightPos.z - thisPos.z < 0)
        {
            transforms[0] = _Vertices[3];
            transforms[1] = _Vertices[0];
            transforms[2] = _Vertices[7];
            transforms[3] = _Vertices[4];
        }
        // 광원이 아래쪽에 존재
        else if(lightPos.z - thisPos.z > 0)
        {
            transforms[0] = _Vertices[1];
            transforms[1] = _Vertices[2];
            transforms[2] = _Vertices[5];
            transforms[3] = _Vertices[6];
        }

        return transforms;
    }
    
    public Mesh CreateShadowMesh(Transform[] shadowCastTransforms)
    {
        Mesh shadowMesh = new Mesh();
        shadowMesh.name = "ShadowMesh";

        shadowMesh.vertices = new Vector3[6];

        shadowMesh.vertices[0] = shadowCastTransforms[0].position;
        shadowMesh.vertices[1] = shadowCastTransforms[2].position;
        shadowMesh.vertices[2] = GroundHitPoint(shadowCastTransforms[0], GetLightDirection(_LightSource, _Ground));

        shadowMesh.vertices[3] = shadowCastTransforms[1].position;
        shadowMesh.vertices[4] = shadowCastTransforms[3].position;
        shadowMesh.vertices[5] = GroundHitPoint(shadowCastTransforms[1], GetLightDirection(_LightSource, _Ground));


        shadowMesh.triangles = new int[12]
        { 
            0,1,2,
            0,2,5,
            0,3,5,
            3,4,5
        }
        ;



        return shadowMesh;
    }
    
}
