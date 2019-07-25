using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject _CameraParent;
    int _CameraCount = 1000;

    public void Awake()
    {
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _CameraCount--;
            Debug.Log(_CameraCount);
            SetCamera();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _CameraCount++;
            Debug.Log(_CameraCount);
            SetCamera();
        }


    }

    public void SetCamera()
    {
        if (_CameraCount % 4 == 0)
        {
             _CameraParent.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(_CameraCount % 4 == 1)
        {
            _CameraParent.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (_CameraCount % 4 == 2)
        {
            _CameraParent.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (_CameraCount % 4 == 3)
        {
            _CameraParent.transform.rotation = Quaternion.Euler(0, 270, 0);
        }
    }
}
