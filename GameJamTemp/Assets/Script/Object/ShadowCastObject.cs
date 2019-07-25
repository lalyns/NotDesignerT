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

    GameObject _ShadowFirstObject;
    public GameObject[] _ShadowsFirst;
    GameObject _ShadowSecondObject;
    public GameObject[] _ShadowsSecond;

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
            GameManager.RAY_DISTANCE, 1 << GameManager.LAYER_BLOCK, QueryTriggerInteraction.Ignore);

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

        Vector3 lightDirection = GetLightDirection(_LightSource, _Ground);

        DecideShadowCastPosition(_LightSource);

        bool isGround = true;
        if (_ShadowFirstObject != null)
        {
            isGround &= GroundCheck(_ShadowFirstObject.transform.position, lightDirection);
        }

        _IsConnected = CheckConnected();


        foreach (GameObject shadow in _ShadowsFirst)
        {
            shadow.SetActive(false);
        }

        foreach (GameObject shadow in _ShadowsSecond)
        {
            shadow.SetActive(false);
        }

        if (!_IsConnected && _BlockLevel > 1 && 
            _LightPosition != LightSource.LightSourcePosition.Center)
        {
            SetActiveShadowFirst();

            if (_BlockLevel > 2)
            {
                bool isShadow = true;

                isShadow &= GroundCheck(_ShadowSecondObject.transform.position, Vector3.down);

                if(isShadow)
                    SetActiveShadowSecond();
            }
        }

    }

    public bool GroundCheck(Vector3 start, Vector3 direction)
    {
        bool isGround = false;

        Ray ray = new Ray();
        ray.origin = start;
        ray.direction = direction;

        RaycastHit[] hitAll = Physics.RaycastAll(ray.origin, ray.direction * GameManager.RAY_DISTANCE, 
            GameManager.RAY_DISTANCE, 1 << GameManager.LAYER_BLOCK, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(start, direction * GameManager.RAY_DISTANCE, Color.red);

        foreach (RaycastHit hit in hitAll)
        {
            if (hit.transform == this.transform)
                continue;

            if (hit.transform.tag == "Ground")
                continue;
            

            if (hit.transform != null)
            {
                Debug.Log(hit.transform.name);

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
            _ShadowFirstObject = _ShadowsFirst[3];
            _ShadowSecondObject = _ShadowsSecond[3];
        }
        // 광원이 왼쪽에 존재
        else if (_LightPosition == LightSource.LightSourcePosition.Left)
        {
            _CheckDirection = Vector3.right;
            _ShadowFirstObject = _ShadowsFirst[1];
            _ShadowSecondObject = _ShadowsSecond[1];
        }
        // 광원이 위쪽에 존재
        else if (_LightPosition == LightSource.LightSourcePosition.Up)
        {
            _CheckDirection = Vector3.back;
            _ShadowFirstObject = _ShadowsFirst[2];
            _ShadowSecondObject = _ShadowsSecond[2];
        }
        // 광원이 아래쪽에 존재
        else if(_LightPosition == LightSource.LightSourcePosition.Down)
        {
            _CheckDirection = Vector3.forward;
            _ShadowFirstObject = _ShadowsFirst[0];
            _ShadowSecondObject = _ShadowsSecond[0];
        }
    }
    
    public bool CheckConnected()
    {
        bool IsConnected = false;

        Ray checkRay = new Ray();
        checkRay.origin = transform.position;
        checkRay.direction = _CheckDirection;

        RaycastHit[] hitAll = Physics.RaycastAll(checkRay.origin, checkRay.direction * GameManager.RAY_DISTANCE,
            GameManager.RAY_DISTANCE, 1 << GameManager.LAYER_BLOCK, QueryTriggerInteraction.Ignore);

        foreach(RaycastHit hit in hitAll)
        {
            if (hit.transform == this.transform)
                continue;

            if (hit.transform != null)
                IsConnected = true;
        }

        return IsConnected;
    }

    public void SetActiveShadowFirst()
    {
        _ShadowFirstObject.SetActive(true);
    }

    public void SetActiveShadowSecond()
    {
        _ShadowSecondObject.SetActive(true);
    }
}
