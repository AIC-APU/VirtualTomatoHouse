using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Plusplus.VirtualTomatoHouse.Scripts.View.HarvestRobot
{
    public class HoldableTomatoSetterForHarvestRobotManager : MonoBehaviour
    {
        [SerializeField] private HarvestRobotManager _harvestRobotManager;

        private Queue<Transform> _holdableTomatoes = new();

        void Start()
        {
            var holdableTomatoes = 
                GameObject.FindGameObjectsWithTag("HoldableTomato")
                .OrderBy(i => System.Guid.NewGuid())
                .ToArray();

            foreach(var holdableTomato in holdableTomatoes)
            {
                _holdableTomatoes.Enqueue(holdableTomato.transform);
            }
        }

        public void SetTarget()
        {
            if(_holdableTomatoes.Count == 0)
            {
                Debug.Log("holdableTomatoes is empty");
                return;
            }
            else if(_harvestRobotManager.Target != null)
            {
                Debug.Log("target is not null");
                return;
            }
            
            _harvestRobotManager.Target = _holdableTomatoes.Dequeue();
        }
    }
}
