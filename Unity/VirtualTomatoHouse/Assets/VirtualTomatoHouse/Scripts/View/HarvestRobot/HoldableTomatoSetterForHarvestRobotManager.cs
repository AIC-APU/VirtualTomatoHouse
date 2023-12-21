using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Plusplus.VirtualTomatoHouse.Scripts.View.HarvestRobot
{
    public class HoldableTomatoSetterForHarvestRobotManager : MonoBehaviour
    {
        [SerializeField] private HarvestRobotManager _harvestRobotManager;
        [SerializeField] private bool _autoMode = false;

        private Queue<Transform> _holdableTomatoes = new();

        private Vector3 _standardPosition = new Vector3(50f, 100f, -100f);

        void Start()
        {
            var holdableTomatoes = 
                GameObject.FindGameObjectsWithTag("HoldableTomato")
                .OrderBy(i => Vector3.Distance(i.transform.position, _standardPosition))
                .ToArray();

            foreach(var holdableTomato in holdableTomatoes)
            {
                _holdableTomatoes.Enqueue(holdableTomato.transform);
            }
        }
    
        void Update()
        {
            if(_autoMode
            && _holdableTomatoes.Count > 0
            && _harvestRobotManager.Target == null)
            {
                SetTarget();
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
