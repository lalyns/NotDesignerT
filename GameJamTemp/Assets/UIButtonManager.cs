using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLibrary;

public class UIButtonManager : MonoBehaviour
{
    public void StartButton()
    {
        GameManager.GameSceneChange();
    }

    public void ExitButton()
    {
        GameManager.GameExit();
    }
}
