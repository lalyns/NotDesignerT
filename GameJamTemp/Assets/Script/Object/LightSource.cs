using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    public LightSourcePosition _LightPosition;
    public float _Direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetPosition();
    }

    void SetPosition()
    {
        if(_LightPosition == LightSourcePosition.Left)
        {
            transform.position = new Vector3(-_Direction, 10, 0);
        }
        else if(_LightPosition == LightSourcePosition.Right)
        {
            transform.position = new Vector3(_Direction, 10, 0);
        }
        if (_LightPosition == LightSourcePosition.Up)
        {
            transform.position = new Vector3(0, 10, _Direction);
        }
        else if (_LightPosition == LightSourcePosition.Down)
        {
            transform.position = new Vector3(0, 10, -_Direction);
        }
    }

    public enum LightSourcePosition
    {
        Left,
        Right,
        Up,
        Down,
    }
}
