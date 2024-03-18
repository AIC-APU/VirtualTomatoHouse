using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.HarvestRobot
{
    public class PIDLift : BasePID
    {
        [Header("Lift")]
        [SerializeField] private float _heightOffset = 0f;
        [SerializeField] private float _maxHeight = 1.8f;
        [SerializeField] private float _minHeight = 0f;

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
            var heihgt = transform.position + output * Time.deltaTime * transform.up;
            heihgt.y = Mathf.Clamp(heihgt.y, _minHeight, _maxHeight);
            transform.position = heihgt;
        }

        protected override bool ReachCheck(Transform target)
        {
            if(target == null) return false;

            var error = GetError(target);
            return Mathf.Abs(error) < _threshold;
        }

        protected override float GetError(Transform target)
        {
            var offsetVec = transform.up * _heightOffset;
            var targetVector = target.position + offsetVec - transform.position;
            return Vector3.Dot(targetVector, transform.up);
        }
    }
}
