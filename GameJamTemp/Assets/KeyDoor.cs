using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public GameObject _Door;
    public bool _IsOpen = false;

    public float _Speed = 10f;
    float time;

    void Update()
    {
        if (_IsOpen)
        {
            if (time < 3f)
            {
                _Door.transform.position -= new Vector3(0f, _Speed, 0f) * Time.deltaTime;
                time += Time.deltaTime;
            }

            else if(time >= 3f)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
