using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLibrary;

public class Flag : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            GameManager.GameSceneChange();
            Debug.Log("스테이지 클리어!");
        }
    }
}
