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

        [SerializeField]
        private string groundLayer;
        public string GroundLayer => groundLayer;

        [SerializeField]
        private string playerLayer;
        public string PlayerLayer => groundLayer;

        [SerializeField]
        private string enemiesLayer;
        public string EnemiesLayer => enemiesLayer;
    }
}
