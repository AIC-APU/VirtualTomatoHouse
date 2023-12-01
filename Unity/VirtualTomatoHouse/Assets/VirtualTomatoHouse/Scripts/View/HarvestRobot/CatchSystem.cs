using System;
using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.View.HarvestRobot
{
    public class CatchSystem : MonoBehaviour
    {
        [NonSerialized] public Transform Target;

        [Header("Bones")]
        [SerializeField] private Transform _tip;

        public void CatchTarget()
        {
            if (Target == null) return;

            Target.SetParent(null);

            //Targetの重力を無効にする
            var rigidbody = Target.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.useGravity = false;
            }

            //Targetを_tipの子オブジェクトにする
            Target.SetParent(_tip);
        }

        public void ReleaseTarget()
        {
            if (Target == null) return;

            //Targetの重力を有効にする
            var rigidbody = Target.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.useGravity = true;
            }

            //Targetを_tipの子オブジェクトから外す
            Target.SetParent(null);

            Target = null;
        }
    }
}
