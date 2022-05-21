using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "Game settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("General")]
        [SerializeField]
        private int fpsLimit;
        public int FpsLimit => fpsLimit;
    }
}
