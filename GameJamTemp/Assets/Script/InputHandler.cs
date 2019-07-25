using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public LightSource _LightSource;

    public void Awake()
    {
        _LightSource = GameObject.FindGameObjectWithTag("LightSource").GetComponent<LightSource>();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            _LightSource._LightPosition = LightSource.LightSourcePosition.Up;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _LightSource._LightPosition = LightSource.LightSourcePosition.Down;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _LightSource._LightPosition = LightSource.LightSourcePosition.Left;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _LightSource._LightPosition = LightSource.LightSourcePosition.Right;
        }
    }


}
