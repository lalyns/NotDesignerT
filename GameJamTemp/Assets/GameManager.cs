using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class GameManager : MonoBehaviour
    {
        public const int       LAYER_SHADOW            = 1 << 9;
        public const int       LAYER_BLOCK             = 1 << 10;
        public const int       LAYER_MAPCOLLIDER       = 1 << 12;
        public const int       LAYER_BOX               = 1 << 13;

        public const float     RAY_DISTANCE            = 2f;
    }
}
