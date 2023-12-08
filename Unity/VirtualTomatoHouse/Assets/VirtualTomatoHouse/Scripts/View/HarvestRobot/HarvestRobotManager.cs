using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace Plusplus.VirtualTomatoHouse.Scripts.View.HarvestRobot
{
    public class HarvestRobotManager : MonoBehaviour
    {
        [NonSerialized] public Transform Target;

        [Header("Transform")]
        [SerializeField] private Transform _tomatoBasket;
        [SerializeField] private Transform _standbyPosition;

        [Header("Robot Systems")]
        [SerializeField] private PIDActuator _pidActuator;
        [SerializeField] private RobotArmIK _densoBoneIk;
        [SerializeField] private CatchSystem _catch;

        private Transform _nowTarget = null;

        void Start()
        {
            _densoBoneIk.Target = _standbyPosition;
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
            await UniTask.WaitUntil(() => _pidActuator.IsReach);

            //トマトに触れる
            _densoBoneIk.Target = Target;
            await UniTask.WaitUntil(() => _densoBoneIk.IsReach);
            Debug.Log("reach");

            //待機
            await UniTask.Delay(500);
            
            //トマトをキャッチ
            _catch.Target = Target;
            _catch.CatchTarget();
            Debug.Log("catch");

            //スタンバイ位置に移動
            _densoBoneIk.Target = _standbyPosition;
            await UniTask.WaitUntil(() => _densoBoneIk.IsReach);

            //トマトをバスケットに移動
            _densoBoneIk.Target = _tomatoBasket;
            await UniTask.WaitUntil(() => _densoBoneIk.IsReach);

            //トマトを離す
            _catch.ReleaseTarget();

            await UniTask.Delay(500);

            //スタンバイ位置に移動
            _densoBoneIk.Target = _standbyPosition;
            await UniTask.WaitUntil(() => _densoBoneIk.IsReach);

            _nowTarget = null;
            Target = null;
        }
    }
}
