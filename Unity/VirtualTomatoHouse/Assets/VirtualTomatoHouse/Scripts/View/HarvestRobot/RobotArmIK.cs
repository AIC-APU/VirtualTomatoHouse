using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.View.HarvestRobot
{
    public class RobotArmIK : MonoBehaviour
    {
        //[NonSerialized] 
        public Transform Target;
        
        [Header("Bones")]
        [SerializeField] private Transform _bone1; //Y only
        [SerializeField] private Transform _bone2; //X only
        [SerializeField] private Transform _bone3; //X only
        [SerializeField] private Transform _tip;

        [Header("Parameters")]
        [SerializeField] private float _bone1Speed = 1f;
        [SerializeField] private float _bone2Speed = 1f;
        [SerializeField] private float _bone3Speed = 1f;
        [SerializeField] private float _bone3OffsetAngle = 0f;
        [SerializeField] private float _threshold = 0.1f;

        private bool _isReach = false;
        public bool IsReach => _isReach;

        private float _length1;
        private float _length2;

        void Start()
        {
            _length1 = Vector3.Distance(_bone2.position, _bone3.position);
            _length2 = Vector3.Distance(_bone3.position, _tip.position);
        }

        void Update()
        {
            if (Target == null) return;

            //targetに到達していたら何もしない
            if (Vector3.Distance(Target.position, _tip.position) < _threshold)
            {
                _isReach = true;
                Target = null;
                return;
            }
            _isReach = false;



            //アームが届く範囲にあればbone2とbone3を動かす
            var distance = Vector3.Distance(Target.position, _bone2.position);
            var straight = (Target.position - _bone2.position).normalized;
            if (distance > Mathf.Abs(_length2 - _length1) && distance < _length1 + _length2)
            {
                //bone1をtargetに向ける
                //アームが水平に設置されている場合に限る
                var bone1Angle = GetBone1Angle(_bone1, Target);
                var bone1GoalRotation = Quaternion.Euler(0, bone1Angle, 0);
                _bone1.rotation = Quaternion.Slerp(_bone1.rotation, bone1GoalRotation, _bone1Speed * Time.deltaTime);

                //straightのx軸回転成分のみを取得
                float straightAngle = Mathf.Atan2(straight.y, Mathf.Sqrt(straight.x * straight.x + straight.z * straight.z)) * Mathf.Rad2Deg;

                //余弦定理でbone2の角度を求める
                float bone2InteriorAngle = Mathf.Acos((_length1 * _length1 + distance * distance - _length2 * _length2) / (2 * _length1 * distance)) * Mathf.Rad2Deg;
                float bone2Angle = 90f - (straightAngle + bone2InteriorAngle);
                bone2Angle = Mathf.Clamp(bone2Angle, -90, 90);

                //bone2のx軸回転を適用
                var bone2GoalRotation = Quaternion.Euler(bone2Angle, 0, 0);
                _bone2.localRotation = Quaternion.Slerp(_bone2.localRotation, bone2GoalRotation, _bone2Speed * Time.deltaTime);


                //余弦定理でbone3の角度を求める
                var bone3InteriorAngle = Mathf.Acos((_length1 * _length1 + _length2 * _length2 - distance * distance) / (2 * _length1 * _length2)) * Mathf.Rad2Deg;
                float bone3Angle = 90f - (bone3InteriorAngle + _bone3OffsetAngle);
                bone3Angle = Mathf.Clamp(bone3Angle, -210, 80);

                //bone3のx軸回転を適用
                var bone3GoalRotation = Quaternion.Euler(bone3Angle, 0, 0);
                _bone3.localRotation = Quaternion.Slerp(_bone3.localRotation, bone3GoalRotation, _bone3Speed * Time.deltaTime);
            }
        }

        private float GetBone1Angle(Transform bone1, Transform target)
        {
            var targetDirection = (target.position - bone1.position).normalized;
            var angle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
            return angle;
        }

        public static float GetNormalizedAngle(float angle)
        {
            angle %= 360;

            if (angle < 0)
                angle += 360;

            return angle;
        }
    }
}
