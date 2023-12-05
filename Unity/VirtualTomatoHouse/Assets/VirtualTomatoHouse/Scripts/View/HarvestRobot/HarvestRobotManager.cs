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

        [Header("Robot Systems")]
        [SerializeField] private PIDActuator _pidActuator;
        [SerializeField] private DensoBoneIK _densoBoneIk;
        [SerializeField] private CatchSystem _catch;

        private Transform _nowTarget = null;

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

            _pidActuator.Target = Target;
            await UniTask.WaitUntil(() => _pidActuator.IsReach);

            _densoBoneIk.Target = Target;
            await UniTask.WaitUntil(() => _densoBoneIk.IsReach);

            await UniTask.Delay(1000);

            _catch.Target = Target;
            _catch.CatchTarget();

            _densoBoneIk.Target = _tomatoBasket;
            await UniTask.WaitUntil(() => _densoBoneIk.IsReach);

            _catch.ReleaseTarget();

            _nowTarget = null;
            Target = null;
        }
    }
}
