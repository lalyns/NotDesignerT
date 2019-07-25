using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            GetComponent<AudioSource>().Play();
            GetComponentInParent<KeyDoor>()._IsOpen = true;
            this.gameObject.SetActive(false);
        }
    }
}
