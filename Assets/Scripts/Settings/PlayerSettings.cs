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

        [SerializeField]
        private float hurtStaggerDelay;
        public float HurtStaggerDelay => hurtStaggerDelay;

        [SerializeField]
        private AudioClip hurtSound;
        public AudioClip HurtSound => hurtSound;

        [Header("Block")]
        [SerializeField]
        [Range(0,1)]
        private float blockedDamage;
        public float BlockedDamage => blockedDamage;

        [SerializeField]
        [Tooltip("Force * dommage qui arrivent")]
        private float pushBackForce;
        public float PushBackForce => pushBackForce;

        [SerializeField]
        private AudioClip blockedSound;
        public AudioClip BlockedSound => blockedSound;

        [Header("Roll")]
        [SerializeField]
        private float rollDuration;
        public float RollDuration => rollDuration;

        [SerializeField]
        private float rollForce;
        public float RollForce => rollForce;

        [SerializeField]
        private float rollCoolDown;
        public float RollCoolDown => rollCoolDown;

        [SerializeField]
        private AudioClip rollSound;
        public AudioClip RollSound => rollSound;

        [Header("Movement")]

        [SerializeField]
        private bool playerStopsWhenKeyUp;
        public bool PlayerStopsWhenKeyUp => playerStopsWhenKeyUp;

        [SerializeField]
        private float movementSpeed;
        public float MovementSpeed => movementSpeed;

        [SerializeField]
        private AudioClip moveSound;
        public AudioClip MoveSound => moveSound;

        [Header("Jumping")]

        [SerializeField]
        private float jumpForce;
        public float JumpForce => jumpForce;

        [SerializeField]
        private AudioClip jumpSound;
        public AudioClip JumpSound => jumpSound;

        [Header("Falling")]

        [SerializeField]
        private bool canMoveWhenFalling;
        public bool CanMoveWhenFalling => canMoveWhenFalling;

        [SerializeField]
        [Range(0,1)]
        private float xSpeedMofierFalling;
        public float XSpeedModiferFalling => xSpeedMofierFalling;

        [SerializeField]
        private AudioClip landingSound;
        public AudioClip LandingSound => landingSound;

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

        [SerializeField]
        private AudioClip primaryAttackSound;
        public AudioClip PrimaryAttackSound => primaryAttackSound;

        [Header("Secondary attack")]

        [SerializeField]
        private float secondaryAttackDamage;
        public float SecondaryAttackDamage => secondaryAttackDamage;

        [SerializeField]
        private float secondaryAttackCoolDown;
        public float SecondaryAttackCoolDown => secondaryAttackCoolDown;

        [SerializeField]
        private AudioClip secondaryAttackSound;
        public AudioClip SecodaryAttackSound => secondaryAttackSound;
    }
}

