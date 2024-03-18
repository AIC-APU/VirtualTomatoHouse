using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plusplus.VirtualTomatoHouse.Scripts.Tomato
{
    public abstract class BaseTomatoSetter : MonoBehaviour
    {
        #region Serialized Private Fields
        [SerializeField] protected List<Transform> _locators = new List<Transform>();
        [SerializeField] protected List<GameObject> _tomatoPrefabs = new List<GameObject>();
        #endregion
        
        protected abstract void SetTomato();
    }
}
