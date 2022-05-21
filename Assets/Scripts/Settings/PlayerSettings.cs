using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "Player settings")]
    public class PlayerSettings : ScriptableObject
    {
        [SerializeField]
        private float jumpForce;
        public float JumpForce => jumpForce;

        [SerializeField]
        private float movementSpeed;
        public float MovementSpeed => movementSpeed;

        [SerializeField]
        private float health;
        public float Health => health;
    }
}

