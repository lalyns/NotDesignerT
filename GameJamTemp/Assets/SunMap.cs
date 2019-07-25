using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunMap : MonoBehaviour
{
    public Image _Sun;
    public Transform[] _Positions;

    LightSource _LightSource;
    LightSource.LightSourcePosition _LightPosition;

    // Start is called before the first frame update
    void Start()
    {
        _LightSource = GameObject.FindGameObjectWithTag("LightSource").GetComponent<LightSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _LightPosition = _LightSource._LightPosition;
        if(_LightPosition == LightSource.LightSourcePosition.Center)
        {
            _Sun.rectTransform.position = _Positions[0].position;
        }
        if (_LightPosition == LightSource.LightSourcePosition.Left)
        {
            _Sun.rectTransform.position = _Positions[1].position;
        }
        if (_LightPosition == LightSource.LightSourcePosition.Right)
        {
            _Sun.rectTransform.position = _Positions[2].position;
        }
        if (_LightPosition == LightSource.LightSourcePosition.Down)
        {
            _Sun.rectTransform.position = _Positions[3].position;
        }
        if (_LightPosition == LightSource.LightSourcePosition.Up)
        {
            _Sun.rectTransform.position = _Positions[4].position;
        }
    }
}
