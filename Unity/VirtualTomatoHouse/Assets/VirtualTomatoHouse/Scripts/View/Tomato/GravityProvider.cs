using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.View.Tomato
{
    public class GravityProvider : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Collider _collider;
        private Vector3 _startPosition;
        private bool _isGravityProvided = false;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _startPosition = transform.position;
        }

        void Update()
        {
            if(_isGravityProvided) return;

            if(Vector3.Distance(_startPosition, transform.position) > 0.01f)
            {
                ProvideGravity();
                _isGravityProvided = true;
            }
        }

        private void ProvideGravity()
        {
            _rigidbody.useGravity = true;
            _collider.isTrigger = false;
        }
    }
}
