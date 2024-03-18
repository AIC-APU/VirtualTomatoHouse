using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.HarvestRobot
{
    public class LiftArmIK : MonoBehaviour
    {
        [Header("Bones")]
        [SerializeField] private Transform _base2;
        [SerializeField] private Transform _base3;

        [Header("Lift")]
        [SerializeField] private Transform _base4;

        [Header("Parameters")]
        [SerializeField] private float _armLength = 0.65f;
        [SerializeField] private float _offset = -0.1f;

        void Update()
        {
            if (_base4.position.y + _offset < 2 * _armLength 
                && _base4.position.y + _offset > 0)
            {
                _base2.localRotation = Quaternion.Euler(0, 0, -GetBase2Angle(_base4));
                _base3.localRotation = Quaternion.Euler(0, 0, GetBase3Angle(_base4));
            }
        }

        private float GetBase2Angle(Transform _base4)
        {
            return Mathf.Asin((_base4.position.y + _offset) / (2f * _armLength)) * Mathf.Rad2Deg;
        }

        private float GetBase3Angle(Transform _base4)
        {
            return 2f * GetBase2Angle(_base4);
        }
    }
}
