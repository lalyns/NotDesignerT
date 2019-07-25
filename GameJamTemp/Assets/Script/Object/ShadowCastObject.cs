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
    GameObject _ShadowObject;
    public GameObject[] _Shadows;
    MeshFilter _ShadowMeshFilter;
    MeshRenderer _ShadowMeshRenderer;
    MeshCollider _ShadowCollider;

    public GameObject _ShadowTarget;

    public GameObject _TopFloor;

    public void Awake()
    {
        _LightSource = GameObject.FindGameObjectWithTag("LightSource");
        _LightSourceScript = _LightSource.GetComponent<LightSource>();
        _Ground = GameObject.FindGameObjectWithTag("Ground");


        SetLevel();
        //SetVertices();

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

    public void Update()
    {

        _LightPosition = _LightSourceScript._LightPosition;

        DecideShadowCastPosition(_LightSource);

        Vector3 lightDirection = GetLightDirection(_LightSource, _Ground);

        _IsConnected = CheckConnected();

        if (!_IsConnected && _BlockLevel > 0 &&
            _LightPosition != LightSource.LightSourcePosition.Center)
        {
            ShadowMesh();
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
                Debug.DrawRay(start, direction * GameManager.RAY_DISTANCE);
                _ShadowTarget = hit.transform.gameObject;
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

    public void DecideShadowCastPosition(GameObject lightSource)
    {
        foreach(GameObject shadow in _Shadows)
        {
            shadow.SetActive(false);
        }
        // 광원이 오른쪽에 존재
        if(_LightPosition == LightSource.LightSourcePosition.Right)
        {
            _CheckDirection = Vector3.left;
            _ShadowObject = _Shadows[3];
        }
        // 광원이 왼쪽에 존재
        else if (_LightPosition == LightSource.LightSourcePosition.Left)
        {
            _CheckDirection = Vector3.right;
            _ShadowObject = _Shadows[1];
        }
        // 광원이 위쪽에 존재
        else if (_LightPosition == LightSource.LightSourcePosition.Up)
        {
            _CheckDirection = Vector3.forward;
            _ShadowObject = _Shadows[2];
        }
        // 광원이 아래쪽에 존재
        else if(_LightPosition == LightSource.LightSourcePosition.Down)
        {
            _CheckDirection = Vector3.back;
            _ShadowObject = _Shadows[0];
        }
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

    public void ShadowMesh()
    {
        _ShadowObject.SetActive(true);
    }
    
}
