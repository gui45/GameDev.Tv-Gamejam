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
        private float attackDamage;
        public float AttackDamage => attackDamage;
    }
}