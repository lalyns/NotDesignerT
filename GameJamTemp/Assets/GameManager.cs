using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameLibrary
{
    public class GameManager : MonoBehaviour
    {
        public const int        LAYER_SHADOW            = 9;
        public const int        LAYER_BLOCK             = 10;
        public const int        LAYER_MAPCOLLIDER       = 12;
        public const int        LAYER_BOX               = 13;

        public const float      RAY_DISTANCE            = 2f;

        public static int       GAMESCENE_NUMBER        = 0;

        public static void GameSceneChange()
        {
            SceneManager.LoadScene(++GAMESCENE_NUMBER);
        }

        public static void GameSceneRestart()
        {
            SceneManager.LoadScene(GAMESCENE_NUMBER);
        }

        public static void GameExit()
        {
            Application.Quit();
        }


    }
}
