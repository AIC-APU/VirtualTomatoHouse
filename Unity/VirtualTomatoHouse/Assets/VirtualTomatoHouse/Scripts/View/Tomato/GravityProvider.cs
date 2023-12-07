using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.View.Tomato
{
    public class GravityProvider : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Collider _collider;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        void OnTriggerEnter(Collider other)
        {
            if(_rigidbody.useGravity) return;
            if (other.gameObject.CompareTag("GravityArea"))
            {
                ProvideGravity();
            }
        }

        public void ProvideGravity()
        {
            _rigidbody.useGravity = true;
            _collider.isTrigger = false;
        }
    }
}
