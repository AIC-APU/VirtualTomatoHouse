using UnityEngine;
using System.Collections.Generic;

namespace Plusplus.VirtualTomatoHouse.Scripts.View.Camera
{
    public abstract class BaseCameraSetter : MonoBehaviour
    {
        #region Public Fields
        [Header("Camera")]
        public UnityEngine.Camera _camera;

        [Header("Range Boxes")]
        public List<Transform> _rangeBoxObjects;
        #endregion

        #region Public Properties
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

        #region Protected Fields
        [Header("Fov Range")]
        [SerializeField] protected float minFov = 30f;
        [SerializeField] protected float maxFov = 60f;
        #endregion

        #region  Const
        protected const float LIMIT_MIN_FOV = 0.00001f;
        protected const float LIMIT_MAX_FOV = 179f;
        #endregion

        public abstract void SetCamera();
        protected abstract void SetCameraPosAndAng(UnityEngine.Camera camera);
        protected abstract void SetCameraFov(UnityEngine.Camera camera);
    }
}
