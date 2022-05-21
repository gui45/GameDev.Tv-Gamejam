using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "Player settings")]
    public class PlayerSettings : ScriptableObject
    {
        [Header("General")]

        [SerializeField]
        private float health;
        public float Health => health;

        [Header("Movement")]

        [SerializeField]
        private bool playerStopsWhenKeyUp;
        public bool PlayerStopsWhenKeyUp => playerStopsWhenKeyUp;

        [SerializeField]
        private float movementSpeed;
        public float MovementSpeed => movementSpeed;

        [SerializeField]
        private float jumpForce;
        public float JumpForce => jumpForce;

        [Header("Falling")]

        [SerializeField]
        private bool canMoveWhenFalling;
        public bool CanMoveWhenFalling => canMoveWhenFalling;

        [SerializeField]
        [Range(0,1)]
        private float xSpeedMofierFalling;
        public float XSpeedModiferFalling => xSpeedMofierFalling;

        [SerializeField]
        [Tooltip("delais que onGround = false pour considerer le joueur comme etant entrain de tomber")]
        private float offGroundDelayToFall;
        public float OffGroundDelayToFall => offGroundDelayToFall;

        [Header("Primary attack")]

        [SerializeField]
        private float primaryAttackDamage;
        public float PrimaryAttackDamage => primaryAttackDamage;

        [SerializeField]
        private float primaryAttackCoolDown;
        public float PrimaryAttackCoolDown => primaryAttackCoolDown;

        [Header("Secondary attack")]

        [SerializeField]
        private float secondaryAttackDamage;
        public float SecondaryAttackDamage => secondaryAttackDamage;

        [SerializeField]
        private float secondaryAttackCoolDown;
        public float SecondaryAttackCoolDown => secondaryAttackCoolDown;
    }
}

