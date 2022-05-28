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
        private float maxTimeAsGhost;
        public float MaxTimeAsGhost => maxTimeAsGhost;

        [Header("Layers")]

        [SerializeField]
        private string playerLayer;
        public string PlayerLayer => playerLayer;

        [SerializeField]
        private string playerInvulnLayer;
        public string PlayerInvulnLayer => playerInvulnLayer;

        [SerializeField]
        private string groundLayer;
        public string GroundLayer => groundLayer;

        [SerializeField]
        private string enemiesLayer;
        public string EnemiesLayer => enemiesLayer;

        [SerializeField]
        private string playerGhostLayer;
        public string PlayerGhostLayer => playerGhostLayer;

        [SerializeField]
        private string deadBodyLayer;
        public string DeadBodyLayer => deadBodyLayer;
    }
}
