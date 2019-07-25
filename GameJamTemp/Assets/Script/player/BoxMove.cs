using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMove : MonoBehaviour
{
    [HideInInspector] public bool start;
    [HideInInspector] public float speed;
    [HideInInspector] public Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(start)
        {
            TargetMove();

            if(Vector3.Distance(this.transform.position, target) <= 0)
            {
                start = false;
            }
        }
    }
    
    public void InputData(bool start , float speed , Vector3 target)
    {
        this.start = start;
        this.speed = speed;
        this.target = target;
    }

    void TargetMove()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);

    }
}
