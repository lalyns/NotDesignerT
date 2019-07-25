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

    GameObject _ShadowObject;
    public GameObject[] _Shadows;

    public GameObject _ShadowTarget;

    public void Awake()
    {
        _LightSource = GameObject.FindGameObjectWithTag("LightSource");
        _LightSourceScript = _LightSource.GetComponent<LightSource>();
        _Ground = GameObject.FindGameObjectWithTag("Ground");

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

            if (hit.transform.tag == "Ground")
            {
                _BlockLevel = 1;
            }
            else if(hit.transform.tag == "Block")
            {
                _BlockLevel = hit.transform.GetComponent<ShadowCastObject>()._BlockLevel + 1;
                break;
            }
        }
    } 

    public void Update()
    {
        SetLevel();

        _LightPosition = _LightSourceScript._LightPosition;

        DecideShadowCastPosition(_LightSource);

        bool isGround = true;
        if (_ShadowObject != null)
        {
            isGround &= GroundCheck(_ShadowObject.transform.position, Vector3.down);
        }

        Vector3 lightDirection = GetLightDirection(_LightSource, _Ground);

        _IsConnected = CheckConnected();


        foreach (GameObject shadow in _Shadows)
        {
            shadow.SetActive(false);
        }

        if(this.transform.name == "CUBE (11)")
        {
            Debug.Log(isGround);
            Debug.Log(_IsConnected);

        }

        if (!_IsConnected && _BlockLevel > 1 && isGround &&
            _LightPosition != LightSource.LightSourcePosition.Center)
        {
            ShadowMesh();
        }

    }

    public bool GroundCheck(Vector3 start, Vector3 direction)
    {
        bool isGround = false;

        Ray ray = new Ray();
        ray.origin = start;
        ray.direction = direction;

        RaycastHit[] hitAll = Physics.RaycastAll(ray.origin, ray.direction * GameManager.RAY_DISTANCE, 
            GameManager.RAY_DISTANCE, GameManager.LAYER_BLOCK, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(start, direction * GameManager.RAY_DISTANCE, Color.red);

        foreach (RaycastHit hit in hitAll)
        {
            if (hit.transform == this.transform)
                continue;

            if (hit.transform != null)
            {
                _ShadowTarget = hit.transform.gameObject;
                isGround = true;
                break;
            }
        }

        return isGround;
    }

    public Vector3 GetLightDirection(GameObject LightSource, GameObject Ground)
    {
        Vector3 direction = Ground.transform.position - LightSource.transform.position ;
        direction = direction.normalized;

        return direction;
    }

    public void DecideShadowCastPosition(GameObject lightSource)
    {
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
            _CheckDirection = Vector3.back;
            _ShadowObject = _Shadows[2];
        }
        // 광원이 아래쪽에 존재
        else if(_LightPosition == LightSource.LightSourcePosition.Down)
        {
            _CheckDirection = Vector3.forward;
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
