using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public LightSource _LightSource;

    int xAxis;
    int yAxis;

    public void Awake()
    {
        _LightSource = GameObject.FindGameObjectWithTag("LightSource").GetComponent<LightSource>();

        xAxis = 0;
        yAxis = 0;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            yAxis += 1;

            if (xAxis > 0 || xAxis < 0)
                xAxis = 0;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            yAxis -= 1;

            if (xAxis > 0 || xAxis < 0)
                xAxis = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            xAxis -= 1;

            if (yAxis > 0 || yAxis < 0)
                yAxis = 0;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            xAxis += 1;

            if (yAxis > 0 || yAxis < 0)
                yAxis = 0;
        }

        xAxis = (int)Mathf.Clamp(xAxis, -1, 1);
        yAxis = (int)Mathf.Clamp(yAxis, -1, 1);

        _LightSource.SetPosition(xAxis, yAxis);
    }


}
