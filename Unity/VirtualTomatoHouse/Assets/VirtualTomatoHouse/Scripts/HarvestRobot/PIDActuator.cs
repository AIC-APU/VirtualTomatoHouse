using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.HarvestRobot
{
    public class PIDActuator : BasePID
    {
        [Header("Actuator")]
        [SerializeField] private float _sideOffset = 0f;

        void Update()
        {
            if (Target == null) return;

            if (ReachCheck(Target))
            {
                Target = null;
                _previousError = 0f;
                _integral = 0f;
                _isReach = true;
            }
            else
            {
                _isReach = false;
                Move(Target);
            }
        }

        protected override void Move(Transform target)
        {
            var error = GetError(target);

            var output = PIDCalculate(error, Time.deltaTime);

            //1フレーム分の移動量を計算
            transform.position += output * Time.deltaTime * transform.right;
        }

        protected override bool ReachCheck(Transform target)
        {
            if (target == null) return false;

            var error = GetError(target);
            return Mathf.Abs(error) < _threshold;
        }

        protected override float GetError(Transform target)
        {
            var offsetVec = transform.right * _sideOffset;
            var targetVector = target.position + offsetVec - transform.position;
            return Vector3.Dot(targetVector, transform.right);
        }
    }
}
