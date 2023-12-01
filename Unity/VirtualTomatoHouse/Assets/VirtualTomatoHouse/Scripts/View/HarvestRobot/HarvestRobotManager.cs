using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Plusplus.VirtualTomatoHouse.Scripts.View.HarvestRobot
{
    public class HarvestRobotManager : MonoBehaviour
    {
        [Header("Transform")]
        public Transform TargetForDebug;
        [SerializeField] private Transform _tomatoBasket;

        [Header("Robot Systems")]
        [SerializeField] private PIDActuator _pidActuator;
        [SerializeField] private DensoBoneIK _densoBoneIk;
        [SerializeField] private CatchSystem _catch;

        private Transform _nowTarget = null;

        public async void Harvest()
        {
            var target = TargetForDebug;

            if(target == null)
            {
                Debug.LogError("target is null");
                return;
            }

            if(_nowTarget != null && _nowTarget == target)
            {
                Debug.LogError("target is same");
                return;
            }
            _nowTarget = target;

            _pidActuator.Target = target;
            await UniTask.WaitUntil(() => _pidActuator.IsReach);

            _densoBoneIk.Target = target;
            await UniTask.WaitUntil(() => _densoBoneIk.IsReach);

            await UniTask.Delay(1000);

            _catch.Target = target;
            _catch.CatchTarget();

            _densoBoneIk.Target = _tomatoBasket;
            await UniTask.WaitUntil(() => _densoBoneIk.IsReach);

            _catch.ReleaseTarget();
        }

    }
}
