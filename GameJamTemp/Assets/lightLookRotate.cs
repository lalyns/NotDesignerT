using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightLookRotate : MonoBehaviour
{
    public Transform lightT;
    public GameObject target;


    private void Update()
    {
        lightT.LookAt(target.transform);
    }


}
