using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace Plusplus.VirtualTomatoHouse.Scripts.HarvestRobot
{
    public class HarvestRobotManager : MonoBehaviour
    {
        [NonSerialized] public Transform Target;

        [Header("Transform")]
        [SerializeField] private Transform _basketStandby;
        [SerializeField] private Transform _leftArmStandby;
        [SerializeField] private Transform _rightArmStandby;

        [Header("Robot Systems")]
        [SerializeField] private PIDActuator _pidActuator;
        [SerializeField] private PIDLift _pidLift;
        [SerializeField] private RobotArmIK _leftArmIk;
        [SerializeField] private RobotArmIK _rightArmIk;
        [SerializeField] private CatchSystem _leftCatch;
        [SerializeField] private CatchSystem _rightCatch;

        private Transform _nowTarget = null;

        void Start()
        {
            _leftArmIk.Target = _leftArmStandby;
            _rightArmIk.Target = _rightArmStandby;
        }

        void Update()
        {
            if(Target != null && _nowTarget == null)
            {
                Harvest();
            }
        }

        public async void Harvest()
        {
            if(Target == null)
            {
                Debug.LogError("target is null");
                return;
            }

            if(_nowTarget != null && _nowTarget == Target)
            {
                Debug.LogError("target is same");
                return;
            }
            _nowTarget = Target;

            //移動
            _pidActuator.Target = Target;
            _pidLift.Target = Target;
            await UniTask.WaitUntil(() => _pidActuator.IsReach);
            await UniTask.WaitUntil(() => _pidLift.IsReach);

            //トマトに触れる
            _leftArmIk.Target = Target;
            await UniTask.WaitUntil(() => _leftArmIk.IsReach);

            //待機
            await UniTask.Delay(500);
            
            //トマトをキャッチ
            _leftCatch.Target = Target;
            _leftCatch.CatchTarget();

            //スタンバイ位置に移動
            _leftArmIk.Target = _leftArmStandby;
            await UniTask.WaitUntil(() => _leftArmIk.IsReach);

            //トマトをバスケットに移動
            _leftArmIk.Target = _basketStandby;
            await UniTask.WaitUntil(() => _leftArmIk.IsReach);

            //トマトを離す
            _leftCatch.ReleaseTarget();

            await UniTask.Delay(500);

            //スタンバイ位置に移動
            _leftArmIk.Target = _leftArmStandby;
            await UniTask.WaitUntil(() => _leftArmIk.IsReach);

            _nowTarget = null;
            Target = null;
        }
    }
}
