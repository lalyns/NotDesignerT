using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLibrary;

public class ShadowCastObject : MonoBehaviour
{
    public int _BlockLevel;

    public Transform _VertexParent;
    Transform[] _Vertices;

    GameObject _LightSource;
    LightSource _LightSourceScript;
    LightSource.LightSourcePosition _LightPosition;
    GameObject _Ground;

    Vector3 _CheckDirection;
    bool _IsConnected;

    Transform[] _ShadowVertices;
    public GameObject _ShadowObject;
    MeshFilter _ShadowMeshFilter;
    MeshRenderer _ShadowMeshRenderer;
    MeshCollider _ShadowCollider;

    public GameObject _TopFloor;

    public void Awake()
    {
        _LightSource = GameObject.FindGameObjectWithTag("LightSource");
        _LightSourceScript = _LightSource.GetComponent<LightSource>();
        _Ground = GameObject.FindGameObjectWithTag("Ground");

        _ShadowMeshFilter = _ShadowObject.GetComponent<MeshFilter>();
        _ShadowMeshRenderer = _ShadowObject.GetComponent<MeshRenderer>();
        _ShadowCollider = _ShadowObject.GetComponent<MeshCollider>();

        SetLevel();
        SetVertices();

    }

    public void SetLevel()
    {
        Ray ray = new Ray();
        ray.origin = this.transform.position;
        ray.direction = Vector3.down;

        RaycastHit[] hitAll = Physics.RaycastAll(ray.origin, ray.direction * GameManager.RAY_DISTANCE,
            GameManager.RAY_DISTANCE, GameManager.LAYER_BLOCK, QueryTriggerInteraction.Ignore);

        foreach (RaycastHit hit in hitAll)
        {
            if (hit.transform == this.transform)
            {
                continue;
            }

            if (hit.transform.GetComponent<ShadowCastObject>() == null)
            {
                _BlockLevel = 0;
            }
            else
            {
                _BlockLevel = hit.transform.GetComponent<ShadowCastObject>()._BlockLevel + 1;
                break;
            }
        }
    } 

    public bool SetTopFloor()
    {
        bool setFloor = false;

        Ray ray = new Ray();
        ray.origin = this.transform.position;
        ray.direction = Vector3.up;

        RaycastHit[] hitAll = Physics.RaycastAll(ray.origin, ray.direction * GameManager.RAY_DISTANCE,
            GameManager.RAY_DISTANCE, GameManager.LAYER_BLOCK, QueryTriggerInteraction.Ignore);

        if(hitAll.Length == 0)
        {
            setFloor = true;
        }

        return setFloor;
    }

    public void SetVertices()
    {
        Transform[] temp1 = _VertexParent.GetComponentsInChildren<Transform>();

        _Vertices = new Transform[temp1.Length - 1];
        int i = 0;

        foreach (Transform vertex in temp1)
        {
            if (vertex == _VertexParent)
                continue;
            else
            {
                _Vertices[i++] = vertex;
            }

        }

        Transform[] temp2 = _ShadowObject.GetComponentsInChildren<Transform>();
        _ShadowVertices = new Transform[temp2.Length - 1];
        i = 0;
        foreach (Transform shadowVertex in temp2)
        {
            if (shadowVertex == _ShadowObject.transform)
                continue;
            else
            {
                _ShadowVertices[i++] = shadowVertex;
            }

        }
    }

    public void Update()
    {
        //_TopFloor.SetActive(SetTopFloor());

        _ShadowMeshFilter.mesh = null;
        _ShadowCollider.sharedMesh = null;

        _LightPosition = _LightSourceScript._LightPosition;

        Vector3[] shadowCastTransforms = DecideShadowCastPosition(_LightSource);
        Vector3 lightDirection = GetLightDirection(_LightSource, _Ground);

        SetShadowVerticesPosition(shadowCastTransforms, lightDirection);

        _IsConnected = CheckConnected();

        if (!_IsConnected && _BlockLevel > 0 &&
            _LightPosition != LightSource.LightSourcePosition.Center)
        {
            CreateShadowMesh(_ShadowVertices);
        }

    }

    public Vector3 GroundHitPoint(Vector3 start, Vector3 direction)
    {
        Vector3 hitPoint = Vector3.zero;

        Ray ray = new Ray();
        ray.origin = start;
        ray.direction = direction;

        RaycastHit[] hitAll = Physics.RaycastAll(ray.origin, ray.direction * GameManager.RAY_DISTANCE, 
            GameManager.RAY_DISTANCE, GameManager.LAYER_BLOCK, QueryTriggerInteraction.Ignore);

        foreach(RaycastHit hit in hitAll)
        {
            if (hit.transform == this.transform)
                continue;
            if (hit.transform != null)
            {
                Debug.Log(this.transform.name + " -> " + hit.transform.name);
                hitPoint = hit.point;

                break;
            }
        }
            
        return hitPoint;
    }

    public Vector3 GetLightDirection(GameObject LightSource, GameObject Ground)
    {
        Vector3 direction = Ground.transform.position - LightSource.transform.position ;
        direction = direction.normalized;

        return direction;
    }

    public Vector3[] DecideShadowCastPosition(GameObject lightSource)
    {
        Vector3[] positions = new Vector3[4];

        // 광원이 오른쪽에 존재
        if(_LightPosition == LightSource.LightSourcePosition.Right)
        {
            positions[0] = _Vertices[0].position;
            positions[1] = _Vertices[1].position;
            positions[2] = _Vertices[4].position;
            positions[3] = _Vertices[5].position;
            _CheckDirection = Vector3.left;
        }
        // 광원이 왼쪽에 존재
        else if (_LightPosition == LightSource.LightSourcePosition.Left)
        {
            positions[0] = _Vertices[2].position;
            positions[1] = _Vertices[3].position;
            positions[2] = _Vertices[6].position;
            positions[3] = _Vertices[7].position;
            _CheckDirection = Vector3.right;
        }
        // 광원이 위쪽에 존재
        else if (_LightPosition == LightSource.LightSourcePosition.Up)
        {
            positions[0] = _Vertices[1].position;
            positions[1] = _Vertices[2].position;
            positions[2] = _Vertices[5].position;
            positions[3] = _Vertices[6].position;
            _CheckDirection = Vector3.forward;
        }
        // 광원이 아래쪽에 존재
        else if(_LightPosition == LightSource.LightSourcePosition.Down)
        {
            positions[0] = _Vertices[0].position;
            positions[1] = _Vertices[3].position;
            positions[2] = _Vertices[4].position;
            positions[3] = _Vertices[7].position;
            _CheckDirection = Vector3.back;
        }

        return positions;
    }
    
    public bool CheckConnected()
    {
        bool IsConnected = false;

        Ray checkRay = new Ray();
        checkRay.origin = transform.position;
        checkRay.direction = _CheckDirection;

        RaycastHit[] hitAll = Physics.RaycastAll(checkRay.origin, checkRay.direction * GameManager.RAY_DISTANCE,
            GameManager.RAY_DISTANCE, GameManager.LAYER_BLOCK, QueryTriggerInteraction.Ignore);

        foreach(RaycastHit hit in hitAll)
        {
            if (hit.transform == this.transform)
                continue;

            if (hit.transform != null)
                IsConnected = true;
        }

        return IsConnected;
    }

    public void SetShadowVerticesPosition(Vector3[] vector3s, Vector3 lightDirection)
    {
        _ShadowVertices[0].position = vector3s[0];
        _ShadowVertices[1].position = vector3s[1];
        _ShadowVertices[2].position = vector3s[2];
        _ShadowVertices[3].position = vector3s[3];
        _ShadowVertices[4].position = GroundHitPoint(vector3s[0], lightDirection);
        _ShadowVertices[5].position = GroundHitPoint(vector3s[1], lightDirection);
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
            0,1,2,
            1,2,3,
            1,3,2,
            0,4,2,
            0,2,4,
            0,5,4,
            0,4,5,
            0,1,5,
            0,5,1,
            1,3,5,
            1,5,3,
            2,4,5,
            2,5,4,
            2,5,3,
            2,3,5
            };

        _ShadowMeshFilter.mesh = shadowMesh;
        _ShadowCollider.sharedMesh = shadowMesh;
    }
    
}
