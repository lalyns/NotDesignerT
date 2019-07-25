using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    public LightSourcePosition _LightPosition;
    public float _Direction;

    public void SetPosition(int x, int y)
    {
        if(x < 0 && y == 0)
        {
            _LightPosition = LightSourcePosition.Left;
            transform.position = new Vector3(-_Direction, 10, 0);
        }
        else if(x > 0 && y == 0)
        {
            _LightPosition = LightSourcePosition.Right;
            transform.position = new Vector3(_Direction, 10, 0);
        }
        else if (y > 0 && x == 0)
        {
            _LightPosition = LightSourcePosition.Up;
            transform.position = new Vector3(0, 10, _Direction);
        }
        else if (y < 0 && x == 0)
        {
            _LightPosition = LightSourcePosition.Down;
            transform.position = new Vector3(0, 10, -_Direction);
        }
        else if(y == 0 && x == 0)
        {
            _LightPosition = LightSourcePosition.Center;
            transform.position = new Vector3(0, 10, 0);
        }
    }

    public enum LightSourcePosition
    {
        Left,
        Right,
        Center,
        Up,
        Down,
    }
}
