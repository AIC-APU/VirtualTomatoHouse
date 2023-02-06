using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualTomatoHouse.Scripts.UnityComponet.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraFovRandomSetter : MonoBehaviour
    {
        #region Public Fields
        public float MinFov
        {
            get { return minFov; }
            set
            {
                minFov = Mathf.Clamp(value, LIMIT_MIN_FOV, maxFov);
            }
        }

        public float MaxFov
        {
            get { return maxFov; }
            set
            {
                maxFov = Mathf.Clamp(value, minFov, LIMIT_MAX_FOV);
            }
        }
        #endregion

        #region Private Fields
        private UnityEngine.Camera _camera;
        private float minFov = 30f;
        private float maxFov = 60f;
        #endregion

        #region  Const
        const float LIMIT_MIN_FOV = 0.00001f;
        const float LIMIT_MAX_FOV = 179f;
        #endregion

        #region MonoBehaviour Callbacks
        void Awake()
        {
            _camera = GetComponent<UnityEngine.Camera>();
        }
        #endregion

        #region Public method
        public void RandomSet()
        {
            //カメラのfovを指定
            _camera.fieldOfView = Random.Range(minFov, maxFov);
        }
        #endregion
    }
}