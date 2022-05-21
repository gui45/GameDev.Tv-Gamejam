using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "Enemies settings")]
    public class EnemiesSettings : ScriptableObject
    {
        [SerializeField]
        private float movementSpeed;
        public float MovementSpeed => movementSpeed;

        [SerializeField]
        private float health;
        public float Health => health;

        [SerializeField]
        private float attack1Damage;
        public float Attack1Damage => attack1Damage;

        [SerializeField]
        private float timeBetweenAttack;
        public float TimeBetweenAttack => timeBetweenAttack;
    }
}