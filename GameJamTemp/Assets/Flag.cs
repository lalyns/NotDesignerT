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
            if (GameManager.GAMESCENE_NUMBER != 5)
            {
                GameManager.GameSceneChange();
            }
            else if(GameManager.GAMESCENE_NUMBER == 5)
            {

            }

            Debug.Log("스테이지 클리어!");
        }
    }
}
