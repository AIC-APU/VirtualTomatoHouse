using System;
using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.View.HarvestRobot
{
    public class PIDActuator : MonoBehaviour
    {
        [NonSerialized] public Transform Target;

        [Header("Side PID Parameters")]
        [SerializeField] private float _sideKp = 1f;
        [SerializeField] private float _sideKi = 0.01f;
        [SerializeField] private float _sideKd = 0.1f;
        [SerializeField] private float _sideThreshold = 0.1f;
        [SerializeField] private float _sideMaxSpeed = 1f;
        [SerializeField] private float _sideOffset = 0f;

        private float _sideErrorSum = 0f;
        private float _sideLastError = 0f;


        [Header("Height PID Parameters")]
        [SerializeField] private float _heightKp = 1f;
        [SerializeField] private float _heightKi = 0.01f;
        [SerializeField] private float _heightKd = 0.1f;
        [SerializeField] private float _heightThreshold = 0.1f;
        [SerializeField] private float _heightMaxSpeed = 1f;
        [SerializeField] private float _heightOffset = 0f;

        private float _heightErrorSum = 0f;
        private float _heightLastError = 0f;

        private bool _isSideReach = false;
        private bool _isHeightReach = false;
        public bool IsReach => _isSideReach && _isHeightReach;

        void Update()
        {
            if (Target == null) return;

            if(IsReach)
            {
                Target = null;
                return;
            }

            SideMove(Target.position);

            HeightMove(Target.position);
        }

        private void SideMove(Vector3 targetPosition)
        {
            var offsetVec = transform.right * _sideOffset;
            var targetVector = targetPosition + offsetVec - transform.position;
            var error = Vector3.Dot(targetVector, transform.right);

            if (Mathf.Abs(error) > _sideThreshold)
            {
                var deltaError = (error - _sideLastError) / Time.deltaTime;
                _sideLastError = error;
                _sideErrorSum += error * Time.deltaTime;

                var output = PIDController(error, _sideKp, _sideKi, _sideKd, _sideMaxSpeed, _sideErrorSum, deltaError);

                transform.position += transform.right * output * Time.deltaTime;

                _isSideReach = false;
            }
            else
            {
                _sideLastError = 0f;
                _sideErrorSum = 0f;
                _isSideReach = true;
            }
        }

        private void HeightMove(Vector3 targetPosition)
        {
            var offsetVec = transform.up * _heightOffset;
            var targetVector = targetPosition + offsetVec - transform.position;
            var error = Vector3.Dot(targetVector, transform.up);

            if (Mathf.Abs(error) > _heightThreshold)
            {
                var deltaError = (error - _heightLastError) / Time.deltaTime;
                _heightLastError = error;
                _heightErrorSum += error * Time.deltaTime;

                var output = PIDController(error, _heightKp, _heightKi, _heightKd, _heightMaxSpeed, _heightErrorSum, deltaError);

                //高さを0から1に制限
                var heihgt = transform.position + transform.up * output * Time.deltaTime;
                heihgt.y = Mathf.Clamp(heihgt.y, 0f, 1f);
                transform.position = heihgt;

                _isHeightReach = false;
            }
            else
            {
                _heightLastError = 0f;
                _heightErrorSum = 0f;

                _isHeightReach = true;
            }
        }

        private float PIDController(float error, float kp, float ki, float kd, float max, float errorsum = 0, float deltaError = 0)
        {
            //PID制御
            var p = error * kp;
            var i = errorsum * ki;
            var d = deltaError * kd;

            //出力
            var output = p + i + d;

            //出力を制限
            output = Mathf.Clamp(output, -max, max);

            return output;
        }
    }
}
