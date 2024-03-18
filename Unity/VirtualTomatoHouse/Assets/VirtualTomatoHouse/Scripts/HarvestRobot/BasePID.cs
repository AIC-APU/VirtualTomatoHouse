using System;
using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.HarvestRobot
{
    public abstract class BasePID : MonoBehaviour
    {
        [Header("Traget")]
        [NonSerialized]
        public Transform Target;

        [Header("PID Parameters")]
        [SerializeField] protected float _p = 1f;
        [SerializeField] protected float _i = 0f;
        [SerializeField] protected float _d = 0f;
        [SerializeField] protected float _threshold = 0.1f;
        [SerializeField] protected float _max = 1f;
        [SerializeField] protected float _min = -1f;

        protected float _integral = 0f;
        protected float _previousError = 0f;

        protected bool _isReach = false;

        public bool IsReach => _isReach;

        protected float PIDCalculate(float error, float deltaTime)
        {
            _integral += error * deltaTime;
            var derivative = (error - _previousError) / deltaTime;
            _previousError = error;

            var output = _p * error + _i * _integral + _d * derivative;
            output = Mathf.Clamp(output, _min, _max);

            return output;
        }

        abstract protected void Move(Transform target);

        abstract protected float GetError(Transform target);

        abstract protected bool ReachCheck(Transform target);
    }
}
