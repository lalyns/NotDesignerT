using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCastObject : MonoBehaviour
{
    public Transform _VertexParent;
    Transform[] _Vertices;

    GameObject _LightSource;
    LightSource _LightSourceScript;
    LightSource.LightSourcePosition _LightPosition;
    GameObject _Ground;

    Transform[] _ShadowVertices;
    public GameObject shadowObject;
    public MeshFilter shadowMeshFilter;
    public MeshRenderer shadowMeshRenderer;
    public MeshCollider collider;
    
    public void Awake()
    {
        _LightSource = GameObject.FindGameObjectWithTag("LightSource");
        _LightSourceScript = _LightSource.GetComponent<LightSource>();

        _Ground = GameObject.FindGameObjectWithTag("Ground");

        Transform[] temp1 = _VertexParent.GetComponentsInChildren<Transform>();

        _Vertices = new Transform[temp1.Length - 1];
        int i = 0;

        foreach(Transform vertex in temp1)
        {
            if (vertex == _VertexParent)
                continue;
            else
            {
                _Vertices[i++] = vertex;
                //Debug.Log(_Vertices[i++].name);
            }
                    
        }

        Transform[] temp2 = shadowObject.GetComponentsInChildren<Transform>();
        _ShadowVertices = new Transform[temp2.Length -1];
        i = 0;
        foreach (Transform shadowVertex in temp2)
        {
            if (shadowVertex == shadowObject.transform)
                continue;
            else
            {
                _ShadowVertices[i++] = shadowVertex;
            }

        }


    }

    public void Update()
    {
        _LightPosition = _LightSourceScript._LightPosition;

        Vector3 lightDirection = GetLightDirection(_LightSource, _Ground);

        Transform[] shadowCastTransforms = DecideShadowCastDirection(_LightSource);

        foreach (Transform trans in shadowCastTransforms)
        {
            Debug.Log(trans.localPosition);
        }

        Debug.DrawLine(shadowCastTransforms[0].position, GroundHitPoint(shadowCastTransforms[0], lightDirection), Color.red);
        Debug.DrawLine(shadowCastTransforms[1].position, GroundHitPoint(shadowCastTransforms[1], lightDirection), Color.red);

        //Mesh shadowMesh = CreateShadowMesh(shadowCastTransforms);
        //shadowMeshFilter.sharedMesh = shadowMesh;

        SetShadowVerticesPosition(shadowCastTransforms, lightDirection);
        CreateShadowMesh(_ShadowVertices);
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
        if(_LightPosition == LightSource.LightSourcePosition.Right)
        {
            transforms[0] = _Vertices[0];
            transforms[1] = _Vertices[1];
            transforms[2] = _Vertices[4];
            transforms[3] = _Vertices[5];
        }
        // 광원이 왼쪽에 존재
        else if (_LightPosition == LightSource.LightSourcePosition.Left)
        {
            transforms[0] = _Vertices[2];
            transforms[1] = _Vertices[3];
            transforms[2] = _Vertices[6];
            transforms[3] = _Vertices[7];
        }
        // 광원이 위쪽에 존재
        else if (_LightPosition == LightSource.LightSourcePosition.Up)
        {
            transforms[0] = _Vertices[1];
            transforms[1] = _Vertices[2];
            transforms[2] = _Vertices[5];
            transforms[3] = _Vertices[6];
        }
        // 광원이 아래쪽에 존재
        else if(_LightPosition == LightSource.LightSourcePosition.Down)
        {
            transforms[0] = _Vertices[0];
            transforms[1] = _Vertices[3];
            transforms[2] = _Vertices[4];
            transforms[3] = _Vertices[7];
        }

        return transforms;
    }
    
    public void SetShadowVerticesPosition(Transform[] shadowCastTransforms, Vector3 lightDirection)
    {
        _ShadowVertices[0].position = shadowCastTransforms[0].position;
        _ShadowVertices[1].position = shadowCastTransforms[1].position;
        _ShadowVertices[2].position = shadowCastTransforms[2].position;
        _ShadowVertices[3].position = shadowCastTransforms[3].position;
        _ShadowVertices[4].position = GroundHitPoint(shadowCastTransforms[0], lightDirection);
        _ShadowVertices[5].position = GroundHitPoint(shadowCastTransforms[1], lightDirection);
    }

    public void CreateShadowMesh(Transform[] shadowCastTransforms)
    {
        Mesh shadowMesh = new Mesh();
        shadowMesh.name = "ShadowMesh";

        shadowMesh.vertices = new Vector3[]
        {
            shadowCastTransforms[0].localPosition,
            shadowCastTransforms[1].localPosition,
            shadowCastTransforms[2].localPosition,
            shadowCastTransforms[3].localPosition,
            shadowCastTransforms[4].localPosition,
            shadowCastTransforms[5].localPosition
        }
        ;


        shadowMesh.triangles = new int[]
        { 
            0,2,1,
            1,2,3,
            0,4,2,
            0,5,4,
            0,1,5,
            1,3,5,
            2,4,5,
            2,5,3,

        }
        ;


        shadowMeshFilter.mesh = shadowMesh;
        collider.sharedMesh = shadowMesh;
    }
    
}
