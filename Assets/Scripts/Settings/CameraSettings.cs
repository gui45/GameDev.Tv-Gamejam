using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "Camera settings")]
    public class CameraSettings : ScriptableObject
    {
        [SerializeField]
        private float returnDelay;
        public float ReturnDelay => returnDelay;
    }
}

